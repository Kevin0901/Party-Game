using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using Firebase;
using Firebase.Database;
public class Launcher : MonoBehaviourPunCallbacks
{
    public static Launcher Instance;
    string PlayerName;
    [Header("創建房間名字輸入")]
    [SerializeField] TMP_InputField roomNameInputField;
    [Header("玩家名字顯示")]
    [SerializeField] TMP_Text UserNameText;
    [Header("錯誤訊息(連線失敗)")]
    [SerializeField] TMP_Text errorText;
    [Header("房間名稱")]
    public TMP_Text roomNameText;
    [Header("查找房間 UI")]
    [SerializeField] Transform roomListContent;
    [SerializeField] GameObject roomListItemPrefab;
    [Header("開始遊戲按鈕")]
    [SerializeField] GameObject StartGameButton;
    [Header("ChoosePlayer UI")]
    [SerializeField] GameObject CP;
    [Header("各個玩家的 UI ")]
    [SerializeField] GameObject[] PlayerMenus;
    public DatabaseReference reference;
    public Room Room;
    [Header("房間管理器")]
    [SerializeField] RoomManager RoomManager;
    [Header("是否已經登入")]
    public bool isLogin;
    PhotonView PV;
    bool isStart;
    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        PV = GetComponent<PhotonView>();
        isLogin = false;
        isStart = false;
        reference = FirebaseDatabase.DefaultInstance.RootReference;  //定義資料庫連接
        if (GameObject.Find("RoomManager") != null)
        {
            Destroy(GameObject.Find("RoomManager"));
        }
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }
        if (!PhotonNetwork.IsConnected)
        {
            Debug.Log("Connecting to Master");
            // PhotonNetwork.AuthValues = new AuthenticationValues("123456789");
            PhotonNetwork.ConnectUsingSettings();  //開啟連線
        }
    }

    public override void OnConnectedToMaster()  //連線成功
    {
        Debug.Log("Connected to Master");
        PhotonNetwork.JoinLobby();  //加入大廳
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnJoinedLobby()  //已加入大廳
    {
        if (isLogin)
        {
            // MenuManger.Instance.OpenMenu("title");  //打開 TitleMenu UI
        }
        else
        {
            // MenuManger.Instance.OpenMenu("start");  //打開 StartMenu UI
        }

        Debug.Log("Joined Lobby");
        PhotonNetwork.NickName = PlayerPrefs.GetString("username");
        RoomManager.gameObject.SetActive(true);
    }

    public void CreateRoom()  //創建房間(按鈕觸發)
    {
        if (string.IsNullOrEmpty(roomNameInputField.text))  //如果沒輸入房間名稱則不給創建
        {
            return;
        }
        PhotonNetwork.CreateRoom(roomNameInputField.text, new RoomOptions { CleanupCacheOnLeave = false, IsVisible = true });  //創建房間
        // MenuManger.Instance.OpenMenu("loading");  //打開 LoadingMenu UI
    }

    public override void OnJoinedRoom()  //進入房間後執行以下程式碼
    {
        // MenuManger.Instance.OpenMenu("room");  //打開 Room UI (ChoosePlayer)
        Room = PhotonNetwork.CurrentRoom;
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;  //設定房間名稱

        Player[] players = PhotonNetwork.PlayerList;  //取得已加入房間的玩家陣列

        PlayerName = PhotonNetwork.NickName;  //取得自身名字

        if (PhotonNetwork.IsMasterClient)  //如果是房主(第一位進到房間的)
        {
            //在資料庫創建 玩家代號 資料表
            Order NewOrder = new Order(PlayerName, "Empty", "Empty", "Empty");
            string OrderJson = JsonUtility.ToJson(NewOrder);
            reference.Child("GameRoom").Child(PhotonNetwork.CurrentRoom.Name).Child("Order").SetRawJsonValueAsync(OrderJson);
        }

        AddPlayer(players);  //在資料庫創建並紀錄 玩家基本資料

        //假如沒人退出過房間，就依玩家進入順序給入相對應的玩家代號到資料庫
        if (PhotonNetwork.LocalPlayer.ActorNumber == players.Length)
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber == 2)
            {
                reference.Child("GameRoom").Child(PhotonNetwork.CurrentRoom.Name).Child("Order").Child("B").SetValueAsync(PlayerName);  //P2
            }
            else if (PhotonNetwork.LocalPlayer.ActorNumber == 3)
            {
                reference.Child("GameRoom").Child(PhotonNetwork.CurrentRoom.Name).Child("Order").Child("C").SetValueAsync(PlayerName);  //P3
            }
            else if (PhotonNetwork.LocalPlayer.ActorNumber == 4)
            {
                reference.Child("GameRoom").Child(PhotonNetwork.CurrentRoom.Name).Child("Order").Child("D").SetValueAsync(PlayerName);  //P4
            }
            SetPlayerUI();

        }
        else  //如果有人退出過房間，就執行以下程式碼
        {
            StartCoroutine(GetRoomInfo((DataSnapshot info) =>
            {
                foreach (var rules in info.Children)  //Order，PlayList
                {
                    foreach (var item in rules.Children)  //A，B，C，D，[PlayerName]
                    {
                        if (item.ChildrenCount == 0)  //只有 Order 的 A，B，C，D 進入以下程式碼
                        {
                            if (item.Value.ToString().Equals("Empty"))  //找出空位子，然後把自己替補進去
                            {
                                //寫入資料庫
                                reference.Child("GameRoom").Child(PhotonNetwork.CurrentRoom.Name).Child("Order").Child(item.Key.ToString()).SetValueAsync(PhotonNetwork.NickName);
                                SetPlayerUI();  //設定 UI
                                break;
                            }
                            else
                            {
                                continue;  //如果不是 Empty，就直接往下一個查詢
                            }
                        }
                        else
                        {
                            break;  //如果進到 PlayList 的 [PlayerName]，直接 Break 出去
                        }
                    }
                }
            }));
        }
        StartGameButton.SetActive(PhotonNetwork.IsMasterClient);  //如果是房主，才會顯示開始按鈕
    }
    void AddPlayer(Player[] players)
    {
        //在資料庫創建並紀錄 玩家基本資料
        User NewUser = new User("red", PhotonNetwork.LocalPlayer.ActorNumber.ToString());
        string Json = JsonUtility.ToJson(NewUser);
        reference.Child("GameRoom").Child(PhotonNetwork.CurrentRoom.Name).Child("PlayerList").Child(PlayerName).SetRawJsonValueAsync(Json);
    }

    IEnumerator OneSec_CatchDataBase()
    {
        yield return new WaitForSeconds(0.1f);
        SetPlayerUI();
    }

    public void SetPlayerUI()
    {
        Player[] players = PhotonNetwork.PlayerList;  //取得已加入房間的玩家陣列
        StartCoroutine(GetRoomInfo((DataSnapshot info) =>  //從資料庫抓取此房間內的所有資料
        {
            string[] PName = new string[4];  //臨時存放玩家的陣列 (照 P1、P2...順序)
            int cnt = 0;  //計數器，紀錄放了幾個玩家名字在 PName[]

            foreach (var rules in info.Children)  //Order，PlayList
            {
                foreach (var item in rules.Children)  //A，B，C，D，[PlayerName]
                {
                    if (item.ChildrenCount == 0)  //只有 Order 的 A，B，C，D 進入以下程式碼
                    {
                        // // Debug.Log(item.Value.ToString());
                        // if (!item.Value.ToString().Equals("Empty"))  //如果不是空的，就把該玩家名字加入 PName[]
                        // {
                        if (cnt < 4)
                        {
                            PName[cnt] = item.Value.ToString();
                            cnt++;
                        }
                        // }
                        // else
                        // {
                        //     continue;  //空的就直接 continue
                        // }
                    }
                    else  //進到 PlayList 的 [PlayerName]
                    {
                        string team = "";  //臨時儲存 team 
                        for (int i = 0; i < PName.Length; i++)
                        {
                            if (item.Key.ToString().Equals(PName[i]))  //抓取 [PlayerName] 內的 team
                            {
                                foreach (var Pinfo in item.Children)
                                {
                                    if (Pinfo.Key.ToString().Equals("team"))
                                    {
                                        team = Pinfo.Value.ToString();
                                    }
                                }
                                GameObject PlayerUI = PlayerMenus[i];  // 取得玩家 UI
                                PlayerUI.transform.Find("Text (TMP)").gameObject.SetActive(true);  //打開玩家名稱 UI
                                for (int j = 0; j < players.Length; j++)
                                {
                                    if (players[j].NickName.Equals(PName[i]) && !isStart)
                                    {
                                        PlayerUI.GetComponent<PlayerListItem>().SetUp(players[j]);  //設定玩家名稱 UI
                                        if (!PlayerUI.GetComponent<PhotonView>().Controller.NickName.Equals(players[j]))
                                        {
                                            PlayerUI.GetComponent<PhotonView>().TransferOwnership(players[j]);  //設定 PhotonView Owner (表示只有 XXX 有此 UI 的擁有權)
                                        }
                                    }
                                }
                                if (team.Equals("red"))  // Team判斷
                                {
                                    PlayerUI.GetComponent<TeamSelect>().red = true;
                                    PlayerUI.GetComponent<TeamSelect>().blue = false;
                                }
                                else
                                {
                                    PlayerUI.GetComponent<TeamSelect>().blue = true;
                                    PlayerUI.GetComponent<TeamSelect>().red = false;
                                }
                                PlayerUI.GetComponent<TeamSelect>().ArrowStart();
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < PName.Length; i++)
            {
                if (PName[i].Equals("Empty"))
                {
                    GameObject PlayerUI = PlayerMenus[i];  // 取得玩家 UI
                    TeamSelect t = PlayerUI.gameObject.GetComponent<TeamSelect>();
                    t.blue = false;
                    t.red = false;
                    for (int j = 0; j < PlayerUI.transform.childCount; j++)  //關閉所有子物件
                    {
                        PlayerUI.transform.GetChild(j).gameObject.SetActive(false);
                    }
                }
            }
            StartCoroutine(OneSec_CatchDataBase());
        }));
    }
    IEnumerator GetRoomInfo(System.Action<DataSnapshot> onCallbacks)  //從資料庫抓取此房間的所有資料
    {
        if (PhotonNetwork.CurrentRoom != null)
        {
            var userData = reference.Child("GameRoom").Child(PhotonNetwork.CurrentRoom.Name).GetValueAsync();

            yield return new WaitUntil(predicate: () => userData.IsCompleted);

            if (userData != null)
            {
                DataSnapshot snapshot = userData.Result;
                onCallbacks.Invoke(snapshot);
            }
        }
    }

    public override void OnMasterClientSwitched(Player newMasterClient)  //如果房主換人
    {
        StartGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)  //如果創建房間錯誤
    {
        errorText.text = "Room Creation Failed:" + message;
        // MenuManger.Instance.OpenMenu("error");
    }

    public void StartGame()  //開始遊戲 (按鈕觸發)
    {
        SavePlayer_And_Team();
    }

    void SavePlayer_And_Team()  //傳所有玩家的名字以及隊伍，到 RoomManager
    {
        bool HaveRed = false;
        bool HaveBlue = false;
        Player[] players = PhotonNetwork.PlayerList;
        RoomManager.PlayerNames = new string[players.Length];
        RoomManager.PlayerTeam = new string[players.Length];
        string[] PlayerNames = RoomManager.PlayerNames;
        string[] PlayerTeam = RoomManager.PlayerTeam;
        for (int i = 0; i < players.Length; i++)
        {
            GameObject pUI = CP.transform.Find("P" + (i + 1) + "Join").gameObject;
            PlayerNames[i] = pUI.GetComponent<PlayerListItem>().text.text.ToString();
            if (pUI.GetComponent<TeamSelect>().red)
            {
                PlayerTeam[i] = "red";
                HaveRed = true;
            }
            else if (pUI.GetComponent<TeamSelect>().blue)
            {
                PlayerTeam[i] = "blue";
                HaveBlue = true;
            }
        }
        if (HaveRed && HaveBlue)
        {
            isStart = true;
            StopCoroutine(OneSec_CatchDataBase());
            StopCoroutine(GetRoomInfo((DataSnapshot info) => { }));
            Room.IsVisible = false;
            reference.Child("GameRoom").Child(PhotonNetwork.CurrentRoom.Name).Child("Time").Child("TotalTime").SetValueAsync(0);
            PV.RPC("RPC_fadeout", RpcTarget.All);
        }
        else
        {
            GameObject.Find("ChoosePlayer").GetComponent<ChoosePlayer>().StartCoroutine("Warning");
        }
    }
    [PunRPC]
    void RPC_fadeout()
    {
        StartCoroutine(fadeout());
    }
    private IEnumerator fadeout() //淡出畫面
    {
        GameObject.Find("ChoosePlayer").GetComponent<CanvasGroup>().blocksRaycasts = false;
        GameObject.Find("TranPageAnimation").GetComponent<Animator>().SetTrigger("change");
        yield return new WaitForSeconds(0.55f);
        GameObject.Find("ChoosePlayer").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("music").SetActive(false);
        if (PV.IsMine)
        {
            PhotonNetwork.LoadLevel(2);
        }
    }

    public void LeaveRoom()  //離開房間 (按鈕觸發)
    {
        PhotonNetwork.LeaveRoom();
    }

    public void JoinRoom(RoomInfo info)  //加入房間
    {
        PhotonNetwork.JoinRoom(info.Name);
        Debug.Log("Join");
        // MenuManger.Instance.OpenMenu("loading");
    }
    public override void OnLeftRoom()  //離開房間觸發
    {
        // MenuManger.Instance.OpenMenu("title");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)  //房間列表更新
    {
        for (int i = 0; i < roomList.Count; i++)
        {
            bool isInst = false;
            if (roomList[i].RemovedFromList)
            {
                for (int j = 0; j < roomListContent.transform.childCount; j++)
                {
                    if (roomListContent.transform.GetChild(j).GetComponent<RoomListItem>().info.Name.Equals(roomList[i].Name))
                    {
                        Destroy(roomListContent.transform.GetChild(j).gameObject);
                    }
                }
                continue;
            }
            foreach (Transform trans in roomListContent)
            {
                if (trans.GetComponent<RoomListItem>().info.Name.Equals(roomList[i].Name))
                {
                    isInst = true;
                }
            }
            if (!isInst)
            {
                Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(roomList[i]);
            }
        }
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)  //當新玩家進入房間
    {
        // StartCoroutine(AddNewPlayerUI(newPlayer));
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            PhotonNetwork.RemoveRPCs(PhotonNetwork.PlayerList[i]);
        }
    }

    public IEnumerator AddNewPlayerUI(Player newPlayer)  //新增新玩家的 UI
    {
        yield return new WaitForSeconds(1f);
        bool isAdd_NewPlayer = false;
        Player[] players = PhotonNetwork.PlayerList;  //取得已加入房間的玩家陣列

        StartCoroutine(GetRoomInfo((DataSnapshot info) =>  //從資料庫抓取此房間內的所有資料
        {
            foreach (var rules in info.Children)  //Order，PlayList
            {
                foreach (var item in rules.Children)  //A，B，C，D，[PlayerName]
                {
                    if (item.ChildrenCount == 0)  //只有 Order 的 A，B，C，D 進入以下程式碼
                    {
                        if (item.Value.ToString().Equals(newPlayer.NickName))  //抓取新玩家的位置 (P 幾)，抓完生成 UI
                        {
                            int pos = 0;
                            switch (item.Key)
                            {
                                case "A":
                                    pos = 0;
                                    break;
                                case "B":
                                    pos = 1;
                                    break;
                                case "C":
                                    pos = 2;
                                    break;
                                case "D":
                                    pos = 3;
                                    break;
                            }
                            GameObject PlayerUI = PlayerMenus[pos];
                            PlayerUI.transform.Find("Text (TMP)").gameObject.SetActive(true);
                            PlayerUI.GetComponent<PlayerListItem>().SetUp(newPlayer);
                            PlayerUI.GetComponent<PhotonView>().TransferOwnership(newPlayer);
                            PlayerUI.GetComponent<TeamSelect>().red = true;
                            PlayerUI.GetComponent<TeamSelect>().blue = false;
                            PlayerUI.GetComponent<TeamSelect>().ArrowStart();
                            isAdd_NewPlayer = true;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }

            if (!isAdd_NewPlayer)
            {
                StartCoroutine(AddNewPlayerUI(newPlayer));
            }
        }));
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        // reference.Child("Account_Online").Child(PlayerPrefs.GetString("username")).SetValueAsync(null);
    }
}

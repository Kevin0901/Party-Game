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
    string otherPlayerName;
    string orderName;
    [Header("玩家英文代號( One 代表 P1 )")]
    [SerializeField] string[] OrderList;
    [Header("創建房間名字輸入")]
    [SerializeField] TMP_InputField roomNameInputField;
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
    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;  //定義資料庫連接
        Debug.Log("Connecting to Master");
        PhotonNetwork.ConnectUsingSettings();  //開啟連線
    }

    public override void OnConnectedToMaster()  //連線成功
    {
        Debug.Log("Connected to Master");
        PhotonNetwork.JoinLobby();  //加入大廳
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnJoinedLobby()  //已加入大廳
    {
        MenuManger.Instance.OpenMenu("title");  //打開 TitleMenu UI
        Debug.Log("Joined Lobby");
    }

    public void CreateRoom()  //創建房間(按鈕觸發)
    {
        if (string.IsNullOrEmpty(roomNameInputField.text))  //如果沒輸入房間名稱則不給創建
        {
            return;
        }
        PhotonNetwork.CreateRoom(roomNameInputField.text);  //創建房間
        MenuManger.Instance.OpenMenu("loading");  //打開 LoadingMenu UI
    }

    public override void OnJoinedRoom()  //進入房間後執行以下程式碼
    {
        MenuManger.Instance.OpenMenu("room");  //打開 Room UI (ChoosePlayer)
        Room = PhotonNetwork.CurrentRoom;
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;  //設定房間名稱

        Player[] players = PhotonNetwork.PlayerList;  //取得已加入房間的玩家陣列

        PlayerName = players[players.Length - 1].NickName;  //取得自身名字

        if (PhotonNetwork.IsMasterClient)  //如果是房主(第一位進到房間的)
        {
            //在資料庫創建 玩家代號 資料表
            Order NewOrder = new Order(PlayerName, "Empty", "Empty", "Empty");
            string OrderJson = JsonUtility.ToJson(NewOrder);
            reference.Child("GameRoom").Child(PhotonNetwork.CurrentRoom.Name).Child("Order").SetRawJsonValueAsync(OrderJson);
        }

        AddPlayer(players);  //在資料庫創建並紀錄 玩家基本資料

        //假如沒人退出過房間，就依玩家進入順序給入相對應的玩家代號到資料庫
        if (players[players.Length - 1].ActorNumber == players.Length)
        {
            if (players[players.Length - 1].ActorNumber == 2)
            {
                reference.Child("GameRoom").Child(PhotonNetwork.CurrentRoom.Name).Child("Order").Child("Two").SetValueAsync(PlayerName);
            }
            else if (players[players.Length - 1].ActorNumber == 3)
            {
                reference.Child("GameRoom").Child(PhotonNetwork.CurrentRoom.Name).Child("Order").Child("Three").SetValueAsync(PlayerName);
            }
            else if (players[players.Length - 1].ActorNumber == 4)
            {
                reference.Child("GameRoom").Child(PhotonNetwork.CurrentRoom.Name).Child("Order").Child("Four").SetValueAsync(PlayerName);
            }

            StartCoroutine(SetPlayerUI());  //進入房間後，依資料庫的玩家資料創建相對應的 UI
        }
        else
        {
            StartCoroutine(FillVacancyOrder_And_SetPlayer(players));  //有人退出過此房間，就執行此方法
        }
        StartGameButton.SetActive(PhotonNetwork.IsMasterClient);  //如果是房主，才會顯示開始按鈕
    }

    IEnumerator FillVacancyOrder_And_SetPlayer(Player[] players)
    {
        //簡單來說，把我自己的代號設為沒人使用的玩家代號，EX.在資料庫中， Two 顯示 Empty，就把我的玩家代號設為 Two (P2)
        bool isAdd = false;
        for (int i = 0; i < OrderList.Length; i++)
        {
            orderName = OrderList[i];
            //從資料庫一一抓取各個玩家代號
            StartCoroutine(GetOrder((string order) =>
            {
                if (order.Equals("Empty"))  //如果玩家代號內顯示 Empty
                {
                    //把我自己的玩家代號設為此玩家代號
                    reference.Child("GameRoom").Child(PhotonNetwork.CurrentRoom.Name).Child("Order").Child(orderName).SetValueAsync(PlayerName);
                    isAdd = true;
                }
            }));
            yield return new WaitForSeconds(0.5f);  //每隔 0.5s 讀取下一筆，防止錯誤

            if (isAdd)
            {
                //代號設定完後，依資料庫的玩家資料創建相對應的 UI
                StartCoroutine(SetPlayerUI());
                break;
            }
        }
    }

    void AddPlayer(Player[] players)
    {
        //在資料庫創建並紀錄 玩家基本資料
        User NewUser = new User("red", players[players.Length - 1].ActorNumber.ToString());
        string Json = JsonUtility.ToJson(NewUser);
        reference.Child("GameRoom").Child(PhotonNetwork.CurrentRoom.Name).Child("PlayerList").Child(PlayerName).SetRawJsonValueAsync(Json);
        //GR
        //  - RoomName
        //     -- PN
        //        -team
        //        
    }

    public IEnumerator SetPlayerUI()
    {
        Player[] players = PhotonNetwork.PlayerList;  //取得已加入房間的玩家陣列

        for (int i = 0; i < OrderList.Length; i++)
        {
            orderName = OrderList[i];
            StartCoroutine(GetOrder((string order) =>  //從資料庫抓取各個玩家代號
            {
                if (!(order.Equals("Empty")))  //如果玩家代號內的值不為 Empty (表示有玩家)
                {
                    otherPlayerName = order;
                    StartCoroutine(GetTeam((string team) =>  //取得該玩家在資料庫內，的隊伍資料(red 或 blue)
                    {
                        GameObject PlayerUI = PlayerMenus[i];  // 取得玩家 UI
                        PlayerUI.transform.Find("Text (TMP)").gameObject.SetActive(true);  //打開玩家名稱 UI
                        for (int j = 0; j < players.Length; j++)
                        {
                            if (players[j].NickName.Equals(order))
                            {
                                PlayerUI.GetComponent<PlayerListItem>().SetUp(players[j]);  //設定玩家名稱 UI
                                PlayerUI.GetComponent<PhotonView>().TransferOwnership(players[j]);  //設定 PhotonView Owner (表示只有 XXX 有此 UI 的擁有權)
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
                    }));
                }
            }));

            yield return new WaitForSeconds(0.5f); //每隔 0.5s 讀取下一筆，防止錯誤
        }
    }
    public IEnumerator GetTeam(System.Action<string> onCallbacks) //從資料庫讀取玩家 Team
    {
        var userData = reference.Child("GameRoom").Child(PhotonNetwork.CurrentRoom.Name).Child("PlayerList").Child(otherPlayerName).Child("team").GetValueAsync();

        yield return new WaitUntil(predicate: () => userData.IsCompleted);

        if (userData != null)
        {
            DataSnapshot snapshot = userData.Result;
            onCallbacks.Invoke(snapshot.Value.ToString());
        }
    }

    public IEnumerator GetOrder(System.Action<string> onCallback)  //從資料庫讀取玩家代號
    {
        var OrderData = reference.Child("GameRoom").Child(PhotonNetwork.CurrentRoom.Name).Child("Order").Child(orderName).GetValueAsync();

        yield return new WaitUntil(predicate: () => OrderData.IsCompleted);

        if (OrderData != null)
        {
            DataSnapshot snapshot = OrderData.Result;
            onCallback.Invoke(snapshot.Value.ToString());
        }
    }

    public override void OnMasterClientSwitched(Player newMasterClient)  //如果房主換人
    {
        StartGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)  //如果創建房間錯誤
    {
        errorText.text = "Room Creation Failed:" + message;
        MenuManger.Instance.OpenMenu("error");
    }

    public void StartGame()  //開始遊戲 (按鈕觸發)
    {
        SavePlayer_And_Team();
        PhotonNetwork.LoadLevel(1);
    }

    void SavePlayer_And_Team()  //傳所有玩家的名字以及隊伍，到 RoomManager
    {
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
            }
            else if (pUI.GetComponent<TeamSelect>().blue)
            {
                PlayerTeam[i] = "blue";
            }
        }
    }

    public void LeaveRoom()  //離開房間 (按鈕觸發)
    {
        PhotonNetwork.LeaveRoom();
    }

    public void JoinRoom(RoomInfo info)  //加入房間
    {
        PhotonNetwork.JoinRoom(info.Name);
        MenuManger.Instance.OpenMenu("loading");
    }
    public override void OnLeftRoom()  //離開房間觸發
    {
        MenuManger.Instance.OpenMenu("title");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)  //房間列表更新
    {
        foreach (Transform trans in roomListContent)
        {
            Destroy(trans.gameObject);
        }
        for (int i = 0; i < roomList.Count; i++)
        {
            if (roomList[i].RemovedFromList)
                continue;
            if (roomList[i].PlayerCount >= 4)
            {
                continue;
            }
            Instantiate(roomListItemPrefab, roomListContent).GetComponent<RoomListItem>().SetUp(roomList[i]);
        }
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)  //當新玩家進入房間
    {
        StartCoroutine(AddNewPlayerUI(newPlayer));
    }

    public IEnumerator AddNewPlayerUI(Player newPlayer)  //新增新玩家的 UI
    {
        yield return new WaitForSeconds(2f);

        Player[] players = PhotonNetwork.PlayerList;

        for (int i = 0; i < OrderList.Length; i++)
        {
            orderName = OrderList[i];
            StartCoroutine(GetOrder((string order) =>
            {
                if (order.Equals(newPlayer.NickName))
                {
                    otherPlayerName = order;
                    StartCoroutine(GetTeam((string team) =>
                    {
                        GameObject PlayerUI = PlayerMenus[i];
                        PlayerUI.transform.Find("Text (TMP)").gameObject.SetActive(true);
                        for (int j = 0; j < players.Length; j++)
                        {
                            if (players[j].NickName.Equals(order))
                            {
                                PlayerUI.GetComponent<PlayerListItem>().SetUp(players[j]);
                                PlayerUI.GetComponent<PhotonView>().TransferOwnership(players[j]);
                            }
                        }
                        if (team.Equals("red"))
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
                    }));
                }
            }));
            yield return new WaitForSeconds(0.5f);
        }
    }
}

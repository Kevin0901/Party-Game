using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.IO;
using System;
using System.Linq;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using TMPro;
using UnityEngine.UI;
using Firebase;
using Firebase.Database;
public class RoomManager : MonoBehaviourPunCallbacks
{
    // public static RoomManager Instance;
    [Header("玩家名稱陣列")]
    public string[] PlayerNames;
    [Header("玩家隊伍陣列")]
    public string[] PlayerTeam;
    PhotonView PV;
    public int Game_num = -1;
    public int TotalTime, ReadyTime = 5, EventTime;
    private int nextEvent;
    bool EnteredGame;
    GameObject PAPA;
    GameObject Black, Event;
    [SerializeField] TMP_Text Count;
    [SerializeField] Image EventPicture;
    [SerializeField] Sprite[] EventSprite;
    [SerializeField] GameObject BlueCastle, RedCastle;
    [SerializeField] GameObject Teaching;
    DatabaseReference reference;
    public string WinTeam = "";
    GameObject WaitPool;
    void Awake()
    {
        EnteredGame = false;
        TotalTime = 0;
        Black = transform.Find("Black").gameObject;
        Event = transform.Find("Event").gameObject;
        // if (Instance)
        // {
        //     Destroy(gameObject);
        //     return;
        // }
        DontDestroyOnLoad(gameObject);
        PV = GetComponent<PhotonView>();  //定義PhotonView
        // Instance = this;
        reference = FirebaseDatabase.DefaultInstance.RootReference;  //定義資料庫連接
    }
    private void Start()
    {
        nextEvent = EventTime;
    }
    void Update()
    {
        if (!PV.IsMine)
        {
            return;
        }
        if (Teaching != null && Input.GetKeyDown(KeyCode.Escape))
        {
            if (Teaching.activeSelf)
            {
                Cursor.visible = false;
                Teaching.SetActive(false);
            }
            else
            {
                Cursor.visible = true;
                Teaching.SetActive(true);
            }
        }
        if (SceneManager.GetActiveScene().name.Equals("MainScene") && Input.inputString.Length > 0 && Input.inputString.All(char.IsDigit))
        {
            // switch (Input.inputString)
            // {
            //     case "0":
            //         Game_num = 0;
            //         break;
            //     case "1":
            //         Game_num = 1;
            //         break;
            //     case "2":
            //         Game_num = 2;
            //         break;
            //     case "3":
            //         Game_num = 3;
            //         break;
            //     case "4":
            //         Game_num = 4;
            //         break;
            //     case "5":
            //         Game_num = 5;
            //         break;
            //     case "6":
            //         Game_num = 6;
            //         break;
            //     case "7":
            //         Game_num = 7;
            //         break;
            //     case "8":
            //         Game_num = 8;
            //         break;
            //     case "9":
            //         Game_num = 9;
            //         break;
            // }
            // Hashtable hash = new Hashtable();
            // hash.Add("GameNum", Game_num);
            // PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
            // PhotonNetwork.LoadLevel(2);
        }
    }
    IEnumerator ready()
    {
        if (ReadyTime == 0)
        {
            Count.text = "Start!!!";
            yield return new WaitForSeconds(0.5f);
            Count.gameObject.SetActive(false);
            StartCoroutine(TimeCount());
        }
        else
        {
            Count.gameObject.SetActive(true);
            Count.text = ReadyTime.ToString();
            ReadyTime--;
            yield return new WaitForSeconds(1f);
            StartCoroutine(ready());
        }
    }
    IEnumerator TimeCount()
    {
        if (PV.IsMine)
        {
            TotalTime++;
            reference.Child("GameRoom").Child(PhotonNetwork.CurrentRoom.Name).Child("Time").Child("TotalTime").SetValueAsync(TotalTime);
        }
        yield return new WaitForSeconds(1f);
        if (PV.IsMine)
        {
            NextEvent_Trigger();
        }
        else
        {
            StartCoroutine(GetTimeInfo((DataSnapshot info) =>  //從資料庫抓取此房間內的所有資料
            {
                foreach (var Time in info.Children)
                {
                    TotalTime = (int)Int64.Parse(Time.Value.ToString());
                    NextEvent_Trigger();
                }
            }));
        }
    }
    void NextEvent_Trigger()
    {
        if (TotalTime == nextEvent)
        {
            if (PV.IsMine)
            {
                Game_num++;
                // Game_num = UnityEngine.Random.Range(0, EventSprite.Length);
                if (Game_num == 9)
                {
                    Game_num = 0;
                }
                EventPicture.sprite = EventSprite[Game_num];
                EventPicture.gameObject.transform.parent.gameObject.SetActive(true);
                Hashtable hash = new Hashtable();
                hash.Add("GameNum", Game_num);
                PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
            }
            StartCoroutine(startEvent(10));
        }
        if (SceneManager.GetActiveScene().name.Equals("MainScene"))
        {
            StartCoroutine(TimeCount());
        }
    }

    IEnumerator GetTimeInfo(System.Action<DataSnapshot> onCallbacks)  //從資料庫抓取此房間的所有資料
    {
        var userData = reference.Child("GameRoom").Child(PhotonNetwork.CurrentRoom.Name).Child("Time").GetValueAsync();

        yield return new WaitUntil(predicate: () => userData.IsCompleted);

        if (userData != null)
        {
            DataSnapshot snapshot = userData.Result;
            onCallbacks.Invoke(snapshot);
        }
    }

    IEnumerator startEvent(float t)
    {
        if (t > 5)
        {
            t--;
            yield return new WaitForSeconds(1f);
            StartCoroutine(startEvent(t));
        }
        else if (t == 0)
        {
            Count.gameObject.SetActive(false);
            nextEvent += EventTime;
            StartCoroutine(Black_fadein());
        }
        else
        {
            Count.gameObject.SetActive(true);
            Debug.Log(t);
            Count.text = t.ToString();
            t--;
            yield return new WaitForSeconds(1f);
            StartCoroutine(startEvent(t));
        }
    }
    void Black_fadeout() //離開黑色
    {
        Black.GetComponentInChildren<Animator>().SetTrigger("fadeout");
    }
    IEnumerator Black_fadein() //變成黑色
    {
        if (0 <= Game_num && Game_num <= 8)
        {
            Black.GetComponentInChildren<Animator>().SetTrigger("fadein");
        }
        yield return new WaitForSeconds(1f);
        EventPicture.gameObject.transform.parent.gameObject.SetActive(false);
        if (PV.IsMine)
        {
            if (SceneManager.GetActiveScene().name.Equals("MainScene"))
            {
                if (0 <= Game_num && Game_num <= 8)
                {
                    StopCoroutine(TimeCount());
                    PhotonNetwork.LoadLevel(3);
                }
                else
                {
                    switch (Game_num)
                    {
                        case 9:  //大洪水
                            Vector3 tsupos = new Vector3(-75f, 1f, 0.0f);
                            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "MainEvent/Tsunami"), tsupos, Quaternion.Euler(0f, 0f, -90f));
                            break;
                        case 10:  //大地震
                            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "MainEvent/Earthquake"), new Vector3(0, 0, 0), this.transform.rotation);
                            break;
                        case 11:  //潘朵拉盒子
                            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "MainEvent/PandoraBox"), new Vector3(-20, 0, 0), this.transform.rotation);
                            break;
                        case 12:  //金毛羊
                            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "MainEvent/GoldenSheep"), new Vector3(-20, 0, 0), this.transform.rotation);
                            break;
                        case 13:  //特洛伊戰爭
                            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "MainEvent/GoldenApple"), new Vector3(-20, 0, 0), this.transform.rotation);
                            break;
                        case 14:  //黑帝斯
                            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "MainEvent/CrystalThief"), new Vector3(-20, 0, 0), this.transform.rotation);
                            break;
                    }
                }
            }
            else if (SceneManager.GetActiveScene().name.Equals("FightScene"))
            {
                WinTeam = GameObject.Find("EndGameUI").GetComponent<EndGame>().WinTeam;
                // if (WinTeam != null)
                // {
                    // PhotonNetwork.LoadLevel(2);
                // }
                Game_num++;
                if (Game_num == 9)
                {
                    Game_num = 0;
                }
                if (PV.IsMine)
                {
                    PhotonNetwork.LoadLevel(3);
                }
                

            }
        }
    }
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (!PV.IsMine)
        {
            Game_num = (int)changedProps["GameNum"];
            // EventPicture.sprite = EventSprite[Game_num];
            // EventPicture.gameObject.transform.parent.gameObject.SetActive(true);
        }
    }
    public override void OnEnable()
    {
        base.OnEnable();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (SceneManager.GetActiveScene().name.Equals("MainScene")) //如果在遊戲場景
        {
            WaitPool = GameObject.Find("LoadSceneCompletePool");
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "LoadComplete"), Vector3.zero, Quaternion.identity);
            StartCoroutine(Wait_OtherPlayer_LoadMainScene());
        }
        else if (SceneManager.GetActiveScene().name.Equals("FightScene"))
        {
            Cursor.visible = true;
            WaitPool = GameObject.Find("LoadFightSceneCompletePool");
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "LoadComplete"), Vector3.zero, Quaternion.identity);
            // PAPA.SetActive(false);
            StartCoroutine(Wait_OtherPlayer_LoadFightScene());
        }
    }

    [PunRPC]  //廣播的方法前面都需要加這個開頭
    void RPC_SetArryList(string[] PN, string[] PT)  //設定 PlayerNames[] 跟 PlayerTeam[]
    {
        // if (PV.IsMine)  //如果是房主的 RoomManager，直接 Skip
        // {
        //     PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerManager"), Vector3.zero, Quaternion.identity);
        //     return;
        // }
        PlayerNames = PN;
        PlayerTeam = PT;
        //每個玩家實例化自己的 PlayerManager
        // GameObject Manager = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerManager"), Vector3.zero, Quaternion.identity);
    }

    IEnumerator Wait_OtherPlayer_LoadFightScene()
    {
        if (WaitPool.transform.childCount == (PhotonNetwork.PlayerList.Length + 1))
        {
            WaitPool.transform.Find("Canvas").gameObject.SetActive(false);
            Black_fadeout();
            if (PV.IsMine)
            {
                for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
                {
                    PhotonNetwork.RemoveRPCs(PhotonNetwork.PlayerList[i]);
                }
                PV.RPC("RPC_SetArryList", RpcTarget.All, PlayerNames, PlayerTeam);  //廣播到所有玩家的電腦，設定 PlayerNames[] 跟 PlayerTeam[]
                GameObject RM = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "arena/FightManager"), Vector3.zero, Quaternion.identity);

            }
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(Wait_OtherPlayer_LoadFightScene());
        }
    }

    IEnumerator Wait_OtherPlayer_LoadMainScene()
    {
        if (WaitPool.transform.childCount == (PhotonNetwork.PlayerList.Length + 1))
        {
            WaitPool.transform.Find("Canvas").gameObject.SetActive(false);
            Cursor.visible = false;
            if (!EnteredGame)
            {
                Black_fadeout();
                StartCoroutine(ready());
                PAPA = GameObject.Find("PAPA");
                Teaching = PAPA.transform.Find("Teaching").gameObject;
                if (PV.IsMine)
                {
                    for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
                    {
                        PhotonNetwork.RemoveRPCs(PhotonNetwork.PlayerList[i]);
                    }
                    PV.RPC("RPC_SetArryList", RpcTarget.All, PlayerNames, PlayerTeam);  //廣播到所有玩家的電腦，設定 PlayerNames[] 跟 PlayerTeam[]
                    GameObject Red = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "RedCastle"), new Vector3(-20.4f, -45.49f, 0), this.transform.rotation);
                    GameObject Blue = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "BlueCastle"), new Vector3(-20.4f, 45.49f, 0), this.transform.rotation);
                }
                EnteredGame = true;
                PAPA.GetComponent<PAPA>().OpenChild();
            }
            else
            {
                if (PV.IsMine)
                {
                    reference.Child("GameRoom").Child(PhotonNetwork.CurrentRoom.Name).Child("Arena").Child("Health").SetValueAsync(null);
                }
                Black_fadeout();
                StartCoroutine(TimeCount());
                if (GameObject.Find("PAPA") != null)
                {
                    Destroy(GameObject.Find("PAPA"));
                    PAPA.SetActive(true);
                    ResourceTypeListSO resourceTypeList = Resources.Load<ResourceTypeListSO>(typeof(ResourceTypeListSO).Name);
                    if (WinTeam == "red")
                    {
                        ResourceManager.Instance.RedAddResource(resourceTypeList.list[0], 200);
                        ResourceManager.Instance.RedAddResource(resourceTypeList.list[1], 200);
                        ResourceManager.Instance.RedAddResource(resourceTypeList.list[2], 200);
                        WinTeam = null;
                    }
                    else if (WinTeam == "blue")
                    {
                        ResourceManager.Instance.BlueAddResource(resourceTypeList.list[0], 200);
                        ResourceManager.Instance.BlueAddResource(resourceTypeList.list[1], 200);
                        ResourceManager.Instance.BlueAddResource(resourceTypeList.list[2], 200);
                        WinTeam = null;
                    }
                }
            }
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(Wait_OtherPlayer_LoadMainScene());
        }
    }

    public void Back_To_Main()
    {
        Destroy(PAPA);
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();
        SceneManager.LoadScene("MainUI");
    }
}

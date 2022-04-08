using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using System.IO;
using Photon.Realtime;
public class FightManager : MonoBehaviour
{
    public static FightManager Instance;
    [SerializeField] public int game_num;
    public List<GameObject> plist;
    public List<bool> redOrBlue; //紀錄是紅隊還是藍隊
    [SerializeField] private GameObject _player;
    PhotonView PV;
    //靜態實例基本宣告
    private void Awake()
    {
        redOrBlue = new List<bool>();
        plist = new List<GameObject>();
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        // DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        redOrBlue = new List<bool>();
        plist = new List<GameObject>();
        PV = GetComponent<PhotonView>();  //定義PhotonView

        string[] Tlist = GameObject.Find("RoomManager").GetComponent<RoomManager>().PlayerTeam;
        for (int i = 0; i < Tlist.Length; i++)
        {
            Debug.Log(Tlist.Length);
            if (Tlist[i].Equals("red"))
            {
                Debug.Log("true");
                redOrBlue.Add(true);
            }
            else
            {
                Debug.Log("false");
                redOrBlue.Add(false);
            }
        }

        int max = GameObject.Find("EventManager").transform.childCount;
        if (PV.IsMine)
        {
            StartCoroutine(Wait(max));
        }
    }
    IEnumerator Wait(int m)
    {
        yield return new WaitForSeconds(0.2f);
        // game_num = 0;
        playerSet();
        GameObject.Find("EventManager").transform.GetChild(game_num).gameObject.SetActive(true);//開啟該場景
        PV.RPC("RPC_SetGame_Num", RpcTarget.All, game_num);  //廣播到所有玩家的電腦，同步 game_num
    }

    [PunRPC]  //廣播的方法前面都需要加這個開頭
    void RPC_SetGame_Num(int num)  //設定 game_num
    {
        if (PV.IsMine)  //如果是房主的 FightManager，直接 Skip
        {
            return;
        }
        game_num = num;
        StartCoroutine(Wait_arenaPlayer());
    }
    IEnumerator Wait_arenaPlayer()
    {
        if (redOrBlue.Count == FightManager.Instance.plist.Count)
        {
            for (int i = 0; i < plist.Count; i++)
            {
                plist[i].GetComponent<arenaPlayer>().p_index = i;
                plist[i].GetComponent<arenaPlayer>().red = redOrBlue[i];
                plist[i].GetComponent<arenaPlayer>().setUI();
            }
            GameObject.Find("EventManager").transform.GetChild(game_num).gameObject.SetActive(true);//開啟該場景
        }
        else
        {
            yield return new WaitForSeconds(0.05f);
            StartCoroutine(Wait_arenaPlayer());
        }
    }
    // private void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.O) && redOrBlue.Count < 3)
    //     {
    //         redOrBlue.Add(false);
    //         GameObject.Find("ChoosePlayer").transform.Find("P" + redOrBlue.Count).gameObject.SetActive(true);
    //     }

    // }
    //開始遊戲(點擊事件)
    public void joinGame()
    {
        redOrBlue.Add(false);
        GameObject.Find("ChoosePlayer").transform.Find("P" + redOrBlue.Count).gameObject.SetActive(true);
    }
    // public void startGame()
    // {
    //     int red = 0;
    //     int blue = 0;
    //     for (int i = 0; i < redOrBlue.Count; i++)
    //     {
    //         if (GameObject.Find("ChoosePlayer").transform.Find("P" + (i + 1)).Find("RedTeam").gameObject.activeSelf)
    //         {
    //             red += 1;
    //             redOrBlue[i] = true;
    //         }
    //         else
    //         {
    //             blue += 1;
    //             redOrBlue[i] = false;
    //         }
    //     }
    //     // if (red > 0 && blue > 0)
    //     // {
    //     // GameObject.Find("ChoosePlayer").GetComponent<CanvasGroup>().alpha = 0;
    //     // GameObject.Find("ChoosePlayer").GetComponent<CanvasGroup>().blocksRaycasts = false;
    //     // GameObject.Find("GameChoose").GetComponent<CanvasGroup>().alpha = 1;
    //     // GameObject.Find("GameChoose").GetComponent<CanvasGroup>().blocksRaycasts = true;
    //     // }
    //     // else
    //     // {
    //     //     Debug.Log("配置不對歐");
    //     // }
    // }
    //選擇哪個遊戲(點擊事件)
    public void gameNum(int num)
    {
        game_num = num;
        SceneManager.sceneLoaded += waitLoad;
        SceneManager.LoadSceneAsync("FightScene", LoadSceneMode.Single);
    }
    //重新開始遊戲(點擊事件)
    // public void gameNum()
    // {
    //     SceneManager.sceneLoaded += waitLoad;
    //     SceneManager.LoadSceneAsync("FightScene", LoadSceneMode.Single);
    // }
    //場景載入完(跳場景)
    public void waitLoad(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= waitLoad;
    }
    void playerSet()
    {
        Player[] players = PhotonNetwork.PlayerList;  //取得已加入房間的玩家陣列
        for (int i = 0; i < players.Length; i++)
        {
            GameObject AP = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "arena/arenaPlayer"), Vector3.zero, Quaternion.identity);
            // plist.Add(AP);
            AP.GetComponent<arenaPlayer>().p_index = i;
            AP.GetComponent<arenaPlayer>().red = redOrBlue[i];
            AP.GetComponent<arenaPlayer>().setUI();
            AP.GetComponent<PhotonView>().TransferOwnership(players[i]);
            switch (i) //初始位置
            {
                case 0:
                    AP.GetComponent<arenaPlayer>().SpawnPoint(new Vector3(-10, 0, 0));
                    break;
                case 1:
                    AP.GetComponent<arenaPlayer>().SpawnPoint(new Vector3(0, 10, 0));
                    break;
                case 2:
                    AP.GetComponent<arenaPlayer>().SpawnPoint(new Vector3(10, 0, 0));
                    break;
                case 3:
                    AP.GetComponent<arenaPlayer>().SpawnPoint(new Vector3(0, -10, 0));
                    break;
            }
        }
    }
    // plist.Clear(); 
    // for (int i = 0; i < redOrBlue.Count; i++)//生成玩家
    // {
    //     GameObject a = Instantiate(_player);
    //     plist.Add(a);
    //     a.GetComponent<arenaPlayer>().p_index = plist.Count - 1;
    //     a.GetComponent<arenaPlayer>().red = redOrBlue[i];
    //     a.GetComponent<arenaPlayer>().setUI();
    //     switch (i) //初始位置
    //     {
    //         case 0:
    //             plist[i].GetComponent<arenaPlayer>().SpawnPoint(new Vector3(-10, 0, 0));
    //             break;
    //         case 1:
    //             plist[i].GetComponent<arenaPlayer>().SpawnPoint(new Vector3(0, 10, 0));
    //             break;
    //         case 2:
    //             plist[i].GetComponent<arenaPlayer>().SpawnPoint(new Vector3(10, 0, 0));
    //             break;
    //         case 3:
    //             plist[i].GetComponent<arenaPlayer>().SpawnPoint(new Vector3(0, -10, 0));
    //             break;
    //     }
    // }
    // GameObject.Find("EventManager").transform.GetChild(game_num).gameObject.SetActive(true);//開啟該場景

    //inputSystm 的加入玩家函式
    // private void OnPlayerJoined(PlayerInput player)
    // {
    //     plist.Add(player.gameObject);
    //     player.gameObject.GetComponent<arenaPlayer>().openP_Num((plist.Count - 1));
    //     Debug.Log("Joined " + player.playerIndex + " - " + player.devices[0].displayName);
    //     Debug.Log("Player Count " + manager.playerCount + "/" + manager.maxPlayerCount);
    //     GameObject.Find("ChoosePlayer").transform.Find("P" + manager.playerCount).gameObject.SetActive(true);
    //     Debug.Log(player.currentControlScheme);
    //     if (player.currentControlScheme == "Keyboard&Mouse")
    //     {
    //         GameObject.Find("ChoosePlayer").transform.Find("P" + manager.playerCount).Find("keyboard").gameObject.SetActive(true);
    //         GameObject.Find("ChoosePlayer").transform.Find("P" + manager.playerCount).Find("gamepad").gameObject.SetActive(false);
    //     }
    //     else
    //     {
    //         GameObject.Find("ChoosePlayer").transform.Find("P" + manager.playerCount).Find("keyboard").gameObject.SetActive(false);
    //         GameObject.Find("ChoosePlayer").transform.Find("P" + manager.playerCount).Find("gamepad").gameObject.SetActive(true);
    //     }
    // }
}
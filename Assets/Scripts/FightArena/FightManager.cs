using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using System.IO;
public class FightManager : MonoBehaviour
{
    public static FightManager Instance;
    [SerializeField] private int game_num;
    public List<GameObject> plist;
    public List<bool> redOrBlue; //紀錄是紅隊還是藍隊
    [SerializeField] private GameObject _player;
    PhotonView PV;
    //靜態實例基本宣告
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        //DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        PV = GetComponent<PhotonView>();
        redOrBlue = new List<bool>();
        plist = new List<GameObject>();
        RoomManager RM = GameObject.Find("RoomManager").GetComponent<RoomManager>();
        for (int i = 0; i < RM.PlayerTeam.Length; i++)
        {
            if (RM.PlayerTeam[i].Equals("red"))
            {
                redOrBlue.Add(true);
            }
            else
            {
                redOrBlue.Add(false);
            }
        }
        if (PV.IsMine)
        {
            playerSet();
        }
        StartCoroutine(Wait_plist());
    }
    private void Update()
    {
        // if (Input.GetKeyDown(KeyCode.O) && redOrBlue.Count < 3)
        // {
        //     redOrBlue.Add(false);
        //     GameObject.Find("ChoosePlayer").transform.Find("P" + redOrBlue.Count).gameObject.SetActive(true);
        // }

    }

    IEnumerator Wait_plist()
    {
        if (redOrBlue.Count == plist.Count)
        {
            RoomManager RM = GameObject.Find("RoomManager").GetComponent<RoomManager>();
            game_num = RM.Game_num;
            GameObject.Find("EventManager").transform.GetChild(game_num).gameObject.SetActive(true);//開啟該場景
        }
        else
        {
            yield return new WaitForSeconds(0.1f);
            StartCoroutine(Wait_plist());
        }
    }

    //開始遊戲(點擊事件)
    public void joinGame()
    {
        redOrBlue.Add(false);
        GameObject.Find("ChoosePlayer").transform.Find("P" + redOrBlue.Count).gameObject.SetActive(true);
    }
    public void startGame()
    {
        int red = 0;
        int blue = 0;
        for (int i = 0; i < redOrBlue.Count; i++)
        {
            if (GameObject.Find("ChoosePlayer").transform.Find("P" + (i + 1)).Find("RedTeam").gameObject.activeSelf)
            {
                red += 1;
                redOrBlue[i] = true;
            }
            else
            {
                blue += 1;
                redOrBlue[i] = false;
            }
        }
        // if (red > 0 && blue > 0)
        // {
        GameObject.Find("ChoosePlayer").GetComponent<CanvasGroup>().alpha = 0;
        GameObject.Find("ChoosePlayer").GetComponent<CanvasGroup>().blocksRaycasts = false;
        GameObject.Find("GameChoose").GetComponent<CanvasGroup>().alpha = 1;
        GameObject.Find("GameChoose").GetComponent<CanvasGroup>().blocksRaycasts = true;
        // }
        // else
        // {
        //     Debug.Log("配置不對歐");
        // }
    }
    //選擇哪個遊戲(點擊事件)
    public void gameNum(int num)
    {
        game_num = num;
        SceneManager.sceneLoaded += waitLoad;
        SceneManager.LoadSceneAsync("FightScene", LoadSceneMode.Single);
    }
    //重新開始遊戲(點擊事件)
    public void gameNum()
    {
        SceneManager.sceneLoaded += waitLoad;
        SceneManager.LoadSceneAsync("FightScene", LoadSceneMode.Single);
    }
    //場景載入完(跳場景)
    public void waitLoad(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= waitLoad;
        // playerSet();
    }
    public void playerSet()
    {
        plist.Clear();
        for (int i = 0; i < redOrBlue.Count; i++)//生成玩家
        {
            GameObject a = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "arena/arenaPlayer"), Vector3.zero, Quaternion.identity);
            a.GetComponent<PhotonView>().TransferOwnership(PhotonNetwork.PlayerList[i]);
            // plist.Add(a);
            // a.GetComponent<arenaPlayer>().p_index = plist.Count - 1;
            // a.GetComponent<arenaPlayer>().red = redOrBlue[i];
            // a.GetComponent<arenaPlayer>().setUI();
            switch (i) //初始位置
            {
                case 0:
                    a.GetComponent<arenaPlayer>().SpawnPoint(new Vector3(-10, 0, 0));
                    break;
                case 1:
                    a.GetComponent<arenaPlayer>().SpawnPoint(new Vector3(0, 10, 0));
                    break;
                case 2:
                    a.GetComponent<arenaPlayer>().SpawnPoint(new Vector3(10, 0, 0));
                    break;
                case 3:
                    a.GetComponent<arenaPlayer>().SpawnPoint(new Vector3(0, -10, 0));
                    break;
            }
        }
    }
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

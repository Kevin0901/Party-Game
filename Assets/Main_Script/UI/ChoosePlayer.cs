using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChoosePlayer : MonoBehaviour
{
    // Start is called before the first frame update
    public bool inChoosePlayer;
    private Animator ChoosePlayerAnimator;
    public CanvasGroup CanvasGroup;
    public int playerCount; //計算玩家數量
    [Header("玩家清單")]
    [SerializeField] private List<string> UIplayerlist = new List<string>();
    void Start()
    {
        inChoosePlayer = false;
        ChoosePlayerAnimator = this.GetComponent<Animator>();
        CanvasGroup = this.GetComponent<CanvasGroup>();
        UIplayerlist.Add("first"); //初始化
        playerCount = 0;
    }
    // Update is called once per frame
    void Update()
    {
        if (inChoosePlayer)
        {
            StartCoroutine(fadein());
            inChoosePlayer = false;
        }

        if (playerCount < 4 && CanvasGroup.blocksRaycasts) //按鍵加入
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Joycheck("0");
            }
            else if (Input.GetKeyDown(KeyCode.B) || Input.GetKeyDown(KeyCode.Joystick1Button2))
            {
                Joycheck("1");
            }
            else if (Input.GetKeyDown(KeyCode.N) || Input.GetKeyDown(KeyCode.Joystick2Button2))
            {
                Joycheck("2");
            }
            else if (Input.GetKeyDown(KeyCode.M) || Input.GetKeyDown(KeyCode.Joystick3Button2))
            {
                Joycheck("3");
            }
            else if (Input.GetKeyDown(KeyCode.Joystick4Button2))
            {
                Joycheck("4");
            }
        }
    }

    void Joycheck(string num)
    {
        bool newP = false;
        for (int i = 0; i < UIplayerlist.Count; i++)
        {
            if (num.Equals(UIplayerlist[i])) //如果控制器存在
            {
                newP = true;
            }
        }
        if (newP)//刪除
        {
            playerCount--;
            int playpos = UIplayerlist.FindIndex(x => x.Equals(num)); //找加入遊戲的排序
            string playerhide = "P" + playpos;
            Transform p = transform.Find(playerhide); //移除物件
            UIplayerManager UIphide = p.gameObject.GetComponent<UIplayerManager>();
            //全部初始化
            UIphide.playersort = 0;
            UIphide.Joysticknum = null;
            UIphide.red = false;
            UIphide.blue = false;
            UIphide.iconjudge = false;
            UIphide.isChooseTeam = false;
            //關閉所有物件
            for (int i = 0; i < p.transform.childCount; i++)
            {
                p.transform.GetChild(i).gameObject.SetActive(false);
            }
            //移除索引
            UIplayerlist.Remove(num);

            for (int i = 2; i <= 4; i++) //往前替補
            {
                if (playpos == UIplayerlist.Count) //如果移除的目標是最後一個
                {
                    break;
                }
                Transform pRE = transform.Find("P" + (i - 1)); //P1
                Transform pNoRE = transform.Find("P" + i);     //P2  
                UIplayerManager UIpRE = pRE.GetComponent<UIplayerManager>();     //P1
                UIplayerManager UIpNoRE = pNoRE.GetComponent<UIplayerManager>(); //P2
                //將P1換成P2的
                UIpRE.playersort = UIpNoRE.playersort - 1;
                UIpRE.Joysticknum = UIpNoRE.Joysticknum;
                UIpRE.red = UIpNoRE.red;
                UIpRE.blue = UIpNoRE.blue;
                UIpRE.iconjudge = UIpNoRE.iconjudge;
                UIpRE.isChooseTeam = UIpNoRE.isChooseTeam;
                UIpRE.TeamChoose();
                //P2初始化
                UIpNoRE.playersort = 0;
                UIpNoRE.Joysticknum = null;
                UIpNoRE.red = false;
                UIpNoRE.blue = false;
                UIpNoRE.iconjudge = false;
                UIpNoRE.isChooseTeam = false;
                for (int j = 0; j < pNoRE.transform.childCount; j++)
                {
                    pNoRE.transform.GetChild(j).gameObject.SetActive(false);
                }
            }
        }
        else
        {
            //加入清單
            playerCount++;
            UIplayerlist.Add(num);
            string playershow = "P" + playerCount;
            Transform p = transform.Find(playershow);
            p.transform.Find("RedPlayer").gameObject.SetActive(true);
            UIplayerManager UIp = p.gameObject.GetComponent<UIplayerManager>();
            UIp.playersort = playerCount;
            UIp.Joysticknum = num;
            UIp.red = true;
            UIp.iconjudge = true;
            UIp.isChooseTeam = true;
        }
    }

    private IEnumerator fadein()
    {
        ChoosePlayerAnimator.SetBool("fadein", true);
        yield return null;
        ChoosePlayerAnimator.SetBool("fadein", false);
        GameObject.Find("LoadingCircle").transform.Find("Image").gameObject.SetActive(false);
        yield return new WaitForSeconds(1f);
        GameObject.Find("TranPageAnimation").transform.Find("Image").gameObject.SetActive(false);
        CanvasGroup.blocksRaycasts = true;
    }

    private IEnumerator fadeout()
    {
        CanvasGroup.blocksRaycasts = false;
        ChoosePlayerAnimator.SetBool("fadeout", true);
        GameObject.Find("LoadingCircle").transform.Find("Image").gameObject.SetActive(true);
        GameObject.Find("TranPageAnimation").transform.Find("Image").gameObject.SetActive(true);
        yield return null;
        ChoosePlayerAnimator.SetBool("fadeout", false);

        ClearAllPlayer();//清除所有清單
        GameObject.Find("GameMenu").GetComponent<GameMenu>().inGameMenu = true;
    }

    private void ClearAllPlayer() //清除玩家
    {
        for (int i = 1; i <= playerCount; i++)
        {
            string playershow = "P" + i;
            Transform p = transform.Find(playershow);
            for (int j = 0; j < p.transform.childCount; j++)
            {
                p.transform.GetChild(j).gameObject.SetActive(false);
            }

            UIplayerManager UIp = p.gameObject.GetComponent<UIplayerManager>();
            UIp.playersort = 0;
            UIp.Joysticknum = "";
            UIp.red = false;
            UIp.blue = false;
            UIp.iconjudge = false;
            UIp.isChooseTeam = false;
        }

        playerCount = 0;
        UIplayerlist.Clear();
        UIplayerlist.Add("first");
    }
    public void back() //點擊事件
    {
        StartCoroutine(fadeout());
    }
    public void Setting()//點擊事件
    {
        GameObject.Find("SettingMenu").transform.Find("Settings").gameObject.SetActive(true);
    }
    public void StartGame() //點擊事件  
    {
        if (playerCount > 1)
        {
            int redCnt = 0;
            int blueCnt = 0;
            for (int i = 1; i <= playerCount; i++)
            {
                string playershow = "P" + i;
                Transform p = transform.Find(playershow);
                if (p.transform.Find("red").gameObject.activeSelf) //如果啟用紅隊
                {
                    redCnt += 1;
                }
                else if (p.transform.Find("blue").gameObject.activeSelf)//如果啟用藍隊
                {
                    blueCnt += 1;
                }
            }
            if (redCnt > 0 && blueCnt > 0)  //如果都大於0的話
            {
                StartCoroutine(fadeoutToMainGame());
            }
            else
            {
                StartCoroutine(Warning());
            }
        }
        else
        {
            StartCoroutine(Warning());
        }
    }

    private IEnumerator fadeoutToMainGame() //去主遊戲
    {
        CanvasGroup.blocksRaycasts = false;
        ChoosePlayerAnimator.SetBool("fadeout", true);
        yield return null;
        ChoosePlayerAnimator.SetBool("fadeout", false);
        GameObject.Find("LoadingCircle").transform.Find("Image").gameObject.SetActive(true);
        GameObject.Find("TranPageAnimation").transform.Find("Image").gameObject.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        GameObject.Find("TranPageAnimation").transform.Find("Image").gameObject.SetActive(false);
        GameObject.Find("LoadingCircle").transform.Find("Image").gameObject.SetActive(false);

        PlayerManager PM = transform.Find("PlayerManager").GetComponent<PlayerManager>(); //找到GameManager
        PM.initializations(playerCount); //建構子初始化陣列

        for (int i = 1; i <= playerCount; i++) //將值輸入進陣列裡
        {
            string playershow = "P" + i;
            Transform p = transform.Find(playershow);
            UIplayerManager UIp = p.gameObject.GetComponent<UIplayerManager>();
            if (UIp.red)
            {
                PM.plist[i - 1].team = "red";
            }
            else if (UIp.blue)
            {
                PM.plist[i - 1].team = "blue";
            }
            PM.plist[i - 1].joynum = UIp.Joysticknum;
            PM.plist[i - 1].sort = UIp.playersort;

        }
        transform.Find("PlayerManager").SetParent(null);
        DontDestroyOnLoad(GameObject.Find("PlayerManager")); //別摧毀物件
        SceneManager.LoadSceneAsync("MainScene");           //載入場景
        StartCoroutine(GameObject.Find("PlayerManager").GetComponent<PlayerManager>().waitSceneLoad("MainScene"));//生成玩家
    }
    private IEnumerator Warning() //配置不符警告
    {
        transform.Find("PlayerWaring").gameObject.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        transform.Find("PlayerWaring").gameObject.SetActive(false);
    }
}
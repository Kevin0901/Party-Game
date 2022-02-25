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
    [Header("玩家清單")]
    [SerializeField] private List<string> playerlist = new List<string>();
    void Start()
    {
        inChoosePlayer = false;
        ChoosePlayerAnimator = this.GetComponent<Animator>();
        CanvasGroup = this.GetComponent<CanvasGroup>();
        playerlist.Add("first"); //初始化
    }
    // Update is called once per frame
    void Update()
    {
        if (inChoosePlayer)
        {
            StartCoroutine(fadein());
            inChoosePlayer = false;
        }

        if (CanvasGroup.blocksRaycasts) //按鍵加入
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Joycheck("0");
            }
            else if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.Joystick1Button2))
            {
                Joycheck("1");
            }
            else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Joystick2Button2))
            {
                Joycheck("2");
            }
            else if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Joystick3Button2))
            {
                Joycheck("3");
            }
            else if (Input.GetKeyDown(KeyCode.R) || Input.GetKeyDown(KeyCode.Joystick4Button2))
            {
                Joycheck("4");
            }
        }
    }

    void Joycheck(string num)
    {
        bool newP = false;
        for (int i = 1; i < playerlist.Count; i++)
        {
            if (num.Equals(playerlist[i])) //如果控制器存在
            {
                newP = true;
            }
        }
        if (newP)//若有該數字刪除該使用者
        {
            int playpos = playerlist.FindIndex(x => x.Equals(num)); //找該玩家在陣列裡的位置
            Transform playerhide = transform.Find("P" + playpos); //尋找該玩家物件
            UIplayerManager UIhide = playerhide.GetComponent<UIplayerManager>();
            //全部初始化
            UIhide.ResetValue();
            //關閉所有物件
            for (int i = 0; i < playerhide.childCount; i++)
            {
                playerhide.GetChild(i).gameObject.SetActive(false);
            }
            //移除索引
            playerlist.Remove(num);

            if (playpos != playerlist.Count) //如果移除的目標不是最後一個
            {
                for (int i = playpos + 1; i <= playerlist.Count; i++) //往前替補
                {
                    Transform pFirst = transform.Find("P" + (i - 1));
                    Transform pSecond = transform.Find("P" + i);
                    UIplayerManager UIFirst = pFirst.GetComponent<UIplayerManager>();
                    UIplayerManager UISecond = pSecond.GetComponent<UIplayerManager>();

                    //將Pi換成Pi+1的
                    UIFirst.playersort = UISecond.playersort - 1;
                    UIFirst.Joysticknum = UISecond.Joysticknum;
                    UIFirst.red = UISecond.red;
                    UIFirst.blue = UISecond.blue;
                    UIFirst.iconjudge = UISecond.iconjudge;
                    // UIFirst.isChooseTeam = UISecond.isChooseTeam;
                    UIFirst.TeamChoose();
                    //Pi+1初始化
                    UISecond.ResetValue();
                    for (int j = 0; j < pSecond.childCount; j++)
                    {
                        pSecond.GetChild(j).gameObject.SetActive(false);
                    }
                }
            }
        }
        else if (playerlist.Count - 1 < 4)
        {
            //加入清單
            playerlist.Add(num);
            Transform addP = transform.Find("P" + (playerlist.Count - 1));
            UIplayerManager UIShow = addP.gameObject.GetComponent<UIplayerManager>();
            UIShow.playersort = playerlist.Count;
            UIShow.Joysticknum = num;
            UIShow.red = true;
            UIShow.iconjudge = true;
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
        ClearAllPlayer();//清除所有玩家
        GameObject.Find("GameMenu").GetComponent<GameMenu>().inGameMenu = true;
    }

    private void ClearAllPlayer() //清除玩家
    {
        for (int i = 1; i < playerlist.Count; i++)
        {
            Transform p = transform.Find("P" + i);
            for (int j = 0; j < p.transform.childCount; j++)
            {
                p.GetChild(j).gameObject.SetActive(false);
            }

            UIplayerManager UIp = p.gameObject.GetComponent<UIplayerManager>();
            UIp.ResetValue();
        }
        playerlist.Clear();
        playerlist.Add("first");
    }
    public void back() //點擊事件
    {
        StartCoroutine(fadeout());
    }
    public void Setting()//點擊事件
    {
        GameObject.Find("SettingMenu").transform.Find("Settings").gameObject.SetActive(true);
    }
    private IEnumerator Warning() //配置不符警告
    {
        transform.Find("PlayerWaring").gameObject.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        transform.Find("PlayerWaring").gameObject.SetActive(false);
    }
    public void StartGame() //點擊事件  
    {
        if (playerlist.Count - 1 > 1)
        {
            int redCnt = 0;
            int blueCnt = 0;
            for (int i = 1; i < playerlist.Count; i++)
            {
                Transform p = transform.Find("P" + i);
                if (p.GetComponent<UIplayerManager>().red == true) //如果啟用紅隊
                {
                    redCnt += 1;
                }
                else//如果啟用藍隊
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
        yield return new WaitForSeconds(1.75f);
        GameObject.Find("TranPageAnimation").transform.Find("Image").gameObject.SetActive(false);
        GameObject.Find("LoadingCircle").transform.Find("Image").gameObject.SetActive(false);

        PlayerManager.Instance.initializations(playerlist.Count - 1); //建構子初始化陣列

        for (int i = 1; i < playerlist.Count; i++) //將值輸入進陣列裡
        {
            Transform p = transform.Find("P" + i);
            UIplayerManager UIp = p.GetComponent<UIplayerManager>();
            if (UIp.red)
            {
                PlayerManager.Instance.plist[i - 1].team = "red";
            }
            else
            {
                PlayerManager.Instance.plist[i - 1].team = "blue";
            }
            PlayerManager.Instance.plist[i - 1].joynum = UIp.Joysticknum;
            PlayerManager.Instance.plist[i - 1].sort = UIp.playersort;

        }
        SceneManager.LoadSceneAsync("MainScene");           //載入場景
        StartCoroutine(PlayerManager.Instance.waitSceneLoad("MainScene"));//生成玩家
    }
}
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
    public int playerCount;
    [Header("玩家清單")]
    [SerializeField] private List<string> UIplayerlist = new List<string>();
    void Start()
    {
        inChoosePlayer = false;
        ChoosePlayerAnimator = this.GetComponent<Animator>();
        CanvasGroup = this.GetComponent<CanvasGroup>();
        UIplayerlist.Add("first");
        playerCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (inChoosePlayer)
        {
            ChoosePlayerAnimator.SetBool("fadein", true);
            StartCoroutine(fadein());
            inChoosePlayer = false;
        }

        if (playerCount < 4 && CanvasGroup.blocksRaycasts)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Joycheck("0");
            }
            else if (Input.GetKeyDown(KeyCode.Joystick1Button2))
            {
                Joycheck("1");
            }
            else if (Input.GetKeyDown(KeyCode.Joystick2Button2))
            {
                Joycheck("2");
            }
            else if (Input.GetKeyDown(KeyCode.Joystick3Button2))
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
            if (num.Equals(UIplayerlist[i]))
            {
                newP = true;
            }
        }
        if (newP)
        {
            playerCount--;
            int playpos = UIplayerlist.FindIndex(x => x.Equals(num));
            string playerhide = "P" + playpos;
            Transform p = transform.Find(playerhide); //刪除的物件
            UIplayerManager UIphide = p.gameObject.GetComponent<UIplayerManager>();
            UIphide.playersort = 0;
            UIphide.Joysticknum = null;
            UIphide.red = false;
            UIphide.blue = false;
            UIphide.iconjudge = false;
            UIphide.isChooseTeam = false;

            for (int i = 0; i < p.transform.childCount; i++)
            {
                p.transform.GetChild(i).gameObject.SetActive(false);
            }
            UIplayerlist.Remove(num);

            for (int i = 2; i <= 4; i++) //往前替補
            {
                if (playpos == UIplayerlist.Count)
                {
                    break;
                }
                Transform pNoRE = transform.Find("P" + i);  //P2  
                Transform pRE = transform.Find("P" + (i - 1));//P1
                if (pNoRE.transform.Find("RedPlayer").gameObject.activeSelf || pNoRE.transform.Find("BluePlayer").gameObject.activeSelf)
                {
                    UIplayerManager UIpNoRE = pNoRE.gameObject.GetComponent<UIplayerManager>();//P2
                    UIplayerManager UIpRE = pRE.gameObject.GetComponent<UIplayerManager>(); //P1

                    UIpRE.playersort = UIpNoRE.playersort - 1;
                    UIpRE.Joysticknum = UIpNoRE.Joysticknum;
                    UIpRE.red = UIpNoRE.red;
                    UIpRE.blue = UIpNoRE.blue;
                    UIpRE.iconjudge = UIpNoRE.iconjudge;
                    UIpRE.isChooseTeam = UIpNoRE.isChooseTeam;
                    UIpRE.TeamChoose();
                    if (UIpRE.red)
                    {
                        pRE.transform.Find("RedPlayer").gameObject.SetActive(true);
                    }
                    else
                    {
                        pRE.transform.Find("BluePlayer").gameObject.SetActive(true);
                    }
                    UIpNoRE.playersort = 0;
                    UIpNoRE.Joysticknum = null;
                    UIpNoRE.red = false;
                    UIpNoRE.blue = false;
                    UIpNoRE.iconjudge = false;
                    UIpNoRE.isChooseTeam = false;
                    UIpNoRE.TeamChoose();
                    for (int j = 0; j < pNoRE.transform.childCount; j++)
                    {
                        pNoRE.transform.GetChild(j).gameObject.SetActive(false);
                    }
                }
            }
        }
        else
        {
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
        yield return new WaitForSeconds(0.5f);
        ChoosePlayerAnimator.SetBool("fadein", false);
        GameObject.Find("LoadingCircle").transform.Find("Image").gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        GameObject.Find("TranPageAnimation").transform.Find("Image").gameObject.SetActive(false);
        CanvasGroup.blocksRaycasts = true;
    }

    private IEnumerator fadeout()
    {
        CanvasGroup.blocksRaycasts = false;
        ChoosePlayerAnimator.SetBool("fadeout", true);
        yield return new WaitForSeconds(0.2f);
        GameObject.Find("LoadingCircle").transform.Find("Image").gameObject.SetActive(true);
        GameObject.Find("TranPageAnimation").transform.Find("Image").gameObject.SetActive(true);

        yield return new WaitForSeconds(0.5f);
        ChoosePlayerAnimator.SetBool("fadeout", false);

        ClearAllPlayer();
        GameObject.Find("GameMenu").GetComponent<GameMenu>().inGameMenu = true;
    }

    private void ClearAllPlayer()
    {
        for (int i = 1; i <= playerCount; i++)
        {
            string playershow = "P" + i;
            Transform p = transform.Find(playershow);
            p.transform.Find("RedPlayer").gameObject.SetActive(false);
            p.transform.Find("red").gameObject.SetActive(false);
            p.transform.Find("blue").gameObject.SetActive(false);
            p.transform.Find("leftArrow").gameObject.SetActive(false);
            p.transform.Find("rightArrow").gameObject.SetActive(false);
            p.transform.Find("L1").gameObject.SetActive(false);
            p.transform.Find("R1").gameObject.SetActive(false);

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

    public void back()
    {
        StartCoroutine(fadeout());
    }
    public void Setting()
    {
        GameObject.Find("SettingMenu").transform.Find("Settings").gameObject.SetActive(true);
    }

    public void StartGame()
    {
        if (playerCount > 1)
        {
            int redonline = 0;
            int blueonline = 0;
            for (int i = 1; i <= playerCount; i++)
            {
                string playershow = "P" + i;
                Transform p = transform.Find(playershow);
                if (p.transform.Find("red").gameObject.activeSelf)
                {
                    redonline += 1;
                }
                else if (p.transform.Find("blue").gameObject.activeSelf)
                {
                    blueonline += 1;
                }
            }
            if (redonline > 0 && blueonline > 0)
            {
                StartCoroutine(fadeoutToMainGame());
            }
            else
            {
                StartCoroutine(Warning(1));
            }
        }
        else
        {
            StartCoroutine(Warning(1));
        }
    }

    private IEnumerator fadeoutToMainGame()
    {
        CanvasGroup.blocksRaycasts = false;
        ChoosePlayerAnimator.SetBool("fadeout", true);
        yield return new WaitForSeconds(0.2f);
        GameObject.Find("LoadingCircle").transform.Find("Image").gameObject.SetActive(true);
        GameObject.Find("TranPageAnimation").transform.Find("Image").gameObject.SetActive(true);

        yield return new WaitForSeconds(0.5f);
        ChoosePlayerAnimator.SetBool("fadeout", false);

        yield return new WaitForSeconds(1.25f);
        GameObject.Find("LoadingCircle").transform.Find("Image").gameObject.SetActive(false);
        PlayerManager PM = transform.Find("GameManager").GetComponent<PlayerManager>();
        PM.initializations(playerCount);

        for (int i = 1; i <= playerCount; i++)
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
        transform.Find("GameManager").gameObject.transform.SetParent(null);
        DontDestroyOnLoad(GameObject.Find("GameManager"));
        SceneManager.LoadSceneAsync("MainScene");
    }

    private IEnumerator Warning(int Warningnum)
    {
        string Warning = "";
        if (Warningnum == 1)
        {
            Warning = "PlayerWaring";
        }
        transform.Find(Warning).gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        transform.Find(Warning).gameObject.SetActive(false);
    }
}
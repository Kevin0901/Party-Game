using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FootEvent : MonoBehaviour
{
    [SerializeField] private GameObject ball, UI;
    [HideInInspector] public int redScore, blueScore;
    [SerializeField] private int GameTime;
    [SerializeField] private float maxSpeed;
    void OnEnable()
    {
        SetPos();
    }
    public void StartGame()
    {
        GameObject a = Instantiate(ball, Vector3.zero, ball.transform.rotation);
        a.GetComponent<football>().maxSpeed = maxSpeed;
        for (int i = 0; i < FightManager.Instance.plist.Count; i++)
        {
            FightManager.Instance.plist[i].transform.Find("fist").gameObject.SetActive(true);
            FightManager.Instance.plist[i].GetComponent<arenaPlayer>().currentState = ArenaState.punch;
        }
        StartCoroutine(timeCount());
    }
    void Update()
    {
        if (GameTime < 0 || (redScore == 3) || (blueScore == 3))
        {
            StartCoroutine(EndGame());
        }
    }
    //設定玩家位置
    private void SetPos()
    {
        int redcnt = 0, bluecnt = 0;
        for (int i = 0; i < FightManager.Instance.plist.Count; i++)
        {
            if (FightManager.Instance.plist[i].GetComponent<arenaPlayer>().red)
            {
                redcnt++;
                switch (redcnt)
                {
                    case 1:
                        FightManager.Instance.plist[i].GetComponent<arenaPlayer>().SpawnPoint(new Vector3(20, 0, 0));
                        break;
                    case 2:
                        FightManager.Instance.plist[i].GetComponent<arenaPlayer>().SpawnPoint(new Vector3(30, 0, 0));
                        break;
                    case 3:
                        FightManager.Instance.plist[i].GetComponent<arenaPlayer>().SpawnPoint(new Vector3(40, 0, 0));
                        break;
                }
            }
            else
            {
                bluecnt++;
                switch (bluecnt)
                {
                    case 1:
                        FightManager.Instance.plist[i].GetComponent<arenaPlayer>().SpawnPoint(new Vector3(-20, 0, 0));
                        break;
                    case 2:
                        FightManager.Instance.plist[i].GetComponent<arenaPlayer>().SpawnPoint(new Vector3(-30, 0, 0));
                        break;
                    case 3:
                        FightManager.Instance.plist[i].GetComponent<arenaPlayer>().SpawnPoint(new Vector3(-40, 0, 0));
                        break;
                }
            }
        }
    }
    //得到分數
    private IEnumerator pointGet()
    {
        StopCoroutine(timeCount());
        for (int i = 0; i < FightManager.Instance.plist.Count; i++)
        {
            FightManager.Instance.plist[i].GetComponent<arenaPlayer>().currentState = ArenaState.idle;
            FightManager.Instance.plist[i].transform.Find("fist").gameObject.SetActive(false);
        }
        this.transform.Find("GameUI").Find("score").gameObject.SetActive(true);
        yield return new WaitForSeconds(0.35f);

        this.transform.Find("GameUI").Find("score").GetComponent<Text>().text = blueScore.ToString() + " : " + redScore.ToString();
        yield return new WaitForSeconds(2.5f);
        this.transform.Find("GameUI").Find("score").gameObject.SetActive(false);

        for (int i = 0; i < FightManager.Instance.plist.Count; i++)
        {
            FightManager.Instance.plist[i].GetComponent<arenaPlayer>().SpawnPoint();
        }

        GameObject a = Instantiate(ball, Vector3.zero, ball.transform.rotation);
        a.GetComponent<football>().maxSpeed = maxSpeed;
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(timeCount());

        for (int i = 0; i < FightManager.Instance.plist.Count; i++)
        {
            FightManager.Instance.plist[i].GetComponent<arenaPlayer>().currentState = ArenaState.punch;
            FightManager.Instance.plist[i].transform.Find("fist").gameObject.SetActive(true);
        }
    }
    //結束判定
    IEnumerator EndGame()
    {
        StopCoroutine(timeCount());
        for (int i = 0; i < FightManager.Instance.plist.Count; i++)
        {
            FightManager.Instance.plist[i].GetComponent<arenaPlayer>().currentState = ArenaState.idle;
            FightManager.Instance.plist[i].transform.Find("fist").gameObject.SetActive(false);
        }
        this.transform.Find("GameUI").Find("score").gameObject.SetActive(true);
        yield return new WaitForSeconds(0.35f);

        this.transform.Find("GameUI").Find("score").GetComponent<Text>().text = blueScore.ToString() + " : " + redScore.ToString();
        yield return new WaitForSeconds(1.5f);

        this.transform.Find("GameUI").Find("score").GetComponent<Text>().text = "遊戲結束";
        yield return new WaitForSeconds(2.5f);

        UI.SetActive(true);
        if (redScore > blueScore)
        {
            UI.transform.Find("red").gameObject.SetActive(true);
        }
        else if ((redScore < blueScore))
        {
            UI.transform.Find("blue").gameObject.SetActive(true);
        }
        else if ((redScore == blueScore))
        {
            UI.transform.Find("draw").gameObject.SetActive(true);
        }
        this.gameObject.SetActive(false);
    }
    //紅隊加分
    public void R_Score()
    {
        redScore++;
        StartCoroutine(pointGet());
    }
    //藍隊加分
    public void B_Score()
    {
        blueScore++;
        StartCoroutine(pointGet());
    }
    //計時器
    IEnumerator timeCount()
    {
        yield return new WaitForSeconds(1f);
        this.transform.Find("GameUI").Find("time").GetComponent<Text>().text = (--GameTime).ToString();
        StartCoroutine(timeCount());
    }
}

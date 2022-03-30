using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FootEvent : MonoBehaviour
{
    [SerializeField] private GameObject ball, UI, fist;
    [HideInInspector] public float redScore, blueScore;
    public int _time;
    void OnEnable()
    {
        SetPos();
    }
    public void StartGame()
    {
        Instantiate(ball, Vector3.zero, ball.transform.rotation);
        for (int i = 0; i < FightManager.Instance.plist.Count; i++)
        {
            FightManager.Instance.plist[i].GetComponent<arenaPlayer>().currentState = ArenaState.punch;
            FightManager.Instance.plist[i].transform.Find("fist").gameObject.SetActive(true);
        }
        StartCoroutine(timeCount());
    }
    void Update()
    {
        if (_time < 0 || (redScore == 3) || (blueScore == 3))
        {
            StartCoroutine(EndGame());
        }
    }
    //設定位置
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
                        FightManager.Instance.plist[i].GetComponent<arenaPlayer>().SpawnPoint(new Vector3(40, 0, 0));
                        break;
                    case 3:
                        FightManager.Instance.plist[i].GetComponent<arenaPlayer>().SpawnPoint(new Vector3(30, 0, 0));
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
                        FightManager.Instance.plist[i].GetComponent<arenaPlayer>().SpawnPoint(new Vector3(-40, 0, 0));
                        break;
                    case 3:
                        FightManager.Instance.plist[i].GetComponent<arenaPlayer>().SpawnPoint(new Vector3(-30, 0, 0));
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
        }
        this.transform.Find("GameUI").Find("score").gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);

        this.transform.Find("GameUI").Find("score").GetComponent<Text>().text = blueScore.ToString() + " : " + redScore.ToString();
        yield return new WaitForSeconds(2.5f);
        this.transform.Find("GameUI").Find("score").gameObject.SetActive(false);

        for (int i = 0; i < FightManager.Instance.plist.Count; i++)
        {
            FightManager.Instance.plist[i].GetComponent<arenaPlayer>().SpawnPoint();
        }

        GameObject a = Instantiate(ball, Vector3.zero, ball.transform.rotation);
        yield return new WaitForSeconds(1f);

        for (int i = 0; i < FightManager.Instance.plist.Count; i++)
        {
            FightManager.Instance.plist[i].GetComponent<arenaPlayer>().currentState = ArenaState.punch;
        }
    }
    //結束判定
    IEnumerator EndGame()
    {
        StopCoroutine(timeCount());
        for (int i = 0; i < FightManager.Instance.plist.Count; i++)
        {
            FightManager.Instance.plist[i].GetComponent<arenaPlayer>().currentState = ArenaState.idle;
        }
        this.transform.Find("GameUI").Find("score").gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);

        this.transform.Find("GameUI").Find("score").GetComponent<Text>().text = blueScore.ToString() + " : " + redScore.ToString();
        yield return new WaitForSeconds(1f);

        this.transform.Find("GameUI").Find("score").GetComponent<Text>().text = "遊戲結束";
        yield return new WaitForSeconds(3f);

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
        for (int i = 0; i < FightManager.Instance.plist.Count; i++)
        {
            FightManager.Instance.plist[i].SetActive(false);
        }
        this.gameObject.SetActive(false);
    }
    //紅隊加分
    public void red_Score()
    {
        redScore++;
        StartCoroutine(pointGet());
        this.transform.Find("GameUI").Find("red").GetComponent<Text>().text = "Score:" + redScore.ToString();
    }
    //藍隊加分
    public void blue_Score()
    {
        blueScore++;
        StartCoroutine(pointGet());
        this.transform.Find("GameUI").Find("blue").GetComponent<Text>().text = "Score:" + blueScore.ToString();
    }
    //計時器
    IEnumerator timeCount()
    {
        yield return new WaitForSeconds(1f);
        this.transform.Find("GameUI").Find("time").GetComponent<Text>().text = (--_time).ToString();
        StartCoroutine(timeCount());
    }
}

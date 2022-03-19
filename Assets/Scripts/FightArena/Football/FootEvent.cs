using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FootEvent : MonoBehaviour
{
    [SerializeField] private GameObject ball, UI;
    [HideInInspector] public float redScore, blueScore;
    [HideInInspector] public int _time;
    private void Start()
    {
        _time = 100;
        // SetPos();
        GameObject a = Instantiate(ball, Vector3.zero, ball.transform.rotation);
        a.GetComponent<football>().parent = this.gameObject;
    }
    public void StartGame()
    {
        // for (int i = 0; i < FightManager.Instance.gamelist.Count; i++)
        // {
        //     FightManager.Instance.gamelist[i].GetComponent<arenaPlayer>().currentState = ArenaState.punch;
        // FightManager.Instance.gamelist[i].transform.Find("fist").gameObject.SetActive(true);
        // }
        StartCoroutine(timeCount());
    }
    void Update()
    {
        if (_time < 0 || (redScore == 3) || (blueScore == 3))
        {
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
            for (int i = 0; i < FightManager.Instance.gamelist.Count; i++)
            {
                FightManager.Instance.gamelist[i].SetActive(false);
            }
            this.gameObject.SetActive(false);
        }
    }
    public void SetPos()
    {
        int redcnt = 0, bluecnt = 0;
        for (int i = 0; i < FightManager.Instance.gamelist.Count; i++)
        {
            if (FightManager.Instance.gamelist[i].GetComponent<arenaPlayer>().red)
            {
                redcnt++;
                switch (redcnt)
                {
                    case 1:
                        FightManager.Instance.gamelist[i].GetComponent<arenaPlayer>().SpawnPoint(new Vector3(20, 0, 0));
                        break;
                    case 2:
                        FightManager.Instance.gamelist[i].GetComponent<arenaPlayer>().SpawnPoint(new Vector3(40, 0, 0));
                        break;
                    case 3:
                        FightManager.Instance.gamelist[i].GetComponent<arenaPlayer>().SpawnPoint(new Vector3(30, 0, 0));
                        break;
                }
            }
            else
            {
                bluecnt++;
                switch (bluecnt)
                {
                    case 1:
                        FightManager.Instance.gamelist[i].GetComponent<arenaPlayer>().SpawnPoint(new Vector3(-20, 0, 0));
                        break;
                    case 2:
                        FightManager.Instance.gamelist[i].GetComponent<arenaPlayer>().SpawnPoint(new Vector3(-40, 0, 0));
                        break;
                    case 3:
                        FightManager.Instance.gamelist[i].GetComponent<arenaPlayer>().SpawnPoint(new Vector3(-30, 0, 0));
                        break;
                }
            }
        }
    }
    private IEnumerator pointGet()
    {
        StopCoroutine(timeCount());
        // for (int i = 0; i < FightManager.Instance.gamelist.Count; i++)
        // {
        //     FightManager.Instance.gamelist[i].GetComponent<arenaPlayer>().currentState = ArenaState.idle;
        // }
        this.transform.GetChild(0).Find("score").gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        this.transform.GetChild(0).Find("score").GetComponent<Text>().text = blueScore.ToString() + " : " + redScore.ToString();
        yield return new WaitForSeconds(2.5f);
        GameObject a = Instantiate(ball, Vector3.zero, ball.transform.rotation);
        a.GetComponent<football>().parent = this.gameObject;
        yield return new WaitForSeconds(0.5f);
        this.transform.GetChild(0).Find("score").gameObject.SetActive(false);
        for (int i = 0; i < FightManager.Instance.gamelist.Count; i++)
        {
            FightManager.Instance.gamelist[i].GetComponent<arenaPlayer>().SpawnPoint();
            FightManager.Instance.gamelist[i].GetComponent<arenaPlayer>().currentState = ArenaState.punch;
        }
    }
    public void red_Score()
    {
        redScore++;
        StartCoroutine(pointGet());
        // this.transform.GetChild(0).Find("red").GetComponent<Text>().text = "Score:" + redScore.ToString();
    }
    public void blue_Score()
    {
        blueScore++;
        StartCoroutine(pointGet());
        // this.transform.GetChild(0).Find("blue").GetComponent<Text>().text = "Score:" + blueScore.ToString();
    }
    IEnumerator timeCount()
    {
        yield return new WaitForSeconds(1f);
        this.transform.GetChild(0).Find("time").GetComponent<Text>().text = (--_time).ToString();
        StartCoroutine(timeCount());
    }
}

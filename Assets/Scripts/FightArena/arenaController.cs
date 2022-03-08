using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class arenaController : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isover, isget1st, isCupidEnd, isCupidGame;
    public GameObject nowgame;
    public List<GameObject> cupidwinplayer = new List<GameObject>();
    private int winplayer;
    [HideInInspector] public playerlist p;
    public Text countdownDisplay;
    void Start()
    {
        isget1st = false;
        isover = false;
        isCupidEnd = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isCupidGame)
        {
            if (isCupidEnd || p.player.Count == 0)
            {
                isover = true;
            }
        }
        else if (p.player.Count == 1)
        {
            isover = true;
        }

        if (isover && !isget1st)
        {
            this.transform.GetChild(0).gameObject.SetActive(true);
            if (isCupidGame)
            {
                CupidWinJudge();
            }
            else if (p.player.Count == 1)//剩1個玩家，通常都是第一名
            {
                // winplayer = p.player[0].GetComponent<arenaPlayer>().order; //顯示贏家
                GameObject.Find("FightGameManager").transform.Find("Gameover").transform.Find("Panel").transform.Find("P" + winplayer).gameObject.SetActive(true);
                StartCoroutine(Exit());
            }
            isget1st = true;
            for (int i = 0; i < p.player.Count; i++) //關閉所有玩家
            {
                p.player[i].SetActive(false);
            }
        }
    }

    public void CupidWinJudge()//剩餘存活玩家分數判定
    {
        for (int i = 0; i < p.bornplayer.Count - 1; i++)
        {
            if (i == 0)
            {
                cupidwinplayer[0] = p.bornplayer[i];
            }

            if (cupidwinplayer[0].GetComponent<arenaPlayer>().CupidGamepoint > p.bornplayer[i + 1].GetComponent<arenaPlayer>().CupidGamepoint)
            {

            }
            else if (cupidwinplayer[0].GetComponent<arenaPlayer>().CupidGamepoint == p.bornplayer[i + 1].GetComponent<arenaPlayer>().CupidGamepoint)
            {
                cupidwinplayer.Add(p.bornplayer[i + 1].gameObject);
            }
            else
            {
                cupidwinplayer[0] = p.bornplayer[i + 1];
            }
        }

        for (int j = 0; j < cupidwinplayer.Count - 1; j++)//cupidwinplayer 排錯檢查，確保裡面的玩家分數是否都一樣
        {
            if (cupidwinplayer[j].GetComponent<arenaPlayer>().CupidGamepoint > cupidwinplayer[j + 1].GetComponent<arenaPlayer>().CupidGamepoint)
            {
                cupidwinplayer.Remove(cupidwinplayer[j + 1]);
            }
            else if (cupidwinplayer[j].GetComponent<arenaPlayer>().CupidGamepoint < cupidwinplayer[j + 1].GetComponent<arenaPlayer>().CupidGamepoint)
            {
                cupidwinplayer.Remove(cupidwinplayer[j]);
            }
        }

        // if (cupidwinplayer.Count == 1)
        // {
        //     winplayer = cupidwinplayer[0].GetComponent<arenaPlayer>().order; //顯示贏家
        //     GameObject.Find("FightGameManager").transform.Find("Gameover").transform.Find("Panel").transform.Find("P" + winplayer).gameObject.SetActive(true);
        //     StartCoroutine(Exit());
        // }
        // else if (cupidwinplayer.Count > 1)
        // {
        //     for (int i = 0; i < cupidwinplayer.Count; i++)
        //     {
        //         winplayer = cupidwinplayer[i].GetComponent<arenaPlayer>().order; //顯示贏家
        //         GameObject.Find("FightGameManager").transform.Find("Gameover").transform.Find("Panel").transform.Find("P" + winplayer).gameObject.SetActive(true);
        //         StartCoroutine(Exit());
        //     }
        // }

    }
    public IEnumerator Exit() //回去主場景
    {
        int countdownTime = 5;
        while (countdownTime > 0)
        {
            countdownDisplay.text = countdownTime.ToString();
            yield return new WaitForSeconds(1f);
            countdownTime -= 1;
        }
        if (countdownTime == 0)
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName("MainScene"));
            GameObject.Find("PAPA").GetComponent<REsetGameObject>().isRESpawn = true;
            SceneManager.UnloadSceneAsync("FightScene");
        }
    }
    // public void Replay()
    // {
    //     isover = false;
    //     isget1st = false;
    //     ReBornplayer();
    //     // Instantiate(nowgame,new Vector3(0,0,0),Quaternion.identity);
    //     GameObject.Find("FightGameManager").transform.Find("Gameover").transform.Find("P" + winplayer).gameObject.SetActive(false);
    //     GameObject cd = GameObject.Find("countdown").transform.Find("countdownUI").gameObject;
    //     cd.SetActive(true);
    //     cd.GetComponent<countdown>().startcountdown(nowgame);
    //     winplayer = 0;
    //     this.transform.GetChild(0).gameObject.SetActive(false);
    // }

    // private void ReBornplayer()
    // {
    //     playerlist p = GameObject.Find("playerManager").GetComponent<playerlist>();
    //     for (int i = 0; i < p.bornplayer.Count; i++)
    //     {
    //         p.player.Add(p.bornplayer[i]);
    //         p.player[i].SetActive(true);
    //         p.player[i].GetComponent<arenaPlayer>().SpawnPoint();
    //         p.player[i].GetComponent<arenaPlayer>().currentState = ArenaState.idle;
    //         GameObject.Find("HealthManager").transform.Find("P" + p.player[i]
    //         .GetComponent<arenaPlayer>().order).GetComponent<heart>().fullHealth();
    //     }
    // }
}

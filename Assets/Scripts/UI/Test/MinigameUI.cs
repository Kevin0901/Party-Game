using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameUI : MonoBehaviour
{
    public GameObject minigame;
    // private arenaController GameController;
    // Start is called before the first frame update
    void Start()
    {
        // GameController = GameObject.Find("FightGameManager").GetComponent<arenaController>();
        // if (minigame.name == "Cupid")
        // {
        //     GameController.isCupidGame = true;
        // }
    }

    // Update is called once per frame
    void Update()
    {
        // int allplayer = GameObject.Find("playerManager").GetComponent<playerlist>().player.Count;
        // List<GameObject> p = GameObject.Find("playerManager").GetComponent<playerlist>().player;
        // for (int i = 0; i < allplayer; i++)
        // {
        //     transform.Find("P" + (i + 1)).gameObject.SetActive(true);
        //     if (p[i].gameObject.GetComponent<arenaPlayer>().isready)
        //     {
        //         transform.Find("P" + (i + 1)).transform.Find("notready").gameObject.SetActive(false);
        //         transform.Find("P" + (i + 1)).transform.Find("ready").gameObject.SetActive(true);
        //     }
        // }
    }

    public void StartFightGame()
    {
        // int cnt = 0;
        // int allplayer = GameObject.Find("playerManager").GetComponent<playerlist>().player.Count;
        // List<GameObject> p = GameObject.Find("playerManager").GetComponent<playerlist>().player;
        // for (int i = 0; i < allplayer; i++)
        // {
        //     if (p[i].gameObject.GetComponent<arenaPlayer>().isready)
        //     {
        //         cnt += 1;
        //     }
        // }
        // if (cnt == allplayer)
        // {
        //     GameObject.Find("MainBlackScreen").gameObject.SetActive(false);
        //     GameObject.Find("BlackScreen").transform.GetChild(0).gameObject.SetActive(false);
        //     GameObject cd = GameObject.Find("countdown").transform.Find("countdownUI").gameObject;
        //     cd.SetActive(true);

        //     cd.GetComponent<countdown>().startcountdown(minigame);
        //     GameObject.Find("playerManager").GetComponent<playerlist>().setbornplayer();

        //     this.transform.gameObject.SetActive(false);
        }
    }

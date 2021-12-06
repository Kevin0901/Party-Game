using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    [System.Serializable]
    public class playerSet
    {
        public string team;
        public string joynum;
        public int sort;
        public playerSet()
        {
            this.team = "red";
            this.joynum = "0";
            this.sort = 0;
        }
    }
    public playerSet[] plist;
    [Header("主場景玩家")]
    [SerializeField] private GameObject player;
    [Header("競技場玩家")]
    [SerializeField] private GameObject fightplayer;
    public GameObject RedCastel, BlueCastel, red, blue;
    private float h, w, proportion_W, proportion_H, factor;
    public string EventGame;
    void Update()
    {
        // Scene scene = SceneManager.GetActiveScene();
        // if (scene.name == "MainScene" && isSpawn)//第一進場景才會執行 playerSpawn()
        // {
        //     isSpawn = false;
        //     StartCoroutine(playerSpawn());
        // }
        // else if (scene.name == "FightScene" && !isSpawn)
        // {
        //     Cursor.visible = true;
        //     StartCoroutine(FightPlayerSpawn());
        // }
    }
    public void initializations(int max) //初始化陣列
    {
        plist = new playerSet[max];
        for (int i = 0; i < max; i++)
        {
            plist[i] = new playerSet();
        }
    }
    public IEnumerator waitSceneLoad(string level) //等場景載入完
    {
        if (SceneManager.GetActiveScene().name != level)
        {
            yield return null;
            StartCoroutine(waitSceneLoad(level));
        }
        else
        {
            StartCoroutine(playerSpawn());
        }
    }
    public IEnumerator playerSpawn()  //生成玩家
    {
        GameObject.Find("MainBlackScreen").GetComponent<Animator>().SetBool("fadeout", true);
        yield return null;
        GameObject.Find("MainBlackScreen").GetComponent<Animator>().SetBool("fadeout", false);
        ScreenSet();
        if (plist.Length > 2)//開啟UI黑條
        {
            GameObject.Find("blackSplit").transform.GetChild(0).gameObject.SetActive(true);
        }

        for (int i = 0; i < plist.Length; i++)
        {
            GameObject p = Instantiate(player, this.transform.position, player.transform.rotation);
            p.transform.Find("player").tag = plist[i].team;
            p.transform.Find("player").GetComponent<PlayerMovement>().joynum = plist[i].joynum;
            p.transform.Find("player").GetComponent<PlayerMovement>().order = plist[i].sort;
            p.transform.Find("player").GetComponent<PlayerMovement>().allplayercount = plist.Length;
            p.transform.Find("player").GetComponent<Team>().enabled = true;
            p.transform.Find("player").GetComponent<PlayerMovement>().bornSet();

            Camera cam = p.transform.Find("Camera").GetComponent<Camera>();
            if (plist.Length == 2)
            {
                // p.GetComponent<PlayerMovement>().deadscreen = GameObject.Find("DeadScreen").transform.Find("Total2").transform.Find("P" + plist[i].sort).gameObject;
                cam.aspect = proportion_W / 2 / proportion_H;//相機比例
                cam.orthographicSize = 40 / proportion_W * proportion_H;//相機大小  40/w/*h
                switch (plist[i].sort)
                {
                    case 1:
                        cam.rect = new Rect(0, 0, 0.5f, 1f);
                        break;
                    case 2:
                        cam.rect = new Rect(0.5f, 0, 0.5f, 1f);
                        break;
                }
            }
            else
            {
                // p.GetComponent<PlayerMovement>().deadscreen = GameObject.Find("DeadScreen").transform.Find("Total4").transform.Find("P" + plist[i].sort).gameObject;
                // p.GetComponent<Camera>().aspect = proportion_W / proportion_H;//相機比例
                cam.orthographicSize = 20 / proportion_W * proportion_H;//相機大小  20/w/*h
                switch (plist[i].sort)
                {
                    case 1:
                        cam.rect = new Rect(0f, 0.5f, 0.5f, 0.5f);
                        break;
                    case 2:
                        cam.rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
                        break;
                    case 3:
                        cam.rect = new Rect(0f, 0f, 0.5f, 0.5f);
                        break;
                    case 4:
                        cam.rect = new Rect(0.5f, 0f, 0.5f, 0.5f);
                        break;
                }
            }
        }
        // red = Instantiate(RedCastel, new Vector3(-20, -43.7f, 0), RedCastel.transform.rotation);
        // red.transform.localScale = new Vector3(4, 4, 4);
        // blue = Instantiate(BlueCastel, new Vector3(-20, 45, 0), BlueCastel.transform.rotation);
        // blue.transform.localScale = new Vector3(4, 4, 4);
    }

    private IEnumerator FightPlayerSpawn()
    {
        yield return new WaitForSeconds(0.1f);
        List<GameObject> pl = GameObject.Find("playerManager").GetComponent<playerlist>().player;
        for (int i = 0; i < plist.Length; i++)
        {
            GameObject p = Instantiate(fightplayer, this.transform.position, player.transform.rotation);
            p.GetComponent<arenaPlayer>().joynum = plist[i].joynum;
            p.GetComponent<arenaPlayer>().order = plist[i].sort;
            pl.Add(p);
            switch (plist[i].sort)
            {
                case 1:
                    p.GetComponent<arenaPlayer>().SpawnPoint(new Vector3(-35, 0, 0));
                    break;
                case 2:
                    p.GetComponent<arenaPlayer>().SpawnPoint(new Vector3(0, 35, 0));
                    break;
                case 3:
                    p.GetComponent<arenaPlayer>().SpawnPoint(new Vector3(35, 0, 0));
                    break;
                case 4:
                    p.GetComponent<arenaPlayer>().SpawnPoint(new Vector3(0, -35, 0));
                    break;
            }
            p.transform.Find("NumTitle").transform.Find("P" + plist[i].sort).gameObject.SetActive(true);
        }
        // List<string> game = new List<string> { "Medusa", "SunMoon", "Sword", "Cupid" };
        // int randomgame = Random.Range(0, game.Count);
        // GameObject.Find("UI").transform.Find("Medusa").gameObject.SetActive(true);
        GameObject.Find("UI").transform.Find(EventGame).gameObject.SetActive(true);

        this.gameObject.SetActive(false);
        // GameObject.Find(game[randomgame]+"UI").gameObject.SetActive(true);

    }
    void ScreenSet() //螢幕比例
    {
        h = Screen.height;
        w = Screen.width;
        factor = screenScale(w, h);
        proportion_W = w / factor;
        proportion_H = h / factor;
    }
    float screenScale(float a, float b)
    {
        while (a != 0 && b != 0)
        {
            if (a > b)
            {
                a %= b;
            }
            else if (b > a)
            {
                b %= a;
            }
        }
        if (a == 0)
        {
            return b;
        }
        else
        {
            return a;
        }
    }
}

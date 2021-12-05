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
    [Header("玩家生成prefabs")]
    [SerializeField] private GameObject player, fightplayer;
    public GameObject RedCastel, BlueCastel, red, blue;
    public bool isSpawn;
    private float h, w, proportion_W, proportion_H, factor;
    public string EventGame;
    void Start()
    {
        isSpawn = true;
    }
    // Update is called once per frame
    void Update()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name == "MainScene" && isSpawn)//第一進場景才會執行 playerSpawn()
        {
            isSpawn = false;
            StartCoroutine(playerSpawn());
        }
        else if (scene.name == "FightScene" && !isSpawn)
        {
            Cursor.visible = true;
            StartCoroutine(FightPlayerSpawn());
            // this.gameObject.SetActive(false);
        }
    }
    private IEnumerator playerSpawn()  //生成玩家
    {
        ScreenSet();
        GameObject.Find("MainBlackScreen").GetComponent<Animator>().SetBool("fadeout", true);
        yield return new WaitForSeconds(0.1f);
        GameObject.Find("MainBlackScreen").GetComponent<Animator>().SetBool("fadeout", false);
        for (int i = 0; i < plist.Length; i++)
        {
            GameObject p = Instantiate(player, this.transform.position, player.transform.rotation);
            p.GetComponent<PlayerMovement>().joynum = plist[i].joynum;
            p.GetComponent<PlayerMovement>().order = plist[i].sort;
            p.GetComponent<PlayerMovement>().allplayercount = plist.Length;
            p.tag = plist[i].team;
            p.AddComponent<Team>();
            p.GetComponent<PlayerMovement>().bornSet();
            if (plist.Length == 2)
            {
                p.GetComponent<PlayerMovement>().deadscreen = GameObject.Find("DeadScreen").transform.Find("Total2").transform.Find("P" + plist[i].sort).gameObject;
                p.transform.GetChild(0).GetComponent<Camera>().aspect = proportion_W / 2 / proportion_H;
                p.transform.GetChild(0).GetComponent<Camera>().orthographicSize = 40 / proportion_W * proportion_H;//40/w/*h
                switch (plist[i].sort)
                {
                    case 1:
                        p.transform.GetChild(0).GetComponent<Camera>().rect = new Rect(0, 0, 0.5f, 1f);
                        break;
                    case 2:
                        p.transform.GetChild(0).GetComponent<Camera>().rect = new Rect(0.5f, 0, 0.5f, 1f);
                        break;
                }
            }
            else
            {
                p.GetComponent<PlayerMovement>().deadscreen = GameObject.Find("DeadScreen").transform.Find("Total4").transform.Find("P" + plist[i].sort).gameObject;
                p.transform.GetChild(0).GetComponent<Camera>().aspect = proportion_W / proportion_H;
                p.transform.GetChild(0).GetComponent<Camera>().orthographicSize = 20 / proportion_W * proportion_H;//20/w/*h
                switch (plist[i].sort)
                {
                    case 1:
                        p.transform.GetChild(0).GetComponent<Camera>().rect = new Rect(0f, 0.5f, 0.5f, 0.5f);
                        break;
                    case 2:
                        p.transform.GetChild(0).GetComponent<Camera>().rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
                        break;
                    case 3:
                        p.transform.GetChild(0).GetComponent<Camera>().rect = new Rect(0f, 0f, 0.5f, 0.5f);
                        break;
                    case 4:
                        p.transform.GetChild(0).GetComponent<Camera>().rect = new Rect(0.5f, 0f, 0.5f, 0.5f);
                        break;
                }
            }
        }
        red = Instantiate(RedCastel, new Vector3(-20, -43.7f, 0), RedCastel.transform.rotation);
        red.transform.localScale = new Vector3(4, 4, 4);
        blue = Instantiate(BlueCastel, new Vector3(-20, 45, 0), BlueCastel.transform.rotation);
        blue.transform.localScale = new Vector3(4, 4, 4);
    }
    void ScreenSet()
    {
        h = Screen.currentResolution.height;
        w = Screen.currentResolution.width;
        factor = screenScale(w, h);
        proportion_W = w / factor;
        proportion_H = h / factor;
        if (plist.Length > 2)
        {
            GameObject.Find("blackSplit").transform.GetChild(0).gameObject.SetActive(true);
        }
    }
    public void initializations(int max)
    {
        plist = new playerSet[max];
        for (int i = 0; i < max; i++)
        {
            plist[i] = new playerSet();
        }
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.SceneManagement;
public class FightManager : MonoBehaviour
{
    public static FightManager Instance;
    public int game_num;
    public List<GameObject> plist, gamelist;
    private PlayerInputManager manager;
    //靜態實例基本宣告
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        plist = new List<GameObject>();
        gamelist = new List<GameObject>();
        manager = this.GetComponent<PlayerInputManager>();
    }
    //inputSystm 的加入玩家函式
    private void OnPlayerJoined(PlayerInput player)
    {
        plist.Add(player.gameObject);
        player.gameObject.GetComponent<arenaPlayer>().openP_Num((plist.Count - 1));
        Debug.Log("Joined " + player.playerIndex + " - " + player.devices[0].displayName);
        Debug.Log("Player Count " + manager.playerCount + "/" + manager.maxPlayerCount);
        GameObject.Find("ChoosePlayer").transform.Find("P" + manager.playerCount).gameObject.SetActive(true);
        Debug.Log(player.currentControlScheme);
        if (player.currentControlScheme == "Keyboard&Mouse")
        {
            GameObject.Find("ChoosePlayer").transform.Find("P" + manager.playerCount).Find("keyboard").gameObject.SetActive(true);
            GameObject.Find("ChoosePlayer").transform.Find("P" + manager.playerCount).Find("gamepad").gameObject.SetActive(false);
        }
        else
        {
            GameObject.Find("ChoosePlayer").transform.Find("P" + manager.playerCount).Find("keyboard").gameObject.SetActive(false);
            GameObject.Find("ChoosePlayer").transform.Find("P" + manager.playerCount).Find("gamepad").gameObject.SetActive(true);
        }
    }
    //重新配置
    public void retry()
    {
        if (plist.Count != 0)
        {
            for (int i = 0; i < plist.Count; i++)
            {
                GameObject.Find("ChoosePlayer").transform.Find("P" + (i + 1)).gameObject.SetActive(false);
                Destroy(plist[i]);
            }
            plist.Clear();
        }
    }
    //選隊
    public void teamChoose()
    {
        int red = 0;
        int blue = 0;
        for (int i = 0; i < plist.Count; i++)
        {
            if (GameObject.Find("ChoosePlayer").transform.Find("P" + (i + 1)).Find("RedTeam").gameObject.activeSelf)
            {
                red += 1;
                plist[i].GetComponent<arenaPlayer>().red = true;
            }
            else
            {
                blue += 1;
                plist[i].GetComponent<arenaPlayer>().red = false;
            }
        }
        if (plist.Count >= 2 && (red > 0 && blue > 0))
        {
            GameObject.Find("ChoosePlayer").GetComponent<CanvasGroup>().alpha = 0;
            GameObject.Find("ChoosePlayer").GetComponent<CanvasGroup>().blocksRaycasts = false;
            GameObject.Find("GameChoose").GetComponent<CanvasGroup>().alpha = 1;
            GameObject.Find("GameChoose").GetComponent<CanvasGroup>().blocksRaycasts = true;
            for (int j = 0; j < plist.Count; j++)
            {
                plist[j].GetComponent<arenaPlayer>().removeChoose();
                DontDestroyOnLoad(plist[j]);
            }
            manager.enabled = false;
        }
        else
        {
            Debug.Log("配置不對歐");
        }
    }
    //選擇哪個遊戲
    public void gameNum(int num)
    {
        SceneManager.sceneLoaded += waitLoad;
        gamelist.Clear();
        game_num = num;
        for (int i = 0; i < plist.Count; i++)
        {
            gamelist.Add(plist[i]);
        }
        SceneManager.LoadSceneAsync("FightScene", LoadSceneMode.Single);
    }
    //選擇哪個遊戲
    public void gameNum()
    {
        SceneManager.sceneLoaded += waitLoad;
        gamelist.Clear();
        for (int i = 0; i < plist.Count; i++)
        {
            gamelist.Add(plist[i]);
        }
        SceneManager.LoadSceneAsync("FightScene", LoadSceneMode.Single);
    }
    //場景載入完會發生的事情
    public void waitLoad(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= waitLoad;
        GameObject.Find("EventManager").transform.GetChild(game_num).gameObject.SetActive(true);
        for (int i = 0; i < plist.Count; i++)
        {
            switch (i)
            {
                case 0:
                    plist[i].GetComponent<arenaPlayer>().SpawnPoint(new Vector3(-10, 0, 0));
                    break;
                case 1:
                    plist[i].GetComponent<arenaPlayer>().SpawnPoint(new Vector3(0, 10, 0));
                    break;
                case 2:
                    plist[i].GetComponent<arenaPlayer>().SpawnPoint(new Vector3(10, 0, 0));
                    break;
                case 3:
                    plist[i].GetComponent<arenaPlayer>().SpawnPoint(new Vector3(0, -10, 0));
                    break;
            }
            plist[i].SetActive(true);
        }
    }

}

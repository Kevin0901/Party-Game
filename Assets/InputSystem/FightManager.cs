using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class FightManager : MonoBehaviour
{
    public static FightManager Instance;
    public int red, blue;
    public List<GameObject> plist;
    private PlayerInputManager manager;
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
        manager = this.GetComponent<PlayerInputManager>();
    }
    private void OnPlayerJoined(PlayerInput player)
    {
        plist.Add(player.gameObject);
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
    public void teamChoose()
    {
        red = 0;
        blue = 0;
        for (int i = 0; i < plist.Count; i++)
        {
            if (plist[i].GetComponent<inputS_Player>().red)
            {
                red += 1;
            }
            else
            {
                blue += 1;
            }
        }
        if (plist.Count >= 2 && (red > 0 && blue > 0))
        {
            GameObject.Find("ChoosePlayer").GetComponent<CanvasGroup>().alpha = 0;
            GameObject.Find("ChoosePlayer").GetComponent<CanvasGroup>().blocksRaycasts = false;
            GameObject.Find("GameChoose").GetComponent<CanvasGroup>().alpha = 1;
            GameObject.Find("GameChoose").GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
        else
        {
            Debug.Log("配置不對歐");
        }
    }
    public void gameNum(int num)
    {
        for (int i = 0; i < plist.Count; i++)
        {
            DontDestroyOnLoad(plist[i]);
        }
        SceneManager.LoadSceneAsync("FightScene", LoadSceneMode.Single);
    }
}

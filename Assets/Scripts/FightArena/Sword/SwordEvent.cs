using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class SwordEvent : MonoBehaviour
{
    [SerializeField] private GameObject UI;
    [SerializeField] private float rotateSpeed, waitToSword, hurtTime, waitTitleTime;
    private int totalPlayer;
    [SerializeField] GameObject StartButton;
    [SerializeField] GameObject StartUI;
    PhotonView PV;
    void Start()
    {
        PV = GetComponent<PhotonView>();
        if (!PhotonNetwork.IsMasterClient)
        {
            StartButton.SetActive(false);
        }
    }
    private void OnEnable()
    {
        transform.Rotate(0, 0, Random.Range(0f, 360f));
        totalPlayer = FightManager.Instance.plist.Count;
    }
    public void StartGame()
    {
        PV.RPC("RPC_StartGame", RpcTarget.All);  //廣播到所有玩家的電腦
    }
    [PunRPC]  //廣播的方法前面都需要加這個開頭
    void RPC_StartGame()
    {
        StartUI.SetActive(false);
        for (int i = 0; i < FightManager.Instance.plist.Count; i++)
        {
            GameObject.Find("HealthUI").transform.GetChild(i).gameObject.SetActive(true);
            FightManager.Instance.plist[i].GetComponent<arenaPlayer>().currentState = ArenaState.walk;
        }
        if (!PV.IsMine)
        {
            return;
        }
        StartCoroutine(randomSword(waitToSword));
        StartCoroutine(changeBG());
    }
    //地圖懸轉
    private IEnumerator changeBG()
    {
        this.transform.Rotate(0, 0, rotateSpeed);
        yield return null;
        StartCoroutine(changeBG());
    }
    private void Update()
    {
        if (FightManager.Instance.plist.Count <= 1)
        {
            UI.SetActive(true);
            if (FightManager.Instance.plist[0].GetComponent<arenaPlayer>().red)
            {
                UI.transform.Find("red").gameObject.SetActive(true);
            }
            else
            {
                UI.transform.Find("blue").gameObject.SetActive(true);
            }
            this.gameObject.SetActive(false);
        }
        else if (totalPlayer > FightManager.Instance.plist.Count)//來判斷是否有玩家死掉要重新生炸彈
        {
            totalPlayer = FightManager.Instance.plist.Count;
            if (!PV.IsMine)
            {
                return;
            }
            StartCoroutine(randomSword(waitToSword));
        }
    }
    //隨機給一個玩家劍
    private IEnumerator randomSword(float time)
    {
        yield return new WaitForSeconds(time);
        int num = Random.Range(0, FightManager.Instance.plist.Count);
        PV.RPC("RPC_randomSword",RpcTarget.All,num);
    }

    [PunRPC]
    void RPC_randomSword(int number)
    {
        FightManager.Instance.plist[number].transform.Find("sword").gameObject.SetActive(true);
        FightManager.Instance.plist[number].transform.Find("sword").GetComponent<sword>().hurtTime = hurtTime;
        FightManager.Instance.plist[number].transform.Find("sword").GetComponent<sword>().waitTime = waitTitleTime;
    }
}

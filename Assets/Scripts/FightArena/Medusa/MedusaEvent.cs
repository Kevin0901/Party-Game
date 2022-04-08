using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class MedusaEvent : MonoBehaviour
{
    [SerializeField] private GameObject UI;
    [SerializeField] private float waitToMove;
    private GameObject mirror, monster;
    [SerializeField] GameObject StartButton;
    [SerializeField] GameObject StartUI;
    PhotonView PV;
    private void OnEnable()
    {
        StartCoroutine(Shield_Set());
    }
    IEnumerator Shield_Set()
    {
        Player[] p = PhotonNetwork.PlayerList;
        if (p.Length == FightManager.Instance.plist.Count)
        {
            for (int i = 0; i < FightManager.Instance.plist.Count; i++)
            {
                FightManager.Instance.plist[i].transform.Find("shield").gameObject.SetActive(true);
            }
        }
        else
        {
            yield return new WaitForSeconds(0.1f);
            StartCoroutine(Shield_Set());
        }
    }
    void Start()
    {
        PV = GetComponent<PhotonView>();
        if (!PhotonNetwork.IsMasterClient)
        {
            StartButton.SetActive(false);
        }
        mirror = this.transform.Find("mirror").gameObject;
        monster = this.transform.Find("monster").gameObject;
    }
    void Update()
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
            this.transform.gameObject.SetActive(false);
        }
    }
    //開始遊戲
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
        StartCoroutine(wait(waitToMove));
    }
    //等待時間
    IEnumerator wait(float t)
    {
        yield return new WaitForSeconds(t);
        mirror.GetComponent<circleMirror>().enabled = true;
        monster.GetComponent<Animator>().enabled = true;
        monster.GetComponent<medusa>().enabled = true;
    }
}

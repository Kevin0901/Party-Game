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
    [SerializeField] GameObject UIBackGround;
    PhotonView PV;
    private void OnEnable()
    {
        for (int i = 0; i < FightManager.Instance.plist.Count; i++)
        {
            FightManager.Instance.plist[i].transform.Find("shield").gameObject.SetActive(true);
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
        PV.RPC("RPC_StartGame",RpcTarget.All);
    }
    [PunRPC]
    public void RPC_StartGame()
    {
        UIBackGround.SetActive(false);
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

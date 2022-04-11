using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.IO;
public class AresEvent : MonoBehaviour
{
    [SerializeField] private GameObject UI;
    [SerializeField] private List<GameObject> helmet;
    [SerializeField] private float aresTime;
    [SerializeField] GameObject StartButton;
    [SerializeField] GameObject UIBackGround;
    PhotonView PV;
    void Start()
    {
        PV = GetComponent<PhotonView>();
        if (!PhotonNetwork.IsMasterClient)
        {
            StartButton.SetActive(false);
        }
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
    public void StartGame()
    {
        PV.RPC("RPC_StartGame", RpcTarget.All);
    }
    [PunRPC]
    public void RPC_StartGame()
    {
        UIBackGround.SetActive(false);
        for (int i = 0; i < FightManager.Instance.plist.Count; i++)
        {
            FightManager.Instance.plist[i].GetComponent<arenaPlayer>().currentState = ArenaState.walk;
            GameObject.Find("HealthUI").transform.GetChild(i).gameObject.SetActive(true);
        }
        StartCoroutine(aresHelmet());
    }
    private IEnumerator aresHelmet()
    {
        yield return new WaitForSeconds(1);
        transform.Find("helmets").gameObject.SetActive(true);
        if (PV.IsMine)
        {
            int r = Random.Range(0, helmet.Count);
            PV.RPC("Pun_SetArens", RpcTarget.All, r);
        }
    }
    [PunRPC]
    void Pun_SetArens(int order)
    {
        helmet[order].GetComponent<helmet>().isArens = true;
        helmet[order].GetComponent<helmet>().aresTime = aresTime;
    }
    public IEnumerator reset()
    {
        for (int i = 0; i < transform.Find("helmets").childCount; i++)
        {
            if (!transform.Find("helmets").GetChild(i).gameObject.activeSelf)
            {
                transform.Find("helmets").GetChild(i).gameObject.SetActive(true);
            }
        }
        yield return new WaitForSeconds(aresTime);
        StartCoroutine(aresHelmet());
    }
}

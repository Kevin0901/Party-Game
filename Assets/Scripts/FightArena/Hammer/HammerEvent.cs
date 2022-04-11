using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.IO;
public class HammerEvent : MonoBehaviour
{
    [SerializeField] private GameObject UI, _hammer;
    public bool isEnd;
    [SerializeField] GameObject StartButton;
    [SerializeField] GameObject UIBackGround;
    PhotonView PV;
    private void Start()
    {
        PV = GetComponent<PhotonView>();
        if (!PhotonNetwork.IsMasterClient)
        {
            StartButton.SetActive(false);
        }
        StartCoroutine(Wait_SetPlayer());
    }
    IEnumerator Wait_SetPlayer()
    {
        yield return new WaitForSeconds(1f);
        SetPlayer();
    }
    private void SetPlayer()
    {
        for (int i = 0; i < FightManager.Instance.plist.Count; i++)
        {
            if (FightManager.Instance.plist[i].GetComponent<PhotonView>().IsMine)
            {
                FightManager.Instance.plist[i].GetComponent<arenaPlayer>().enabled = false;
                FightManager.Instance.plist[i].transform.rotation = Quaternion.Euler(0, 0, -180);
                switch (i)
                {
                    case 0:
                        FightManager.Instance.plist[i].GetComponent<arenaPlayer>().SpawnPoint(new Vector3(-45, 28, 0));
                        break;
                    case 1:
                        FightManager.Instance.plist[i].GetComponent<arenaPlayer>().SpawnPoint(new Vector3(-15, 28, 0));
                        break;
                    case 2:
                        FightManager.Instance.plist[i].GetComponent<arenaPlayer>().SpawnPoint(new Vector3(15, 28, 0));
                        break;
                    case 3:
                        FightManager.Instance.plist[i].GetComponent<arenaPlayer>().SpawnPoint(new Vector3(45, 28, 0));
                        break;
                }
                GameObject a = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "arena/Hammer/hammer"), FightManager.Instance.plist[i].transform.position,
                FightManager.Instance.plist[i].transform.rotation);
                PV.RPC("RPC_SetHammer", RpcTarget.All, a.GetComponent<PhotonView>().ViewID, FightManager.Instance.plist[i].GetComponent<PhotonView>().ViewID);
            }
        }
    }
    [PunRPC]
    public void RPC_SetHammer(int HamID, int iswho)
    {
        StartCoroutine(Set_Hammer(HamID,iswho));
    }
    IEnumerator Set_Hammer(int HamID, int iswho)
    {
        yield return new WaitForSeconds(1f);
        GameObject Ham = PhotonView.Find(HamID).gameObject;
        GameObject p = PhotonView.Find(iswho).gameObject;
        Ham.transform.SetParent(p.transform);
        Ham.transform.GetChild(0).GetComponent<strike>()._event = this.gameObject.GetComponent<HammerEvent>();
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
            if (FightManager.Instance.plist[i].GetComponent<PhotonView>().IsMine)
            {
                FightManager.Instance.plist[i].GetComponentInChildren<strike>().enabled = true;
            }
        }
    }
    public IEnumerator EndGame(GameObject winner)
    {
        isEnd = true;
        yield return new WaitForSeconds(1f);
        PV.RPC("RPC_End", RpcTarget.All, winner.GetComponent<PhotonView>().ViewID);
    }
    [PunRPC]
    public void RPC_End(int winnerID)
    {
        isEnd = true;
        UI.SetActive(true);
        GameObject winner = PhotonView.Find(winnerID).gameObject;
        if (winner.GetComponent<arenaPlayer>().red)
        {
            UI.transform.Find("red").gameObject.SetActive(true);
        }
        else
        {
            UI.transform.Find("blue").gameObject.SetActive(true);
        }
        this.gameObject.SetActive(false);
    }
}

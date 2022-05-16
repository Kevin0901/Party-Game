using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class MultiSwordChange : MonoBehaviour
{
    PhotonView PV;
    GameObject firstPlayer;
    // Start is called before the first frame update
    void Start()
    {
        PV = this.GetComponent<PhotonView>();
        if (PhotonView.Find((int)PV.InstantiationData[0]) != null)
        {
            firstPlayer = PhotonView.Find((int)PV.InstantiationData[0]).gameObject;

            firstPlayer.transform.Find("sword").gameObject.SetActive(true);
            firstPlayer.transform.Find("sword").gameObject.GetComponent<sword>().StartCoroutine("Savelastplayer", firstPlayer);
            firstPlayer.GetComponent<arenaPlayer>().StartCoroutine("changeColorTitle_Sword");
        }
        if(PV.IsMine)
        {
            PhotonNetwork.Destroy(this.gameObject);
        }
    }
}

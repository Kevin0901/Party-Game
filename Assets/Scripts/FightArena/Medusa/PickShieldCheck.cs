using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class PickShieldCheck : MonoBehaviour
{
    PhotonView PV;
    GameObject Find_Parent_Player;
    // Start is called before the first frame update
    void Start()
    {
        PV = this.GetComponent<PhotonView>();
        if (PhotonView.Find((int)PV.InstantiationData[0]).gameObject != null)
        {
            Find_Parent_Player = PhotonView.Find((int)PV.InstantiationData[0]).gameObject;
            Find_Parent_Player.transform.Find("shield").gameObject.SetActive(true);
        }
        PhotonNetwork.Destroy(this.gameObject);
    }
}

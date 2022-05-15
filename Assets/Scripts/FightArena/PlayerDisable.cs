using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class PlayerDisable : MonoBehaviour
{
    PhotonView PV;
    GameObject Find_Parent_Player;
    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();  //定義PhotonView
        Find_Parent_Player = PhotonView.Find((int)PV.InstantiationData[0]).gameObject;
        this.gameObject.transform.SetParent(Find_Parent_Player.transform);
        this.transform.parent.gameObject.SetActive(false);
    }
}

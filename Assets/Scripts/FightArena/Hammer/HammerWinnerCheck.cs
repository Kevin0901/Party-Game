using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class HammerWinnerCheck : MonoBehaviour
{
    PhotonView PV;
    GameObject WinnerP;
    // Start is called before the first frame update
    void Start()
    {
        PV = this.GetComponent<PhotonView>();
        if (PhotonView.Find((int)PV.InstantiationData[0]) != null)
        {
            WinnerP = PhotonView.Find((int)PV.InstantiationData[0]).gameObject;
            GameObject.Find("EventManager").transform.Find("hammer").Find("event").gameObject.GetComponent<HammerEvent>().End((int)PV.InstantiationData[0]);
        }
    }
}

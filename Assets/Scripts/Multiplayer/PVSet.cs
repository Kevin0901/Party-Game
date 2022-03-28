using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
public class PVSet : MonoBehaviour
{
    public MultiPlayerManager MultiPlayerManager;
    public PhotonView PV;
    void Awake()
    {
        // PV = GetComponent<PhotonView>();
        // MultiPlayerManager = PhotonView.Find((int)PV.InstantiationData[0]).GetComponent<MultiPlayerManager>();
    }
}

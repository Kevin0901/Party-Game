using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class WaitLoadComplete : MonoBehaviour
{
    PhotonView PV;
    // Start is called before the first frame update
    void Start()
    {
        PV = this.GetComponent<PhotonView>();
        transform.SetParent(GameObject.Find("LoadSceneCompletePool").transform);
    }
}

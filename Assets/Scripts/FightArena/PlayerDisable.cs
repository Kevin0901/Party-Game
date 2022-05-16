using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
public class PlayerDisable : MonoBehaviour
{
    PhotonView PV;
    GameObject Find_Parent_Player;
    // Start is called before the first frame update
    void Start()
    {
        PV = GetComponent<PhotonView>();  //定義PhotonView
        if (PhotonView.Find((int)PV.InstantiationData[0]) != null)
        {
            Find_Parent_Player = PhotonView.Find((int)PV.InstantiationData[0]).gameObject;
            this.gameObject.transform.SetParent(Find_Parent_Player.transform);
            this.transform.parent.gameObject.SetActive(false);
        }
    }
    private void Update()
    {
        if(SceneManager.GetActiveScene().name.Equals("MainScene") && PV.IsMine)
        {
            PhotonNetwork.Destroy(this.gameObject);
        }
    }
}

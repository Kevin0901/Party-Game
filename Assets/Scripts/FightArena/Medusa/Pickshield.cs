using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
public class Pickshield : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == 10 && !other.gameObject.transform.Find("shield").gameObject.activeSelf 
            && other.gameObject.GetComponent<PhotonView>().IsMine)
        {
            // other.gameObject.transform.Find("shield").gameObject.SetActive(true);
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "arena/Medusa/PickShieldCheck"), Vector3.zero, this.transform.rotation
                , 0, new object[] { other.gameObject.GetComponent<PhotonView>().ViewID });
            PhotonNetwork.Destroy(this.gameObject);
        }
    }

}

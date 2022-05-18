using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
public class Pickshield : MonoBehaviour
{
    PhotonView PV;
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == 10 && !other.gameObject.transform.Find("shield").gameObject.activeSelf)
        {
            // other.gameObject.transform.Find("shield").gameObject.SetActive(true);
            if (other.gameObject.GetComponent<PhotonView>().IsMine)
            {
                PV = this.GetComponent<PhotonView>();
                PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "arena/Medusa/PickShieldCheck"), Vector3.zero, this.transform.rotation
                                , 0, new object[] { other.gameObject.GetComponent<PhotonView>().ViewID, PV.ViewID });
            }
            Destroy(this.gameObject);
        }
    }

}

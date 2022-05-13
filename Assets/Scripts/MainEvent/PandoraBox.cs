using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class PandoraBox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 10)
        {
            if (other.gameObject.GetComponent<PhotonView>().IsMine)
            {
                other.gameObject.GetComponent<Effect>().StartCoroutine("RandGiveEffect");
                PhotonNetwork.Destroy(this.gameObject);
            }
        }
    }
}

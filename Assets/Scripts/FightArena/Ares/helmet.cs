using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class helmet : MonoBehaviour
{
    public bool isArens;
    public float aresTime;
    PhotonView PV;
    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == 10 && Input.GetKey(KeyCode.Space))
        {
            if (isArens)
            {
                // if (other.GetComponent<PhotonView>().IsMine)
                // {
                    other.GetComponent<arenaPlayer>().StartCoroutine("ChangeARES", aresTime);
                // }
                PV.RPC("RPC_IsArens", RpcTarget.All);
            }
            else
            {
                PV.RPC("RPC_NoArens", RpcTarget.All, this.gameObject.name);
            }
        }
    }
    [PunRPC]
    void RPC_NoArens(string name)
    {
        if (this.gameObject.name.Equals(name))
        {
            this.transform.gameObject.SetActive(false);
        }
    }
    [PunRPC]
    void RPC_IsArens()
    {
        this.transform.parent.gameObject.SetActive(false);
        this.transform.parent.GetComponentInParent<AresEvent>().StartCoroutine("reset");
    }
}

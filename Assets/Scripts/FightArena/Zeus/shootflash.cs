using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class shootflash : MonoBehaviour
{
    [HideInInspector] public GameObject shooter;
    [HideInInspector] public float damege;
    PhotonView PV;
    GameObject PAPAPlayer;
    private void Start()
    {
        PV = this.GetComponent<PhotonView>();
        if (PhotonView.Find((int)PV.InstantiationData[0]) != null)
        {
            PAPAPlayer = PhotonView.Find((int)PV.InstantiationData[0]).gameObject;
            shooter = PAPAPlayer;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 10 && other.gameObject != shooter)
        {
            other.GetComponent<arenaPlayer>().hurt(damege);
            PhotonView.Find((int)PV.InstantiationData[0]).gameObject.transform.Find("NumTitle").GetChild(shooter.GetComponent<arenaPlayer>().p_index).GetComponent<SpriteRenderer>().color = Color.white;;
        }
    }
}

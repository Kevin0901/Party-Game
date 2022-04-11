using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class pickcow : MonoBehaviour
{
    public int cowScore;
    PhotonView PV;
    void Start()
    {
        PV = GetComponent<PhotonView>();  //定義PhotonView
        this.transform.parent = PhotonView.Find((int)PV.InstantiationData[0]).transform;
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == 10)
        {
            if (other.GetComponent<arenaPlayer>().red)
            {
                this.transform.parent.GetComponent<HermesEvent>().R_ScoreADD(cowScore);
                Destroy(this.gameObject);
            }
            else
            {
                this.transform.parent.GetComponent<HermesEvent>().B_ScoreADD(cowScore);
                Destroy(this.gameObject);
            }
        }
    }
}

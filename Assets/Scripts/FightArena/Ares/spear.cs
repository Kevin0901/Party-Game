using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class spear : MonoBehaviour
{
    private Animator mAnimator;
    public GameObject player;
    PhotonView PV;
    private void Start()
    {
        PV = GetComponent<PhotonView>();
        mAnimator = GetComponent<Animator>();
    }
    private void Update()
    {
        if(!PV.IsMine)
        {
            return;
        }
        aresSpear();
    }
    //戰神揮矛
    private void aresSpear()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mAnimator.SetTrigger("rotate");
        }
    }
    private void LateUpdate()
    {
        if(!PV.IsMine)
        {
            return;
        }
        transform.position = player.transform.position;
    }

    
}

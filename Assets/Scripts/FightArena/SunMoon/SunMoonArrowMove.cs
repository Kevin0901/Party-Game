using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class SunMoonArrowMove : MonoBehaviour
{
    public bool isSun, noEffect;
    public float speed, power;
    private Vector3 dir;
    private Rigidbody2D mrigibody2D;
    public GameObject shooter;
    BoxCollider2D BC2D;
    PhotonView PV;
    private void Start()
    {
        BC2D = GetComponent<BoxCollider2D>();
        mrigibody2D = this.GetComponent<Rigidbody2D>();
        PV = GetComponent<PhotonView>();
    }
    //箭矢速度
    void FixedUpdate()
    {
        if (noEffect)
        {
            mrigibody2D.velocity = dir.normalized * speed;
        }
        else if (isSun)
        {
            mrigibody2D.velocity = dir.normalized * speed * 2.25f;
        }
        else
        {
            mrigibody2D.velocity = dir.normalized * speed * 0.35f;
        }
    }
    //箭矢摧毀
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 10 && other.gameObject != shooter)
        {
            if(!PV.IsMine)
            {
                other.GetComponent<Rigidbody2D>().AddForce(dir.normalized * power, ForceMode2D.Impulse);
            }else if(PV.IsMine)
            {
                PhotonNetwork.Destroy(this.gameObject);
            }
        }
    }
    //離開背景
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("background"))
        {
            if(PV.IsMine)
            {
                PhotonNetwork.Destroy(this.gameObject);
            }
        }
    }
    //射擊方向
    public void setArrow()
    {
        Quaternion k = Quaternion.AngleAxis(transform.rotation.eulerAngles.z, Vector3.forward);
        dir = k * Vector3.up; //得知射的方向
        noEffect = true;
    }
}

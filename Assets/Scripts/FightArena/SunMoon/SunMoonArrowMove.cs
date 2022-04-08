using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class SunMoonArrowMove : MonoBehaviour
{
    public bool isSun, noEffect;
    public float speed, power;
    private Vector3 dir;
    private Rigidbody2D mrigibody2D;
    public GameObject shooter;
    PhotonView PV;
    BoxCollider2D BC2D;
    void Awake()
    {
        
    }
    private void Start()
    {
        BC2D = GetComponent<BoxCollider2D>();
        mrigibody2D = this.GetComponent<Rigidbody2D>();
        StartCoroutine(wait());
    }
    IEnumerator wait()
    {
        yield return new WaitForSeconds(1f);
        PV = GetComponent<PhotonView>();
        if(!PV.IsMine)
        {
            BC2D.enabled = false;
        }
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
            other.GetComponent<Rigidbody2D>().AddForce(dir.normalized * power, ForceMode2D.Impulse);
            Destroy(this.gameObject);
        }
    }
    //離開背景
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("background"))
        {
            Destroy(this.gameObject);
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

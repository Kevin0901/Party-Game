using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerShoot : MonoBehaviour
{
    [SerializeField] private GameObject item;
    private arenaPlayer player;
    public bool isCenter; //是否在正中心
    [Header("箭矢設定")]
    public float speed = 47.5f;
    public float mass = 2000;
    [Header("攻擊間隔")]
    [SerializeField] private float fireRate;
    private float nextfire;
    private void Start()
    {
        player = this.gameObject.GetComponent<arenaPlayer>();
        nextfire = 0;
        isCenter = false;
    }
    private void Update()
    {
        if (Input.GetAxis("R2-0") != 0 && Time.time > nextfire)
        {
            GameObject a = Instantiate(item, transform.position, transform.rotation);
            Physics2D.IgnoreCollision(a.GetComponent<Collider2D>(), player.GetComponent<Collider2D>()); //忽略自己
            a.GetComponent<Rigidbody2D>().mass = mass;
            if (isCenter) //如果在正中心發射的話
            {
                a.GetComponent<SpriteRenderer>().color = new Color32(0, 0, 0, 255);
                a.GetComponent<SunMoonArrowMove>().speed = speed * 1.5f;
                a.transform.localScale *= 1.5f;
            }
            else
            {
                a.GetComponent<SunMoonArrowMove>().speed = speed;
            }
            a.GetComponent<SunMoonArrowMove>().setArrow();
            nextfire = Time.time + fireRate; //下次發射的時間
        }
    }
}

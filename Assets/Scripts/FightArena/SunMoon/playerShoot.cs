using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerShoot : MonoBehaviour
{
    [SerializeField] private GameObject item;
    private arenaPlayer player;
    public bool isCenter; //是否在正中心
    [Header("箭矢設定")]
    public float power = 50;
    public float speed = 40;
    [Header("攻擊間隔")]
    [SerializeField] private float fireRate;
    private float nextfire;
    private void Start()
    {
        player = this.gameObject.GetComponent<arenaPlayer>();
        nextfire = 0;
        isCenter = false;
    }
    void Update()
    {
        // if (Input.GetAxis("R2-" + player.joynum) != 0)
        // {
        //     delayShoot = Time.time;
        // }
        if (Input.GetAxis("R2-" + player.joynum) != 0 && Time.time > nextfire)
        {
            GameObject a = Instantiate(item, transform.position, item.transform.rotation);
            if (isCenter) //如果在正中心發射的話
            {
                a.GetComponent<SpriteRenderer>().color = new Color32(0, 0, 0, 0);
                a.GetComponent<SunMoonArrowMove>().power = power + 50;
                a.GetComponent<SunMoonArrowMove>().speed = speed * 1.5f;
                a.transform.localScale *= 1.5f;
            }
            else
            {
                a.GetComponent<SunMoonArrowMove>().power = power;
                a.GetComponent<SunMoonArrowMove>().speed = speed;
            }
            a.GetComponent<SunMoonArrowMove>().setRotate(this.transform.rotation.eulerAngles.z);
            a.GetComponent<SunMoonArrowMove>().setArrow(this.gameObject);
            nextfire = Time.time + fireRate;
        }
    }
}

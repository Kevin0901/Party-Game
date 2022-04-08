using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerShoot : MonoBehaviour
{
    [SerializeField] private GameObject item;
    private arenaPlayer player;
    public bool isCenter;
    [Header("箭矢設定")]
    private Team t;
    public float power = 50;
    public float speed = 40;
    [SerializeField] private float fireRate;
    private float nextfire, delayShoot;
    private void Start()
    {
        t = this.gameObject.GetComponent<Team>();
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
            a.GetComponent<SunMoonArrowMove>().enemy = t.Enemyteam;
            if (isCenter)
            {
                a.GetComponent<SpriteRenderer>().color = new Color32(37, 135, 0, 255);
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

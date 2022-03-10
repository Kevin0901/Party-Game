using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class playerShoot : MonoBehaviour
{
    [SerializeField] private GameObject item;
    public bool isCenter; //是否在正中心
    [Header("箭矢設定")]
    public float speed;
    public float mass;
    [Header("攻擊間隔")]
    [SerializeField] private float fireRate;
    private float nextfire;
    private PlayerInput controls;
    private void Awake()
    {
        controls = this.GetComponent<PlayerInput>();
    }
    private void Start()
    {
        nextfire = 0;
        isCenter = false;
    }
    private void OnEnable()
    {
        controls.actions["shoot"].performed += shoot;
    }
    private void OnDisable()
    {
        controls.actions["shoot"].performed -= shoot;
    }
    public void shoot(InputAction.CallbackContext ctx)
    {
        if (ctx.ReadValueAsButton() && Time.time > nextfire)
        {
            GameObject a = Instantiate(item, transform.position, transform.rotation);
            Physics2D.IgnoreCollision(a.GetComponent<Collider2D>(), this.GetComponent<Collider2D>()); //忽略自己

            if (isCenter) //如果在正中心發射的話
            {
                a.GetComponent<SpriteRenderer>().color = new Color32(0, 0, 0, 255);
                a.GetComponent<Rigidbody2D>().mass = mass * 2f;
                a.GetComponent<SunMoonArrowMove>().speed = speed * 1.8f;
                a.transform.localScale *= 1.5f;
            }
            else
            {
                a.GetComponent<Rigidbody2D>().mass = mass;
                a.GetComponent<SunMoonArrowMove>().speed = speed;
            }
            a.GetComponent<SunMoonArrowMove>().setArrow();
            nextfire = Time.time + fireRate; //下次發射的時間
        }
    }
}

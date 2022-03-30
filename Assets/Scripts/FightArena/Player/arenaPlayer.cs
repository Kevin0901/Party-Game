using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ArenaState
{
    idle,
    walk,
    love,
    lighting,
    shoot,
    fastMode,
    punch
}
public class arenaPlayer : MonoBehaviour
{
    [Header("玩家基礎數值")]
    public ArenaState currentState;
    public float speed;
    private float curH;
    public bool red;
    public int p_index;
    public Vector3 spawnPos;
    [Header("月亮太陽射擊設定")]
    [SerializeField] private GameObject item;
    [SerializeField] private float fireRate, Arrowspeed = 50, power;
    private float nextfire;
    public bool isCenter; //玩家是否在正中心
    [Header("邱比特設定")]
    [SerializeField] private float loveTime = 2f;
    private float nextlove;
    public int love_index;
    [Header("宙斯")]
    [SerializeField] private GameObject lighting;
    [SerializeField] private float powerTime;
    public bool isPress;
    [Header("荷米斯")]
    public float turboSpeed;
    [Header("足球")]
    [SerializeField] private float fistpower;

    private Rigidbody2D mrigibody;
    private Animator mAnimator;
    [HideInInspector] public SpriteRenderer titleColor;
    private heart lifeUI;
    private Vector2 movement;
    void Start()
    {
        mrigibody = this.GetComponent<Rigidbody2D>();
        mAnimator = this.GetComponent<Animator>();
        nextlove = loveTime;
        nextfire = 0;
        curH = 3f;
        isCenter = false;
    }
    //偵測按鍵輸入
    private void Update()
    {
        mouseRotate();
        if (currentState == ArenaState.lighting)
        {
            ShootLight();
        }
        else if (currentState == ArenaState.punch && (Input.GetMouseButtonDown(0)))
        {
            transform.Find("fist").GetComponent<Animator>().SetTrigger("punch");
            transform.Find("fist").GetComponent<fist>().punch(fistpower);
        }
        else if (currentState == ArenaState.shoot)
        {
            shootArrow();
        }
    }
    //玩家狀態更新
    void FixedUpdate()
    {
        if (currentState == ArenaState.love)
        {
            Lover();
        }
        else if (currentState == ArenaState.fastMode)
        {
            Move();
            if (movement != Vector2.zero)
            {
                mrigibody.AddForce(movement * turboSpeed, ForceMode2D.Impulse);
            }
        }
        else if (currentState == ArenaState.lighting && !isPress)
        {
            Move();
        }
        else if (currentState != ArenaState.idle && currentState != ArenaState.lighting)
        {
            Move();
        }
    }
    //滑鼠旋轉玩家
    private void mouseRotate()
    {
        Vector3 pos = Input.mousePosition;
        pos = Camera.main.ScreenToWorldPoint(pos);
        Vector2 line = pos - this.transform.position;
        float rotate = Mathf.Atan2(line.y, line.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(rotate - 90, Vector3.forward);
    }
    //移動玩家
    private void Move()
    {
        movement = Vector2.zero;
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");
        if (movement != Vector2.zero)
        {
            mrigibody.AddForce(movement * speed, ForceMode2D.Force);
        }
    }
    //魅惑
    private void Lover()
    {
        if (love_index >= FightManager.Instance.plist.Count)
        {
            love_index = FightManager.Instance.plist.Count - 1;
            if (love_index == p_index && love_index != 0)
            {
                love_index = love_index - 1;
            }
        }
        Vector3 pos = FightManager.Instance.plist[love_index].transform.position;
        mrigibody.AddForce((pos - this.transform.position).normalized * speed * 0.66f, ForceMode2D.Force);
        loveTime -= Time.deltaTime;
        if (loveTime < 0)
        {
            loveTime = nextlove;
            titleColor.color = Color.white;
            currentState = ArenaState.walk;
        }
    }
    //射箭
    private void shootArrow()
    {
        if (Input.GetMouseButtonDown(0) && Time.time > nextfire)
        {
            GameObject arr = Instantiate(item, transform.position, transform.rotation);
            if (isCenter) //如果在正中心發射的話
            {
                arr.GetComponent<SpriteRenderer>().color = new Color32(0, 0, 0, 255);
                arr.GetComponent<SunMoonArrowMove>().speed = Arrowspeed * 1.75f;
                arr.GetComponent<SunMoonArrowMove>().power = power * 1.25f;
                arr.transform.localScale *= 1.25f;
            }
            else
            {
                arr.GetComponent<SunMoonArrowMove>().speed = Arrowspeed;
                arr.GetComponent<SunMoonArrowMove>().power = power;
            }
            arr.GetComponent<SunMoonArrowMove>().setArrow();
            nextfire = Time.time + fireRate; //下次發射的時間
        }
    }
    //閃電發射
    private void ShootLight()
    {
        if (Input.GetMouseButton(0))
        {
            isPress = true;
            mrigibody.velocity = Vector2.zero;
            powerTime += Time.deltaTime;
            if (powerTime < 2.5)
                titleColor.color = new Color(1, 1 - (powerTime * 0.128f), 1 - (powerTime * 0.348f));
            else
            {
                titleColor.color = Color.red;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Debug.Log(powerTime);
            isPress = false;
            GameObject a = Instantiate(lighting, transform.position,
            lighting.transform.rotation * this.transform.rotation);
            if (powerTime < 2.5)
            {
                a.transform.localScale += new Vector3(a.transform.localScale.x * 2 * powerTime, 0, 0);
                a.GetComponent<shootflash>().damege = 0.5f;
            }
            else
            {
                a.transform.localScale += new Vector3(a.transform.localScale.x * 8, 0, 0);
                a.GetComponent<shootflash>().damege = 1f;

            }
            titleColor.color = Color.white;
            a.GetComponent<shootflash>().shooter = this.gameObject;
            currentState = ArenaState.walk;
            powerTime = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            mrigibody.AddForce(movement * speed * 0.85f, ForceMode2D.Impulse);
            currentState = ArenaState.walk;
        }
    }
    //重生點
    public void SpawnPoint(Vector3 pos)
    {
        this.transform.position = pos;
        spawnPos = pos;
    }
    public void SpawnPoint()
    {
        this.transform.position = spawnPos;
    }
    //玩家受到傷害
    public void hurt(float damege)
    {
        curH -= damege;
        lifeUI.hurt(curH);
        if (curH <= 0)
        {
            this.gameObject.SetActive(false);
        }
        mAnimator.SetTrigger("hurt");
    }
    public void setUI()
    {
        this.transform.Find("NumTitle").GetChild(p_index).gameObject.SetActive(true);
        lifeUI = GameObject.Find("HealthUI").transform.Find("P" + (p_index + 1)).GetComponent<heart>();
        titleColor = this.transform.Find("NumTitle").GetChild(p_index).GetComponent<SpriteRenderer>();
    }
    //玩家關閉
    private void OnDisable()
    {
        if (!this.gameObject.activeSelf)
            FightManager.Instance.plist.Remove(this.gameObject);
    }
    //改變Title顏色
    public IEnumerator changeColorTitle_Sword()
    {
        titleColor.color = new Color32(34, 179, 229, 255);
        yield return new WaitForSeconds(transform.Find("sword").GetComponent<sword>().gaveTime);
        titleColor.color = Color.white;
    }
}

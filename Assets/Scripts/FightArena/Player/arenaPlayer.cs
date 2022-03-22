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
    [SerializeField] private float fireRate, Arrowspeed = 50;
    [SerializeField] private float mass = 1000;
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
    private SpriteRenderer sprite;
    private Rigidbody2D mrigibody;
    private Animator mAnimator;
    private GameObject ui;
    private Vector2 movement;
    [Header("足球")]
    [SerializeField] private float fistpower;

    void Start()
    {
        mrigibody = this.GetComponent<Rigidbody2D>();
        mAnimator = this.GetComponent<Animator>();
        sprite = this.GetComponent<SpriteRenderer>();
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
        else if (Input.GetMouseButtonDown(0) && currentState == ArenaState.punch)
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
        if (currentState != ArenaState.idle && currentState != ArenaState.lighting)
        {
            Move();
        }
        else if (currentState == ArenaState.love)
        {
            Lover();
        }
        else if (currentState == ArenaState.fastMode)
        {
            if (movement != Vector2.zero)
            {
                mrigibody.AddForce(movement * turboSpeed, ForceMode2D.Impulse);
            }
        }
        else if (currentState == ArenaState.lighting && !isPress)
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
        if (love_index >= FightManager.Instance.gamelist.Count)
        {
            love_index = FightManager.Instance.gamelist.Count - 1;
            if (love_index == p_index && love_index != 0)
            {
                love_index = love_index - 1;
            }
        }
        Vector3 pos = FightManager.Instance.gamelist[love_index].transform.position;
        mrigibody.AddForce((pos - this.transform.position).normalized * speed, ForceMode2D.Force);
        loveTime -= Time.deltaTime;
        if (loveTime < 0)
        {
            loveTime = nextlove;
            this.transform.Find("NumTitle").GetChild(p_index).GetComponent<SpriteRenderer>().color = Color.white;
            currentState = ArenaState.walk;
        }
    }
    //射箭
    private void shootArrow()
    {
        if (Input.GetMouseButtonDown(0) && Time.time > nextfire)
        {
            GameObject arr = Instantiate(item, transform.position, transform.rotation);
            Physics2D.IgnoreCollision(arr.GetComponent<Collider2D>(), this.GetComponent<Collider2D>()); //忽略自己讓箭矢不會射到自己
            if (isCenter) //如果在正中心發射的話
            {
                arr.GetComponent<SpriteRenderer>().color = new Color32(0, 0, 0, 255);
                arr.GetComponent<Rigidbody2D>().mass = mass * 2f;
                arr.GetComponent<SunMoonArrowMove>().speed = Arrowspeed * 1.8f;
                arr.transform.localScale *= 1.5f;
            }
            else
            {
                arr.GetComponent<Rigidbody2D>().mass = mass;
                arr.GetComponent<SunMoonArrowMove>().speed = Arrowspeed;
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
                this.transform.Find("NumTitle").GetChild(p_index).GetComponent<SpriteRenderer>().color = new Color(1, 1 - (powerTime * 0.135f), 1 - (powerTime * 0.5f), 1);
            else
            {
                this.transform.Find("NumTitle").GetChild(p_index).GetComponent<SpriteRenderer>().color = Color.red;
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
            }
            else
            {
                a.transform.localScale += new Vector3(a.transform.localScale.x * 6f, 0, 0);
            }
            this.transform.Find("NumTitle").GetChild(p_index).GetComponent<SpriteRenderer>().color = Color.white;
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
        GameObject.Find("HealthUI").transform.Find("P" + (p_index + 1)).GetComponent<heart>().hurt(curH);
        if (curH <= 0)
        {
            this.gameObject.SetActive(false);
        }
        mAnimator.SetTrigger("hurt");
    }
    //玩家關閉(reset)
    private void OnDisable()
    {
        FightManager.Instance.gamelist.Remove(this.gameObject);
        this.GetComponent<arenaPlayer>().currentState = ArenaState.idle;
        curH = 3;
    }
    //改變Title顏色
    private IEnumerator changeColorTitle_Sword()
    {
        this.transform.Find("NumTitle").GetChild(p_index).GetComponent<SpriteRenderer>().color = new Color32(34, 179, 229, 255);
        yield return new WaitForSeconds(transform.Find("sword").GetComponent<sword>().gaveTime);
        this.transform.Find("NumTitle").GetChild(p_index).GetComponent<SpriteRenderer>().color = Color.white;
    }
}

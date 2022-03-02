using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum MonsterState
{
    walk,
    attack,
    track,
    idle,
    pigeon
}
[RequireComponent(typeof(monsterMove))]
[RequireComponent(typeof(LaserDetect))]
[RequireComponent(typeof(Team))]
public class monsterMove : MonoBehaviour
{
    private static int redsort, bluesort;
    [HideInInspector] public Animator animator;//動畫
    [HideInInspector] public Vector3 enemyPos;//敵人位置

    [Header("怪物數值設定")]
    [SerializeField] private float dir = 1f;//走路方向
    [SerializeField] private float setTime = 1f;//放置時間
    public float attackRange = 2f;//攻擊範圍
    public int attackDamage;
    public float speed = 1.5f;//走路速度
    [SerializeField] private int MaxHealth, CurHealth;

    [Header("每幾秒攻擊一次")]
    [SerializeField] private float attackRate = 1f; //攻擊間隔
    [Header("召喚消耗")]
    public ResourceAmount[] CostArray;

    [Header("怪物狀態")]
    public MonsterState currentState;//角色狀態

    [Header("偵測清單上的怪物")]
    public List<GameObject> EnemyList = new List<GameObject>();//進偵測範圍的第一個人寫進陣列裡
    private float nextAttack;//下次攻擊時間
    private bool fix; //是否修正方向.
    private health health;
    private Rigidbody2D rigi;
    private SpriteRenderer spriteRenderer;
    [Header("往上的圖")]
    [SerializeField] private Sprite up;
    [Header("往下的圖")]
    [SerializeField] private Sprite down;
    public int pigeon;
    //喚醒設定
    private void Awake()
    {
        currentState = MonsterState.idle;
        health = this.GetComponentInChildren<health>();
        health.maxH = MaxHealth;
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        if (this.gameObject.tag == "red")
        {
            redsort++;
            spriteRenderer.sprite = up;
            spriteRenderer.sortingLayerName = "RedMonster";
            spriteRenderer.sortingOrder = redsort;
            dir = 1f;
        }
        else
        {
            bluesort++;
            spriteRenderer.sprite = down;
            spriteRenderer.sortingLayerName = "BlueMonster";
            spriteRenderer.sortingOrder = bluesort;
            dir = -1f;
        }
        this.gameObject.GetComponent<LaserDetect>().enabled = false;
        animator = this.gameObject.GetComponent<Animator>();
        animator.enabled = false;
    }
    //開始設定
    void Start()
    {
        rigi = this.GetComponent<Rigidbody2D>();
        animator.SetFloat("attackSpeed", 1 / attackRate);
        nextAttack = 0;
        attackRange *= this.gameObject.transform.localScale.x;
        currentState = MonsterState.idle;
        StartCoroutine(waitIdle(setTime));
    }
    //等待時間
    private IEnumerator waitIdle(float t)
    {
        yield return new WaitForSeconds(t);
        animator.enabled = true;
        this.gameObject.GetComponent<LaserDetect>().enabled = true;
        animator.SetFloat("moveY", dir); //預設動畫方向
        currentState = MonsterState.walk;
    }
    //更新
    void Update()
    {
        fix = this.gameObject.GetComponent<LaserDetect>().isfix;
        CurHealth = health.curH;
        monsterBehavior();
        if (pigeon == 1)
        {
            StartCoroutine(Pigeonsec());
        }
    }
    private void OnEnable()
    {
        animator.SetFloat("attackSpeed", 1 / attackRate);
    }
    //怪物動作
    void monsterBehavior()
    {
        if (currentState == MonsterState.attack)
        {
            animator.SetBool("moving", false);
            if (nextAttack > 0)
            {
                nextAttack -= Time.deltaTime;
            }
            else
            {
                Vector2 direction = enemyPos - transform.position;
                direction = direction.normalized;
                animator.SetFloat("moveX", Mathf.RoundToInt(direction.x));
                animator.SetFloat("moveY", Mathf.RoundToInt(direction.y));
                StartCoroutine(AttackCo());
            }
        }
        else if (currentState == MonsterState.track && (fix != true) && currentState != MonsterState.attack)
        {
            if (nextAttack > 0)
            {
                nextAttack -= Time.deltaTime;
            }
            Vector2 direction = enemyPos - transform.position;
            direction = direction.normalized;
            transform.position = Vector3.MoveTowards(transform.position, enemyPos, Time.deltaTime * speed); //往敵人方向移動
            animator.SetFloat("moveX", direction.x);
            animator.SetFloat("moveY", direction.y);
            animator.SetBool("moving", true);
        }
        else if (currentState == MonsterState.walk)
        {
            transform.position += new Vector3(0, speed, 0) * Time.deltaTime * dir; //持續往前進
            animator.SetFloat("moveX", 0);
            animator.SetFloat("moveY", dir);
            animator.SetBool("moving", true);
            nextAttack = 0;
        }
    }
    //攻擊
    private IEnumerator AttackCo()
    {
        rigi.mass -= 200f;
        nextAttack = attackRate;
        animator.SetBool("attacking", true);
        currentState = MonsterState.attack;
        yield return null;
        animator.SetBool("attacking", false);
        yield return new WaitForSeconds(attackRate);
        rigi.mass += 200f;
    }
    private IEnumerator Pigeonsec()
    {
        animator.SetBool("moving", false);
        animator.SetBool("attacking", false);
        animator.SetBool("transform", true);
        currentState = MonsterState.pigeon;
        yield return null;
        animator.SetBool("transform", false);
        yield return new WaitForSeconds(5);
        animator.SetBool("moving", true);
        currentState = MonsterState.walk;
        pigeon = 0;
    }

    public IEnumerator MonsterStartStone()
    {
        StartCoroutine(MonsterStartStone(5));
        yield return null;
    }
    public IEnumerator MonsterStartStone(int time)
    {
        this.currentState = MonsterState.idle;
        this.GetComponent<SpriteRenderer>().color = new Color32(89, 89, 89, 255);
        yield return new WaitForSeconds(time);
        this.currentState = MonsterState.walk;
        this.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
    }
}
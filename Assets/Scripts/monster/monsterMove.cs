using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum MonsterState
{
    idle,
    walk,
    track,
    attack,
    pigeon
}
[RequireComponent(typeof(monsterMove))]
[RequireComponent(typeof(Team))]
public class monsterMove : MonoBehaviour
{
    [HideInInspector] public static int redsort, bluesort;
    [HideInInspector] public Animator animator;//動畫
    [Header("初始數值")]
    [SerializeField] private float walkDir_Y;
    [SerializeField] private float setTime;
    [Header("怪物數值")]
    public MonsterState currentState;
    public float speed;
    [SerializeField] private int MaxHealth;
    private int CurHealth;
    [SerializeField] private float attackRate;
    [SerializeField] public int attackDamage;
    [SerializeField] private float attackRange;
    [SerializeField] private float detectRange;
    [Header("召喚消耗")]
    public ResourceAmount[] CostArray;
    [Header("追蹤目標")]
    public GameObject enemy;
    private float nextAttack;//下次攻擊時間
    RaycastHit2D[] raycast2D = new RaycastHit2D[3]; //雷射
    LayerMask mask;  //遮罩
    private float angle;
    [Header("雷射發射長度")]
    [SerializeField] private float dis; //發射長度
    private bool isFix;
    private health health;
    private Team t;
    private SpriteRenderer spriteRenderer;
    [Header("起始圖片_上")]
    [SerializeField] private Sprite up;
    [Header("起始圖片_下")]
    [SerializeField] private Sprite down;
    private void Awake()
    {
        health = this.GetComponentInChildren<health>();
        animator = this.gameObject.GetComponent<Animator>();
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        health.maxH = MaxHealth;

        if (this.gameObject.tag == "red") //如果是紅隊
        {
            redsort++;
            spriteRenderer.sprite = up;
            spriteRenderer.sortingLayerName = "RedMonster";
            spriteRenderer.sortingOrder = redsort;
            walkDir_Y = 1f;
        }
        else if (this.gameObject.tag == "blue")
        {
            bluesort++;
            spriteRenderer.sprite = down;
            spriteRenderer.sortingLayerName = "BlueMonster";
            spriteRenderer.sortingOrder = bluesort;
            walkDir_Y = -1f;
        }
    }
    void Start()
    {
        t = this.GetComponent<Team>();
        animator.SetFloat("attackSpeed", 1 / attackRate);
        nextAttack = 0;
        dis *= this.gameObject.transform.localScale.x;
        mask = 1 << 9 | 1 << 10 | 1 << 11;
        angle = 35;
        // StartCoroutine(waitIdle(setTime));
    }
    void Update()
    {
        CurHealth = health.curH;
        fixPosition();
        detectEnemy();
        monsterState();
    }
    private IEnumerator waitIdle(float t)
    {
        yield return new WaitForSeconds(t);
        animator.SetFloat("moveY", walkDir_Y); //預設動畫方向
        currentState = MonsterState.walk;
    }
    void OnDrawGizmos()//畫圖
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
    void detectEnemy()
    {
        if (currentState != MonsterState.idle)
        {
            Collider2D[] collider2D = Physics2D.OverlapCircleAll(this.transform.position, detectRange);
            foreach (var k in collider2D)
            {
                if (k.gameObject != this.gameObject || !k.CompareTag(this.tag))
                {
                    if (enemy == null)
                    {
                        enemy = k.gameObject;
                    }
                    else if (k.gameObject == enemy)
                    {
                        // float dis = Vector3.Distance(this.transform.position, enemy.GetComponent<Collider2D>().bounds.center);
                        float dis = Vector3.Distance(this.transform.position, enemy.transform.position);
                        Debug.Log(dis);
                        if (dis <= attackRange)
                        {
                            currentState = MonsterState.attack;
                        }
                        else if (dis <= detectRange)
                        {
                            currentState = MonsterState.track;
                        }
                    }
                }
            }
        }
    }
    void monsterState()//怪物動作
    {
        if (nextAttack > 0)
        {
            nextAttack -= Time.deltaTime;
        }
        if (enemy != null && Vector3.Distance(enemy.transform.position, transform.position) > detectRange)
        {
            enemy = null;
            currentState = MonsterState.walk;
        }
        if (currentState == MonsterState.idle)
        {
            animator.SetBool("moving", false);
        }
        else if (currentState == MonsterState.attack && nextAttack <= 0)
        {
            StartCoroutine(attack());
        }
        else if ((currentState == MonsterState.track && !isFix) || currentState == MonsterState.pigeon)
        {
            Vector2 direction = enemy.GetComponent<Collider2D>().bounds.center - transform.position;
            direction = direction.normalized;
            transform.position = Vector3.MoveTowards(transform.position, enemy.transform.position, Time.deltaTime * speed); //往敵人方向移動
            animator.SetFloat("moveX", direction.x);
            animator.SetFloat("moveY", direction.y);
            animator.SetBool("moving", true);
        }
        else if (currentState == MonsterState.walk || currentState == MonsterState.pigeon)
        {
            transform.position += new Vector3(0, speed, 0) * Time.deltaTime * walkDir_Y; //持續往前進
            animator.SetFloat("moveX", 0);
            animator.SetFloat("moveY", walkDir_Y);
            animator.SetBool("moving", true);
        }
    }
    bool fixPosition()
    {
        if (currentState == MonsterState.walk && currentState == MonsterState.track)
        {
            float x = animator.GetFloat("moveX");
            float y = animator.GetFloat("moveY");
            Vector2 enemyPos = new Vector2(x, y);
            Vector3 line = Vector2.zero - enemyPos;
            float rotate = Mathf.Atan2(line.y, line.x) * Mathf.Rad2Deg;
            Quaternion k = Quaternion.AngleAxis(angle, Vector3.forward);
            Vector3 Left = k * enemyPos;
            k = Quaternion.AngleAxis(-angle, Vector3.forward);
            Vector3 Right = k * enemyPos;

            raycast2D[0] = Physics2D.Raycast(transform.position, Left, dis, mask);
            raycast2D[1] = Physics2D.Raycast(transform.position, enemyPos, dis, mask);
            raycast2D[2] = Physics2D.Raycast(transform.position, Right, dis, mask);
            Debug.DrawRay(transform.position, enemyPos * dis, Color.black);
            Debug.DrawRay(transform.position, Left * dis, Color.black);
            Debug.DrawRay(transform.position, Right * dis, Color.black);

            foreach (RaycastHit2D i in raycast2D)
            {
                if ((i.collider != null) && (i.collider.tag == this.gameObject.tag))
                {
                    float posL = Vector3.Distance(i.collider.transform.position, transform.position + Left);
                    float posR = Vector3.Distance(i.collider.transform.position, transform.position + Right);
                    if (posL > posR)
                    {
                        transform.position += Left * speed * Time.deltaTime;
                    }
                    else
                    {
                        transform.position += Right * speed * Time.deltaTime;
                    }
                    return isFix = true;
                }
            }
        }
        return isFix = false;
    }
    private IEnumerator attack()
    {
        Vector2 dir = enemy.transform.position - transform.position;
        dir = dir.normalized;
        animator.SetBool("moving", false);
        animator.SetFloat("moveX", Mathf.RoundToInt(dir.x));
        animator.SetFloat("moveY", Mathf.RoundToInt(dir.y));
        nextAttack = attackRate;
        animator.SetBool("attacking", true);
        currentState = MonsterState.attack;
        yield return null;
        animator.SetBool("attacking", false);
    }
    public IEnumerator PigeonChange(float t)
    {
        currentState = MonsterState.pigeon;
        animator.SetBool("moving", false);
        animator.SetBool("pigeon", true);
        yield return new WaitForSeconds(t);
        animator.SetBool("pigeon", false);
    }
}
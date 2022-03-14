using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum ArenaState
{
    idle,
    walk,
    love,
}
public class arenaPlayer : MonoBehaviour
{
    [SerializeField] private float loveTime = 2f;
    private float nextTime;
    public int love_index;
    public ArenaState currentState;
    [Header("玩家基礎數值")]
    public float speed = 30f;
    [SerializeField] private float curH;
    public bool red;
    public int p_index;
    public Vector3 spawnPos;
    private SpriteRenderer sprite;
    private Rigidbody2D mrigibody;
    private Animator mAnimator;
    private GameObject ui;
    private PlayerInput controls;
    private Vector2 movement;
    void Awake()
    {
        controls = this.GetComponent<PlayerInput>();
        controls.actions["choose"].performed += choose;
        // controls.actionEvents[1].AddListener(choose);
    }
    void Start()
    {
        mrigibody = this.GetComponent<Rigidbody2D>();
        mAnimator = this.GetComponent<Animator>();
        sprite = this.GetComponent<SpriteRenderer>();
        nextTime = loveTime;
        curH = 3f;
    }
    //玩家狀態更新
    void FixedUpdate()
    {
        if (currentState == ArenaState.love)
        {
            if (love_index >= FightManager.Instance.gamelist.Count)
            {
                love_index = FightManager.Instance.gamelist.Count - 1;
                if (love_index == p_index && p_index != 0)
                {
                    love_index = love_index - 1;
                }
            }
            Vector3 pos = FightManager.Instance.gamelist[love_index].transform.position;
            mrigibody.AddForce((pos - this.transform.position).normalized * speed * 0.9f, ForceMode2D.Force);
            loveTime -= Time.deltaTime;
            if (loveTime < 0)
            {
                loveTime = nextTime;
                this.transform.Find("NumTitle").GetChild(p_index).GetComponent<SpriteRenderer>().color = Color.white;
                currentState = ArenaState.walk;
            }
        }
        else if (currentState == ArenaState.walk)
        {
            if (movement != Vector2.zero)
            {
                mrigibody.AddForce(movement * speed, ForceMode2D.Force);
            }
        }
    }
    //選隊
    public void choose(InputAction.CallbackContext ctx)
    {
        if (ui.transform.Find("RedTeam").gameObject.activeSelf)
        {
            ui.transform.Find("RedTeam").gameObject.SetActive(false);
            ui.transform.Find("BlueTeam").gameObject.SetActive(true);
        }
        else
        {
            ui.transform.Find("RedTeam").gameObject.SetActive(true);
            ui.transform.Find("BlueTeam").gameObject.SetActive(false);
        }

    }
    //移除inputSystem設定
    public void removeChoose()
    {
        controls.actions["choose"].performed -= choose;
        // controls.actionEvents[1].RemoveListener(choose);
    }
    //搖桿旋轉玩家
    public void rotate(InputAction.CallbackContext ctx)
    {
        Vector2 input = ctx.ReadValue<Vector2>();
        if (input != Vector2.zero)
        {
            float rotate = Mathf.Atan2(input.y, input.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(rotate - 90, Vector3.forward);
        }
    }
    //滑鼠旋轉玩家
    public void mou(InputAction.CallbackContext ctx)
    {
        Vector3 pos = ctx.ReadValue<Vector2>();
        pos = Camera.main.ScreenToWorldPoint(pos);
        Vector2 line = pos - this.transform.position;
        float rotate = Mathf.Atan2(line.y, line.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(rotate - 90, Vector3.forward);
    }
    //移動玩家
    public void Move(InputAction.CallbackContext ctx)
    {
        movement = ctx.ReadValue<Vector2>();
    }
    //開啟Title
    public void openP_Num(int num)
    {
        p_index = num;
        this.transform.Find("NumTitle").GetChild(p_index).gameObject.SetActive(true);
        ui = GameObject.Find("ChoosePlayer").transform.Find("P" + (p_index + 1)).gameObject;
        if (ui.transform.Find("BlueTeam").gameObject.activeSelf)
        {
            ui.transform.Find("RedTeam").gameObject.SetActive(true);
            ui.transform.Find("BlueTeam").gameObject.SetActive(false);
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
    //改變顏色
    public void changeColor()
    {
        StartCoroutine(changeColorTitle_Sword());
    }
    //改變Title顏色
    private IEnumerator changeColorTitle_Sword()
    {
        this.transform.Find("NumTitle").GetChild(p_index).GetComponent<SpriteRenderer>().color = new Color32(34, 179, 229, 255);
        yield return new WaitForSeconds(transform.Find("sword").GetComponent<sword>().gaveTime);
        this.transform.Find("NumTitle").GetChild(p_index).GetComponent<SpriteRenderer>().color = Color.white;
    }
}

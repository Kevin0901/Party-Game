using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum ArenaState
{
    idle,
    walk,
    dizzy,
    stone
}
public class arenaPlayer : MonoBehaviour
{
    [HideInInspector] public float dizzyT;
    public bool red;
    public bool isready;
    public ArenaState currentState;
    [Header("玩家數值")]
    public float speed = 30f;
    [SerializeField] private float curH;
    public int p_index;
    private SpriteRenderer spriteRen;
    private Vector3 spawnPos;
    private Rigidbody2D mrigibody;
    [Header("石頭")]
    [SerializeField] private Sprite stone;
    public int CupidGamepoint;
    private GameObject ui;

    private PlayerInput controls;
    private Vector2 movement;
    void Awake()
    {
        controls = this.GetComponent<PlayerInput>();
        controls.actions["choose"].performed += choose;
        // controls.actionEvents[1].AddListener(choose);
    }
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
    public void removeChoose()
    {
        controls.actions["choose"].performed -= choose;
        // controls.actionEvents[1].RemoveListener(choose);
    }
    public void rotate(InputAction.CallbackContext ctx)
    {
        Vector2 input = ctx.ReadValue<Vector2>();
        if (input != Vector2.zero)
        {
            float rotate = Mathf.Atan2(input.y, input.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(rotate - 90, Vector3.forward);
        }
    }
    public void mou(InputAction.CallbackContext ctx)
    {
        Vector3 pos = ctx.ReadValue<Vector2>();
        pos = Camera.main.ScreenToWorldPoint(pos);
        Vector2 line = pos - this.transform.position;
        float rotate = Mathf.Atan2(line.y, line.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(rotate - 90, Vector3.forward);
    }
    public void Move(InputAction.CallbackContext ctx)
    {
        movement = ctx.ReadValue<Vector2>();
    }
    void Start()
    {
        CupidGamepoint = 0;
        curH = 3f;
        isready = false;
        mrigibody = this.GetComponent<Rigidbody2D>();
        spriteRen = this.GetComponent<SpriteRenderer>();
        dizzyT = 0.15f;
        spawnPos = this.transform.position;
    }
    void FixedUpdate()
    {
        if (currentState == ArenaState.dizzy)
        {
            dizzyT -= Time.deltaTime;
            spriteRen.color = new Color32(200, 3, 180, 255);
            if (dizzyT < 0)
            {
                spriteRen.color = new Color32(255, 255, 255, 255);
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
        else if (currentState == ArenaState.stone)
        {
            spriteRen.sprite = stone;
        }
    }
    public void openP_Num(int num)
    {
        p_index = num;
        this.transform.Find("NumTitle").GetChild(p_index).gameObject.SetActive(true);
        ui = GameObject.Find("ChoosePlayer").transform.Find("P" + p_index).gameObject;
        if (ui.transform.Find("BlueTeam").gameObject.activeSelf)
        {
            ui.transform.Find("RedTeam").gameObject.SetActive(true);
            ui.transform.Find("BlueTeam").gameObject.SetActive(false);
        }
    }
    public void SpawnPoint(Vector3 pos)
    {
        this.transform.position = pos;
        spawnPos = pos;
    }
    public void SpawnPoint()
    {
        this.transform.position = spawnPos;
    }
    private void OnDisable()
    {
        FightManager.Instance.gamelist.Remove(this.gameObject);
    }
    public void hurt(float damege)
    {
        curH -= damege;
        GameObject.Find("HealthUI").transform.Find("P" + p_index).GetComponent<heart>().hurt(curH);
        if (curH <= 0)
        {
            this.gameObject.SetActive(false);
        }
    }
}

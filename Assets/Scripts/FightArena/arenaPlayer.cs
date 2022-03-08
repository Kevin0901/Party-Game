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
    private Vector3 change;
    [Header("玩家數值")]
    public float speed = 30f;
    private SpriteRenderer spriteRen;
    private Vector3 spawnPos;
    private Rigidbody2D mrigibody;
    // private float rotate;
    private Vector3 worldPosition, mousePos, startPos, line;
    [Header("石頭")]
    [SerializeField] private Sprite stone;
    public int CupidGamepoint;
    private GameObject ui;
    PlayerInput controls;
    void Awake()
    {
        controls = this.GetComponent<PlayerInput>();
        controls.actionEvents[1].AddListener(choose);
        ui = GameObject.Find("ChoosePlayer").transform.Find("P" + (controls.playerIndex + 1)).gameObject;
        if (ui.transform.Find("BlueTeam").gameObject.activeSelf)
        {
            ui.transform.Find("RedTeam").gameObject.SetActive(true);
            ui.transform.Find("BlueTeam").gameObject.SetActive(false);
        }
    }
    public void choose(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
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
    }
    public void removeChoose()
    {
        controls.actionEvents[1].RemoveListener(choose);
    }
    public void rotatePlayer(InputAction.CallbackContext ctx)
    {
        Vector2 input = ctx.ReadValue<Vector2>();
        Debug.Log(input);
        if (input != Vector2.zero)
        {
            float rotate = Mathf.Atan2(input.y, input.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(rotate - 90, Vector3.forward);
        }
    }
    void Start()
    {
        CupidGamepoint = 0;
        isready = false;
        mrigibody = this.GetComponent<Rigidbody2D>();
        spriteRen = this.GetComponent<SpriteRenderer>();
        dizzyT = 0.15f;
        change.z = 0;
        startPos = Vector3.zero;
        // OpenHealth();
        spawnPos = this.transform.position;
    }
    // Update is called once per frame
    private void Update()
    {
        // if (joynum == "0")
        // {
        //     mousePos = Input.mousePosition;
        //     worldPosition = Camera.main.ScreenToWorldPoint(mousePos);
        //     line = worldPosition - this.transform.position;
        //     line.z = 0;
        //     rotate = Mathf.Atan2(line.y, line.x) * Mathf.Rad2Deg;
        //     transform.rotation = Quaternion.AngleAxis(rotate - 90, Vector3.forward);
        // }
        // else
        // {
        //     change.x = Input.GetAxis("X-virtualMouse" + joynum);
        //     change.y = Input.GetAxis("Y-virtualMouse" + joynum);
        //     if (change != Vector3.zero)
        //     {
        //         line = change - startPos;
        //         line.z = 0;
        //         rotate = Mathf.Atan2(line.y, line.x) * Mathf.Rad2Deg;
        //         transform.rotation = Quaternion.AngleAxis(rotate - 90, Vector3.forward);
        //     }
        // }
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
            Move();
        }
        else if (currentState == ArenaState.stone)
        {
            spriteRen.sprite = stone;
        }
    }
    void Move()
    {
        // change = Vector3.zero;
        // change.x = Mathf.RoundToInt(Input.GetAxisRaw("Xplayer" + joynum));
        // change.y = Mathf.RoundToInt(Input.GetAxisRaw("Yplayer" + joynum));
        // if (change != Vector3.zero)
        // {
        //     mrigibody.AddForce(change.normalized * speed, ForceMode2D.Force);
        // }
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

    public void hurt(float damege)
    {
        // GameObject.Find("HealthManager").transform.Find("P" + order).GetComponent<heart>().hurt(damege);
    }
    // void OpenHealth()
    // {
    //     GameObject.Find("HealthManager").transform.Find("P" + order).gameObject.SetActive(true);
    //     GameObject.Find("HealthManager").transform.Find("P" + order).GetComponent<heart>().SetPlayer(this.gameObject);
    // }
}

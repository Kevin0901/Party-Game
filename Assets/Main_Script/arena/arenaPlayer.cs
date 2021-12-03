using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ArenaState
{
    idle,
    walk,
    dizzy,
    repel,
    stone
}
public class arenaPlayer : MonoBehaviour
{
    [HideInInspector] public float dizzyT, repelpower;
    [HideInInspector] public Vector3 repeldir;
    public bool isready;
    public ArenaState currentState;
    private Vector3 change;
    [Header("控制器控制")]
    public string joynum;//控制器
    [Header("玩家登入排序")]
    public int order;//P1 P2
    [Header("玩家數值")]
    public float speed = 30f;
    private SpriteRenderer spriteRen;
    private Vector3 spawnPos;
    private Rigidbody2D mrigibody;
    private float rotate, health;
    private Vector3 worldPosition, mousePos, startPos, line;
    [Header("石頭")]
    [SerializeField] private Sprite stone;
    public int CupidGamepoint;

    void Start()
    {
        CupidGamepoint = 0;
        isready = false;
        mrigibody = this.GetComponent<Rigidbody2D>();
        spriteRen = this.GetComponent<SpriteRenderer>();
        health = 3;
        dizzyT = 0.15f;
        change.z = 0;
        startPos = Vector3.zero;
        OpenHealth();
        spawnPos = this.transform.position;
    }
    // Update is called once per frame
    private void Update()
    {
        if (!isready)
        {
            ReadyGame();
        }
        if (joynum == "0")
        {
            mousePos = Input.mousePosition;
            worldPosition = Camera.main.ScreenToWorldPoint(mousePos);
            line = worldPosition - this.transform.position;
            line.z = 0;
            rotate = Mathf.Atan2(line.y, line.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(rotate - 90, Vector3.forward);
        }
        else
        {
            change.x = Input.GetAxis("X-virtualMouse" + joynum);
            change.y = Input.GetAxis("Y-virtualMouse" + joynum);
            if (change != Vector3.zero)
            {
                line = change - startPos;
                line.z = 0;
                rotate = Mathf.Atan2(line.y, line.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.AngleAxis(rotate - 90, Vector3.forward);
            }
        }
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
        else if (currentState == ArenaState.repel)
        {
            mrigibody.AddForce(repeldir.normalized * repelpower, ForceMode2D.Impulse);
            currentState = ArenaState.walk;
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
        change = Vector3.zero;
        change.x = Mathf.RoundToInt(Input.GetAxisRaw("Xplayer" + joynum));
        change.y = Mathf.RoundToInt(Input.GetAxisRaw("Yplayer" + joynum));
        if (change != Vector3.zero)
        {
            mrigibody.AddForce(change.normalized * speed, ForceMode2D.Force);
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
    private void ReadyGame()
    {
        if (joynum == "0")
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isready = true;
            }
        }
        else if (joynum == "1")
        {
            if (Input.GetKeyDown(KeyCode.Joystick1Button2))
            {
                isready = true;
            }
        }
        else if (joynum == "2")
        {
            if (Input.GetKeyDown(KeyCode.Joystick2Button2))
            {
                isready = true;
            }
        }
        else if (joynum == "3")
        {
            if (Input.GetKeyDown(KeyCode.Joystick3Button2))
            {
                isready = true;
            }
        }
        else if (joynum == "4")
        {
            if (Input.GetKeyDown(KeyCode.Joystick4Button2))
            {
                isready = true;
            }
        }
    }
    public void hurt(float damege)
    {
        GameObject.Find("HealthManager").transform.Find("P" + order).GetComponent<heart>().hurt(damege);
    }

    void OpenHealth()
    {
        GameObject.Find("HealthManager").transform.Find("P" + order).gameObject.SetActive(true);
        GameObject.Find("HealthManager").transform.Find("P" + order).GetComponent<heart>().SetPlayer(this.gameObject);
    }
}

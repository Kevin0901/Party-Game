using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.IO;
public enum ArenaState
{
    idle,
    walk,
    love,
    lighting,
    shoot,
    fastMode,
    punch,
    ares
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
    public bool isCenter; //玩家是否在正中心
    [Header("邱比特設定")]
    [SerializeField] private float loveTime = 2f;
    private float nextlove;
    public int love_index;
    [Header("宙斯")]
    [SerializeField] private GameObject lighting;
    [SerializeField] private float maxPowerTime;
    private float powerTime;
    public bool isPress;
    [Header("荷米斯")]
    public float turboSpeed;
    [Header("足球")]
    [SerializeField] private float fistpower;
    [Header("戰神")]
    [SerializeField] private GameObject spear;

    private Rigidbody2D mrigibody;
    private Animator mAnimator;
    private float nextfire;
    [HideInInspector] public SpriteRenderer titleColor;
    private heart lifeUI;
    private Vector2 movement;
    PhotonView PV;
    void Start()
    {
        PV = GetComponent<PhotonView>();
        mrigibody = this.GetComponent<Rigidbody2D>();
        mAnimator = this.GetComponent<Animator>();
        nextlove = loveTime;
        nextfire = 0;
        curH = 3f;
        isCenter = false;

        FightManager FM = FightManager.Instance;
        FM.plist.Add(this.gameObject);
        p_index = FM.plist.Count - 1;
        red = FM.redOrBlue[p_index];
        setUI();
    }
    //偵測按鍵輸入
    private void Update()
    {
        if (!PV.IsMine)
        {
            return;
        }
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
        if (!PV.IsMine)
        {
            return;
        }
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
    private void LateUpdate()
    {
        if (!PV.IsMine)
        {
            return;
        }
        mouseRotate();
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
    public IEnumerator ChangeARES(float time)
    {
        GameObject s = null;
        if (PV.IsMine)
        {
            mAnimator.SetBool("change",true);
        }
        yield return new WaitForSeconds(1f);
        mAnimator.GetComponent<arenaPlayer>().currentState = ArenaState.ares;
        speed *= 1.2f;
        if (PV.IsMine)
        {
            s = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "arena/Ares/rotateSpear"), this.transform.position, this.transform.rotation);
            s.GetComponent<spear>().player = this.gameObject;
        }
        yield return new WaitForSeconds(time);
        speed /= 1.2f;
        mAnimator.GetComponent<arenaPlayer>().currentState = ArenaState.walk;
        // mAnimator.GetComponent<Animator>().SetTrigger("change");
        if (PV.IsMine)
        {
            mAnimator.SetBool("change",false);
            PhotonNetwork.Destroy(s);
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
            GameObject arr = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "arena/SunMoon/Arrow"), transform.position, transform.rotation);
            PV.RPC("RPC_ShootArrow", RpcTarget.All, isCenter, arr.GetComponent<PhotonView>().ViewID, PV.ViewID);
            nextfire = Time.time + fireRate; //下次發射的時間
        }
    }
    [PunRPC]
    void RPC_ShootArrow(bool center, int PVID, int ShooterID)
    {
        GameObject Arrow = PhotonView.Find(PVID).gameObject;
        Arrow.GetComponent<SunMoonArrowMove>().shooter = PhotonView.Find(ShooterID).gameObject;
        if (center) //如果在正中心發射的話
        {
            Arrow.GetComponent<SpriteRenderer>().color = new Color32(0, 0, 0, 255);
            Arrow.GetComponent<SunMoonArrowMove>().speed = Arrowspeed * 1.5f;
            Arrow.GetComponent<SunMoonArrowMove>().power = power * 1.25f;
            Arrow.transform.localScale *= 1.25f;
        }
        else
        {
            Arrow.GetComponent<SunMoonArrowMove>().speed = Arrowspeed;
            Arrow.GetComponent<SunMoonArrowMove>().power = power;
        }
        Arrow.GetComponent<SunMoonArrowMove>().setArrow();
    }
    //閃電發射
    private void ShootLight()
    {
        if (Input.GetMouseButton(0))
        {
            isPress = true;
            mrigibody.velocity = Vector2.zero;
            powerTime += Time.deltaTime;
            if (powerTime < maxPowerTime)
                titleColor.color = new Color(1, 1 - (0.5f / maxPowerTime) * powerTime, 0);
            else
            {
                titleColor.color = Color.red;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isPress = false;
            float damage;
            GameObject a = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "arena/Zeus/lightShoot"), transform.position,
            lighting.transform.rotation * this.transform.rotation);
            if (powerTime < maxPowerTime)
            {
                a.transform.localScale += new Vector3(a.transform.localScale.x * powerTime, 0, 0);
                a.GetComponent<shootflash>().damege = 0.5f;
                damage = 0.5f;
            }
            else
            {
                a.transform.localScale += new Vector3(a.transform.localScale.x * 10, 0, 0);
                a.GetComponent<shootflash>().damege = 1.5f;
                damage = 1.5f;

            }
            PV.RPC("RPC_lightShoot", RpcTarget.All, a.GetComponent<PhotonView>().ViewID, damage);
            a.GetComponent<shootflash>().shooter = this.gameObject;
            titleColor.color = Color.white;
            currentState = ArenaState.walk;
            powerTime = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            mrigibody.AddForce(movement * speed * 0.85f, ForceMode2D.Impulse);
            currentState = ArenaState.walk;
        }
    }
    [PunRPC]
    void RPC_lightShoot(int lightID, float dam)
    {
        PhotonView.Find(lightID).gameObject.GetComponent<shootflash>().damege = dam;
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
        if (!PV.IsMine)
        {
            return;
        }
        curH -= damege;
        lifeUI.hurt(curH);
        Debug.Log(curH);
        PV.RPC("RPC_ArenaPlayer_Hurt", RpcTarget.All, damege, PV.Owner.NickName);
        if (curH <= 0)
        {
            this.gameObject.SetActive(false);
        }
        mAnimator.SetTrigger("hurt");
    }
    [PunRPC]
    void RPC_ArenaPlayer_Hurt(float dam, string who)
    {
        if (PV.IsMine)
        {
            return;
        }
        if (PV.Owner.NickName.Equals(who))
        {
            curH -= dam;
            lifeUI.hurt(curH);
            if (curH <= 0)
            {
                this.gameObject.SetActive(false);
            }
            mAnimator.SetTrigger("hurt");
        }
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
        yield return new WaitForSeconds(transform.Find("sword").GetComponent<sword>().waitTime);
        titleColor.color = Color.white;
    }
}

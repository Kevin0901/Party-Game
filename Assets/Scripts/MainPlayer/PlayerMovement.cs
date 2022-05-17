using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using System.IO;
public enum PlayerState
{
    walk,
    attack,
    idle,
}

public class PlayerMovement : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [Header("玩家參數")]
    [SerializeField] private PlayerState currentState;
    public float speed;
    [SerializeField] private int MaxHealth, CurHealth;
    [SerializeField] private float dir;
    public int attackDamage;
    [SerializeField] private float attackRate; //攻擊間隔
    [SerializeField] private float fireRate;
    [Header("紅隊復活點")]
    [SerializeField] private Vector3 RedspawnPoint;
    [Header("藍隊復活點")]
    [SerializeField] private Vector3 BluespawnPoint;
    private Animator animator;
    private Rigidbody2D mrigibody;
    private Vector3 change;
    public health health;
    [Header("控制器控制")]
    public string joynum;//控制器
    [Header("玩家登入排序")]
    public int order;//P1 P2
    [Header("玩家攝影機")]
    public Camera playercamera;
    [Header("虛擬滑鼠")]
    public GameObject mouse;
    [Header("計時器")]
    [SerializeField] private GameObject timer;
    [Header("物品欄")]
    public int phfeather = 0;
    public int phfeatheruse = 0;
    [SerializeField] private UI_Inventory uiInventory;
    [SerializeField] private GameObject inventorysign;
    public Inventory inventory;
    [SerializeField] private GameObject[] generateUI;
    [SerializeField] private GameObject[] towerlist;
    [SerializeField] private GameObject[] monsterlist;
    [HideInInspector] public int intower;
    [HideInInspector] public int interritory;
    [SerializeField] private GameObject[] canthrowitem;
    [Header("總玩家數")]
    public int allplayercount;
    // public GameObject deadscreen;
    private UIState UI;
    private int getallinv = 0;
    private static int spriteNum = 0;
    public float orginspeed;
    private float nextfire;
    public int ProjectileCost = 1;
    public PhotonView PV;
    public MultiPlayerManager MultiPlayerManager;
    public GameObject PAPA;
    private void Awake()
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        health = this.GetComponentInChildren<health>();
        animator = this.GetComponent<Animator>();
        health.maxH = MaxHealth;
        orginspeed = speed;
        PV = GetComponent<PhotonView>();  //定義PhotonView
        MultiPlayerManager = PhotonView.Find((int)PV.InstantiationData[0]).GetComponent<MultiPlayerManager>();  //設定自己的 PlayerManager
        MultiPlayerManager.OherPlayer = this.gameObject;  //設定 PlayerManager 中的 OherPlayer
    }
    void Start()
    {
        PAPA = GameObject.Find("PAPA");
        this.transform.parent.SetParent(PAPA.transform);
        mouse = this.transform.parent.Find("mouseUI").gameObject;
        nextfire = 0;
        mrigibody = this.GetComponent<Rigidbody2D>();
        UiandInventoryGet();
        StartCoroutine(Wait_Game_Start());
        if (!PV.IsMine)  //如果此玩家 GameObject 是別人的鏡像，消除 UI 以及 Camera
        {
            mouse.SetActive(false);
            timer.SetActive(false);
            playercamera.gameObject.SetActive(false);

            return;
        }
    }
    IEnumerator Wait_Game_Start()
    {
        if (GameObject.Find("RoomManager").GetComponent<RoomManager>().ReadyTime <= 0)
        {
            currentState = PlayerState.walk;
        }
        else
        {
            currentState = PlayerState.idle;
            yield return new WaitForSeconds(0.1f);
            StartCoroutine(Wait_Game_Start());
        }
    }
    private void OnEnable()
    {
        StartCoroutine(WudiSet(2.5f));
        // if (deadscreen != null)
        // {
        //     deadscreen.SetActive(false);
        // }
        animator.enabled = true;
        animator.SetFloat("attackSpeed", 1 / attackRate);
        animator.SetFloat("moveY", dir);
        currentState = PlayerState.walk;
        this.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
        speed = orginspeed;
    }
    private void OnDisable()
    {
        // if (deadscreen != null)
        // {
        //     deadscreen.SetActive(true);
        // }
    }
    IEnumerator WudiSet(float t)
    {
        health.iswudi = true;
        yield return new WaitForSeconds(t);
        health.iswudi = false;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        CurHealth = health.curH;
        if (CurHealth <= 0)
        {
            animator.enabled = false;
            transform.Find("down hitbox").gameObject.SetActive(false);
            transform.Find("right hitbox").gameObject.SetActive(false);
            transform.Find("up hitbox").gameObject.SetActive(false);
            transform.Find("left hitbox").gameObject.SetActive(false);
            // this.transform.SetParent(GameObject.Find("PAPA").transform);
            Invoke("spawn", 2);
            if (PV.IsMine)
            {
                transform.parent.Find("DeadScreen").gameObject.SetActive(true);
            }
            this.gameObject.SetActive(false);
        }
        if (!PV.IsMine)
        {
            return;
        }
        PlayerMove();
        Playerthrow();
        PlayerItemUse();
        PlayerBulid();
        if (Input.GetKeyDown(KeyCode.J))
        {
            // StartCoroutine(this.GetComponent<Effect>().RandGiveEffect());
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            // inventory.AddItem(new Item { itemType = Item.ItemType.Medusaeye, amount = 10 });

        }
    }
    public void spawn()
    {
        health.curH = health.maxH;
        if (this.tag == "red")
        {
            this.transform.position = RedspawnPoint;
        }
        else
        {
            this.transform.position = BluespawnPoint;
        }
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name == "MainScene")
        {
            transform.parent.Find("DeadScreen").gameObject.SetActive(false);
            this.gameObject.SetActive(true);
            // this.transform.SetParent(null);
        }
    }
    void PlayerMove()
    {
        if (Input.GetButton("attack" + joynum) && currentState != PlayerState.attack) //如果按下攻擊鍵且玩家不在攻擊動作
        {
            // if (this.gameObject.activeSelf)
            // {
            StartCoroutine(AttackCo());
            // }
        }
        else if (currentState == PlayerState.walk) //如果玩家動作在走路
        {
            UpdateAnimationAndMove();
        }
        foreach (Item checkph in inventory.GetItemList())   //背包有沒有羽毛
        {
            if (checkph.itemType.ToString() == "Phoneixfeather")
            {
                phfeather = 1;
            }
        }
        if (phfeatheruse == 1)
        {
            inventory.RemoveItem(new Item { itemType = Item.ItemType.Phoneixfeather, amount = 1 });
            phfeatheruse = 0;
        }
    }
    void Playerthrow()
    {
        if (getallinv == 1)
        {
            for (int i = 0; i < canthrowitem.Length; i++)
            {
                if (inventorysign.GetComponent<InventorySign>().signitemname() == canthrowitem[i].name)
                {
                    int shoottimes = inventorysign.GetComponent<InventorySign>().signitemamount();
                    if (int.Parse(joynum) != 0)
                    {
                        if ((Input.GetAxisRaw("R2-" + joynum) > 0) && Time.time > nextfire)
                        {
                            if (shoottimes > 0)
                            {
                                GameObject throwI = Instantiate(canthrowitem[i], transform.position, canthrowitem[i].transform.rotation);
                                throwI.transform.SetParent(this.transform);
                                nextfire = Time.time + fireRate;
                                switch (i)
                                {
                                    case 0:
                                        inventory.RemoveItem(new Item { itemType = Item.ItemType.Firehell, amount = 1 });
                                        break;
                                    case 1:
                                        inventory.RemoveItem(new Item { itemType = Item.ItemType.TransPotion, amount = 1 });
                                        break;
                                    case 2:
                                        inventory.RemoveItem(new Item { itemType = Item.ItemType.Medusaeye, amount = 1 });
                                        break;
                                }

                            }
                        }
                    }
                    else
                    {
                        if (Input.GetMouseButton(0) && Time.time > nextfire)
                        {
                            if (shoottimes > 0)
                            {
                                GameObject throwI = Instantiate(canthrowitem[i], transform.position, canthrowitem[i].transform.rotation);
                                throwI.transform.SetParent(this.transform);
                                nextfire = Time.time + fireRate;
                                switch (i)
                                {
                                    case 0:
                                        inventory.RemoveItem(new Item { itemType = Item.ItemType.Firehell, amount = 1 });
                                        break;
                                    case 1:
                                        inventory.RemoveItem(new Item { itemType = Item.ItemType.TransPotion, amount = 1 });
                                        break;
                                    case 2:
                                        inventory.RemoveItem(new Item { itemType = Item.ItemType.Medusaeye, amount = 1 });
                                        break;
                                }

                            }
                        }
                    }
                }
            }
        }
    }
    void PlayerBulid()
    {
        if (int.Parse(joynum) != 0)
        {
            if (Input.GetAxisRaw("L2-" + joynum) > 0)
            {
                if (UI.b != 0 && interritory == 1 && intower == 0)
                {
                    if (Time.time > nextfire && this.tag == "red")
                    {
                        if (ResourceManager.Instance.RedCanAfford(towerlist[UI.bx].GetComponent<TowerData>().CostArray) != false)
                        {
                            ResourceManager.Instance.RedSpendResources(towerlist[UI.bx].GetComponent<TowerData>().CostArray);
                            towerlist[UI.bx].gameObject.tag = this.tag;
                            Instantiate(towerlist[UI.bx], this.transform.position, Quaternion.identity);
                            nextfire = Time.time + fireRate;
                        }
                        else
                        {
                            StartCoroutine(ResuorceNotEnoughShow());
                        }
                    }
                    else if (Time.time > nextfire && this.tag == "blue")
                    {
                        if (ResourceManager.Instance.BlueCanAfford(towerlist[UI.bx].GetComponent<TowerData>().CostArray) != false)
                        {
                            ResourceManager.Instance.BlueSpendResources(towerlist[UI.bx].GetComponent<TowerData>().CostArray);
                            towerlist[UI.bx].gameObject.tag = this.tag;
                            Instantiate(towerlist[UI.bx], this.transform.position, Quaternion.identity);
                            nextfire = Time.time + fireRate;
                        }
                        else
                        {
                            StartCoroutine(ResuorceNotEnoughShow());
                        }
                    }
                }
                else if (UI.m != 0 && interritory == 1)
                {
                    if (Time.time > nextfire && this.tag == "red")
                    {
                        if (ResourceManager.Instance.RedCanAfford(monsterlist[UI.mx].GetComponent<monsterMove>().CostArray) != false)
                        {
                            ResourceManager.Instance.RedSpendResources(monsterlist[UI.mx].GetComponent<monsterMove>().CostArray);
                            monsterlist[UI.mx].gameObject.tag = this.tag;
                            GameObject monster = Instantiate(monsterlist[UI.mx], this.transform.position, Quaternion.identity);
                            monster.transform.position += new Vector3(0, 2, 0);
                            nextfire = Time.time + fireRate;
                        }
                        else
                        {
                            StartCoroutine(ResuorceNotEnoughShow());
                        }
                    }
                    else if (Time.time > nextfire && this.tag == "blue")
                    {
                        if (ResourceManager.Instance.BlueCanAfford(monsterlist[UI.mx].GetComponent<monsterMove>().CostArray) != false)
                        {
                            ResourceManager.Instance.BlueSpendResources(monsterlist[UI.mx].GetComponent<monsterMove>().CostArray);
                            monsterlist[UI.mx].gameObject.tag = this.tag;
                            GameObject monster = Instantiate(monsterlist[UI.mx], this.transform.position, Quaternion.identity);
                            monster.transform.position += new Vector3(0, -2, 0);
                            nextfire = Time.time + fireRate;
                        }
                        else
                        {
                            StartCoroutine(ResuorceNotEnoughShow());
                        }
                    }
                }
                else
                {
                    StartCoroutine(Cantbuildshow());
                }
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                if (UI.b != 0 && interritory == 1 && intower == 0)
                {
                    if (Time.time > nextfire && this.tag == "red")
                    {
                        if (ResourceManager.Instance.RedCanAfford(towerlist[UI.bx].GetComponent<TowerData>().CostArray) != false)
                        {
                            ResourceManager.Instance.RedSpendResources(towerlist[UI.bx].GetComponent<TowerData>().CostArray);
                            towerlist[UI.bx].gameObject.tag = this.tag;
                            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Main-tower/" + towerlist[UI.bx].gameObject.name), this.transform.position, Quaternion.identity, 0, new object[] { PV.ViewID });
                            // Instantiate(towerlist[UI.bx], this.transform.position, Quaternion.identity);
                            nextfire = Time.time + fireRate;
                        }
                        else
                        {
                            StartCoroutine(ResuorceNotEnoughShow());
                        }
                    }
                    else if (Time.time > nextfire && this.tag == "blue")
                    {
                        if (ResourceManager.Instance.BlueCanAfford(towerlist[UI.bx].GetComponent<TowerData>().CostArray) != false)
                        {
                            ResourceManager.Instance.BlueSpendResources(towerlist[UI.bx].GetComponent<TowerData>().CostArray);
                            towerlist[UI.bx].gameObject.tag = this.tag;
                            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Main-tower/" + towerlist[UI.bx].gameObject.name), this.transform.position, Quaternion.identity, 0, new object[] { PV.ViewID });
                            // Instantiate(towerlist[UI.bx], this.transform.position, Quaternion.identity);
                            nextfire = Time.time + fireRate;
                        }
                        else
                        {
                            StartCoroutine(ResuorceNotEnoughShow());
                        }
                    }
                }
                else if (UI.m != 0 && interritory == 1)
                {
                    if (Time.time > nextfire && this.tag == "red")
                    {
                        if (ResourceManager.Instance.RedCanAfford(monsterlist[UI.mx].GetComponent<monsterMove>().CostArray) != false)
                        {
                            ResourceManager.Instance.RedSpendResources(monsterlist[UI.mx].GetComponent<monsterMove>().CostArray);
                            monsterlist[UI.mx].gameObject.tag = this.tag;
                            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Monster/" + monsterlist[UI.mx].gameObject.name),
                            this.transform.position + new Vector3(0, 2, 0), Quaternion.identity,
                            0, new object[] { PV.ViewID });
                            // GameObject monster = Instantiate(monsterlist[UI.mx], this.transform.position, Quaternion.identity);
                            // monster.transform.position += new Vector3(0, 2, 0);
                            nextfire = Time.time + fireRate;
                        }
                        else
                        {
                            StartCoroutine(ResuorceNotEnoughShow());
                        }
                    }
                    else if (Time.time > nextfire && this.tag == "blue")
                    {
                        if (ResourceManager.Instance.BlueCanAfford(monsterlist[UI.mx].GetComponent<monsterMove>().CostArray) != false)
                        {
                            ResourceManager.Instance.BlueSpendResources(monsterlist[UI.mx].GetComponent<monsterMove>().CostArray);
                            monsterlist[UI.mx].gameObject.tag = this.tag;
                            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Monster/" + monsterlist[UI.mx].gameObject.name),
                            this.transform.position + new Vector3(0, -2, 0), Quaternion.identity,
                            0, new object[] { PV.ViewID });
                            // GameObject monster = Instantiate(monsterlist[UI.mx], this.transform.position, Quaternion.identity);
                            // monster.transform.position += new Vector3(0, -2, 0);
                            nextfire = Time.time + fireRate;
                        }
                        else
                        {
                            StartCoroutine(ResuorceNotEnoughShow());
                        }
                    }
                }
                else
                {
                    StartCoroutine(Cantbuildshow());
                }
            }
        }

    }
    void PlayerItemUse()
    {
        if (int.Parse(joynum) == 0 && Input.GetKeyDown(KeyCode.Q) && Time.time > nextfire)
        {
            nextfire = Time.time + fireRate;
            switch (inventorysign.GetComponent<InventorySign>().signitemname())
            {
                case "Hermisboots":
                    if (inventorysign.GetComponent<InventorySign>().signitemamount() != 0)
                    {
                        inventory.RemoveItem(new Item { itemType = Item.ItemType.Hermisboots, amount = 1 });
                        StartCoroutine(this.GetComponent<Effect>().SpeedUpEffect());

                    }
                    break;
                case "Wine":
                    if (inventorysign.GetComponent<InventorySign>().signitemamount() != 0)
                    {
                        inventory.RemoveItem(new Item { itemType = Item.ItemType.Wine, amount = 1 });
                        StartCoroutine(this.GetComponent<Effect>().PotionHealEffect());

                    }
                    break;
                case "PowerPotion":
                    if (inventorysign.GetComponent<InventorySign>().signitemamount() != 0)
                    {
                        inventory.RemoveItem(new Item { itemType = Item.ItemType.PowerPotion, amount = 1 });
                        StartCoroutine(this.GetComponent<Effect>().PowerUPEffect());
                    }
                    break;
            }
        }
        else if (int.Parse(joynum) != 0 && Input.GetAxisRaw("R2-" + joynum) > 0 && Time.time > nextfire)
        {
            nextfire = Time.time + fireRate;
            switch (inventorysign.GetComponent<InventorySign>().signitemname())
            {
                case "Hermisboots":
                    if (inventorysign.GetComponent<InventorySign>().signitemamount() != 0)
                    {
                        inventory.RemoveItem(new Item { itemType = Item.ItemType.Hermisboots, amount = 1 });
                        StartCoroutine(this.GetComponent<Effect>().SpeedUpEffect());
                    }
                    break;
                case "Wine":
                    if (inventorysign.GetComponent<InventorySign>().signitemamount() != 0)
                    {
                        inventory.RemoveItem(new Item { itemType = Item.ItemType.Wine, amount = 1 });
                        StartCoroutine(this.GetComponent<Effect>().PotionHealEffect());
                    }
                    break;
                case "PowerPotion":
                    if (inventorysign.GetComponent<InventorySign>().signitemamount() != 0)
                    {
                        inventory.RemoveItem(new Item { itemType = Item.ItemType.PowerPotion, amount = 1 });
                        StartCoroutine(this.GetComponent<Effect>().PowerUPEffect());
                    }
                    break;
            }
        }
    }

    private IEnumerator Cantbuildshow()
    {
        this.GetComponent<UIState>().NoticeUI.transform.GetChild(1).gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        this.GetComponent<UIState>().NoticeUI.transform.GetChild(1).gameObject.SetActive(false);
    }

    private IEnumerator ResuorceNotEnoughShow()
    {
        this.GetComponent<UIState>().NoticeUI.transform.GetChild(0).gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        this.GetComponent<UIState>().NoticeUI.transform.GetChild(0).gameObject.SetActive(false);
    }

    private IEnumerator AttackCo()
    {
        currentState = PlayerState.attack;
        animator.SetBool("attacking", true);
        yield return null;
        animator.SetBool("attacking", false);
        yield return new WaitForSeconds(attackRate);
        currentState = PlayerState.walk;
    }
    public GameObject getplayersign()
    {
        return inventorysign;
    }
    void UiandInventoryGet()
    {
        UI = this.GetComponent<UIState>();
        if (!PV.IsMine)
        {
            UI.enabled = false;
            for (int i = 1; i < 10; i++)
            {
                this.gameObject.transform.parent.GetChild(i).gameObject.SetActive(false);
            }
        }
        else
        {
            bornSet();
        }
        inventory = new Inventory();
        uiInventory = UI.GetInventory();
        uiInventory.SetInventory(inventory);
        inventorysign = UI.GetInventorySign();
        inventorysign.GetComponent<InventorySign>().inventory = this.inventory;
        UI.GetGameobject();
        getallinv = 1;
    }
    void UpdateAnimationAndMove()
    {
        change = Vector3.zero;
        change.x = Mathf.RoundToInt(Input.GetAxisRaw("Xplayer" + joynum));
        change.y = Mathf.RoundToInt(Input.GetAxisRaw("Yplayer" + joynum));
        if (change != Vector3.zero)
        {
            mrigibody.MovePosition(transform.position + change.normalized * speed * Time.deltaTime);
            animator.SetFloat("moveX", change.x);
            animator.SetFloat("moveY", change.y);
            animator.SetBool("moving", true);
        }
        else
        {
            animator.SetBool("moving", false);
        }
    }
    public void bornSet()
    {
        // GameObject time = Instantiate(timer, timer.transform.position, timer.transform.rotation);
        timer.transform.Find("Num").Find("P" + order).gameObject.SetActive(true);
        // Canvas timecanvas = time.GetComponent<Canvas>();
        // timecanvas.renderMode = RenderMode.ScreenSpaceCamera;
        // timecanvas.worldCamera = this.transform.GetChild(0).gameObject.GetComponent<Camera>();
        // timecanvas.sortingLayerName = "UI";
        // timecanvas.sortingOrder = 1;
        spriteNum++;
        if (this.gameObject.tag == "red")
        {
            this.transform.position = RedspawnPoint;
            spriteRenderer.sortingOrder = spriteNum;
            dir = 1f;
        }
        else if ((this.gameObject.tag == "blue"))
        {
            this.transform.position = BluespawnPoint;
            spriteRenderer.sortingOrder = spriteNum;
            dir = -1f;
        }
        // GameObject mouse = Instantiate(v, v.transform.position, v.transform.rotation);
        if (int.Parse(joynum) == 0)
        {
            mouse.AddComponent<mouseMove>();
            mouse.GetComponent<mouseMove>().totalplayer = allplayercount;
        }
        else
        {
            mouse.AddComponent<VirtualmouseMove>();
            mouse.GetComponent<VirtualmouseMove>().num = joynum;
            mouse.GetComponent<VirtualmouseMove>().totalplayer = allplayercount;
        }
        // Canvas canvas = mouse.GetComponent<Canvas>();
        // canvas.renderMode = RenderMode.ScreenSpaceCamera;
        // canvas.worldCamera = this.transform.GetChild(0).gameObject.GetComponent<Camera>();
        // canvas.sortingLayerName = "UI";
        // canvas.sortingOrder = 2;
        // mouse.transform.SetParent(GameObject.Find("UIManager").transform.Find("Mouse").transform);
    }
}

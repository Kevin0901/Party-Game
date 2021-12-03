using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public enum PlayerState
{
    walk,
    attack,
}

public class PlayerMovement : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    [Header("玩家參數")]
    public PlayerState currentState;
    public float speed;
    public int attackDamage;
    [SerializeField] private int MaxHealth, CurHealth;
    [SerializeField] private float dir;

    [Header("每幾秒攻擊一次")]
    [SerializeField] private float attackRate; //攻擊間隔

    [Header("玩家復活點")]
    [SerializeField] private Vector3 RedspawnPoint, BluespawnPoint;
    private Animator animator;
    private Rigidbody2D mrigibody;
    private Vector3 change;
    private health health;
    [Header("控制器控制")]
    public string joynum;//控制器
    [Header("玩家登入排序")]
    public int order;//P1 P2
    [Header("虛擬滑鼠")]
    [SerializeField] private GameObject v;
    [Header("計時器")]
    [SerializeField] private GameObject timer;
    [Header("物品欄")]
    public int phfeather = 0;
    public int phfeatheruse = 0;
    [SerializeField] private UI_Inventory uiInventory;
    [SerializeField] private GameObject inventorysign;
    public Inventory inventory;
    [SerializeField] private GameObject[] generateUI;
    public Camera playercamera;
    [SerializeField] private GameObject[] towerlist;
    [SerializeField] private GameObject[] monsterlist;
    public int intower;
    public int interritory;
    [Header("總玩家數")]
    public int allplayercount;
    private float fireRate, nextfire;
    public GameObject deadscreen;
    private UIState UI;
    [SerializeField] private GameObject[] canthrowitem;
    private int getallinv = 0;
    private Animation anim;
    private static int spriteNum;
    private float orginspeed;

    // Start is called before the first frame update
    private void Awake()
    {
        health = this.GetComponentInChildren<health>();
        animator = this.GetComponent<Animator>();
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        health.maxH = MaxHealth;
        orginspeed = speed;
    }
    void Start()
    {
        spriteNum = 0;
        fireRate = 0.45f;
        nextfire = 0;
        mrigibody = this.GetComponent<Rigidbody2D>();
        animator.SetFloat("attackSpeed", 1 / attackRate);
        currentState = PlayerState.walk;
        playercamera = this.transform.GetChild(0).gameObject.GetComponent<Camera>();
        spawnUI();
        animator.SetFloat("moveY", dir);
    }
    private void OnEnable()
    {
        health.iswudi = true;
        StartCoroutine(WudiSet());
        if (deadscreen != null)
        {
            deadscreen.SetActive(false);
        }
        animator.enabled = true;
        animator.SetFloat("attackSpeed", 1 / attackRate);
        currentState = PlayerState.walk;
        animator.SetFloat("moveY", dir);
        this.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
        speed = orginspeed;
    }
    private void OnDisable()
    {
        if (deadscreen != null)
        {
            deadscreen.SetActive(true);
        }
    }

    IEnumerator WudiSet()
    {
        yield return new WaitForSeconds(2f);
        health.iswudi = false;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        CurHealth = health.curH;
        if (CurHealth <= 0)
        {
            animator.enabled = false;
            transform.Find("down hitnox").gameObject.SetActive(false);
            transform.Find("right hitnox").gameObject.SetActive(false);
            transform.Find("up hitnox").gameObject.SetActive(false);
            transform.Find("left hitnox").gameObject.SetActive(false);
            this.transform.SetParent(GameObject.Find("PAPA").transform);
            Invoke("spawn", 2);
            this.gameObject.SetActive(false);
        }
        PlayerEffect();
        Playerthrow();
        PlayerItemUse();
        PlayerBulid();

        if (Input.GetKeyDown(KeyCode.E))
        {
            inventory.AddItem(new Item { itemType = Item.ItemType.Medusaeye, amount = 1 });
        }

    }
    private IEnumerator HermisEffect()
    {
        inventory.RemoveItem(new Item { itemType = Item.ItemType.Hermisboots, amount = 1 });
        speed = orginspeed * 2;
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(1);
        }
        speed = orginspeed;
    }

    private IEnumerator PowerEffect()
    {
        inventory.RemoveItem(new Item { itemType = Item.ItemType.PowerPotion, amount = 1 });
        attackDamage *= 2;
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(1);
        }
        attackDamage /= 2;
    }
    private IEnumerator WineEffect()
    {
        inventory.RemoveItem(new Item { itemType = Item.ItemType.Wine, amount = 1 });
        speed /= 2;
        for (int i = 0; i < 6; i++)
        {
            int reheal = (int)((health.maxH * 0.4f) / 6);
            if (health.curH < health.maxH)
            {
                if (health.curH + reheal > health.maxH)
                {
                    health.curH = health.maxH;
                }
                else
                {
                    health.curH += reheal;
                }
            }
            yield return new WaitForSeconds(0.5f);
        }
        speed *= 2;
    }
    public IEnumerator StoneEffect()
    {
        StartCoroutine(StoneEffect(orginspeed));
        yield return null;
    }
    public IEnumerator StoneEffect(float speeds)
    {
        speed = 0;
        this.GetComponent<SpriteRenderer>().color = new Color32(89, 89, 89, 255);
        yield return new WaitForSeconds(5);
        this.GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
        speed = speeds;
    }
    void PlayerEffect()
    {
        if (Input.GetButton("attack" + joynum) && currentState != PlayerState.attack) //如果按下攻擊鍵且玩家不在攻擊動作
        {
            if (this.gameObject.activeSelf)
            {
                StartCoroutine(AttackCo());
            }
        }
        else if (currentState == PlayerState.walk) //如果玩家動作在走路
        {
            UpdateAnimationAndMove();
        }

        foreach (Item checkph in inventory.GetItemList())
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
            if (inventorysign.GetComponent<InventorySign>().signitemname() == canthrowitem[0].name)
            {
                int shoottimes = inventorysign.GetComponent<InventorySign>().signitemamount();
                if (Input.GetAxisRaw("R2-" + joynum) > 0 && Time.time > nextfire)
                {
                    if (shoottimes > 0)
                    {
                        GameObject throwI = Instantiate(canthrowitem[0], transform.position, canthrowitem[0].transform.rotation);
                        throwI.transform.SetParent(this.transform);
                        throwI.AddComponent<item_MoveMovementGamepad>();
                        nextfire = Time.time + fireRate;
                        inventory.RemoveItem(new Item { itemType = Item.ItemType.Firehell, amount = 1 });
                    }
                }
            }
            else if (inventorysign.GetComponent<InventorySign>().signitemname() == canthrowitem[1].name)
            {
                int shoottimes = inventorysign.GetComponent<InventorySign>().signitemamount();
                if (Input.GetAxisRaw("R2-" + joynum) > 0 && Time.time > nextfire)
                {
                    if (shoottimes > 0)
                    {
                        GameObject throwI = Instantiate(canthrowitem[1], transform.position, canthrowitem[1].transform.rotation);
                        throwI.transform.SetParent(this.transform);
                        throwI.AddComponent<item_MoveMovementGamepad>();
                        nextfire = Time.time + fireRate;
                        inventory.RemoveItem(new Item { itemType = Item.ItemType.TransPotion, amount = 1 });
                    }
                }
            }
            else if (inventorysign.GetComponent<InventorySign>().signitemname() == canthrowitem[2].name)
            {
                int shoottimes = inventorysign.GetComponent<InventorySign>().signitemamount();
                if (Input.GetAxisRaw("R2-" + joynum) > 0 && Time.time > nextfire)
                {
                    if (shoottimes > 0)
                    {
                        GameObject throwI = Instantiate(canthrowitem[2], transform.position, canthrowitem[2].transform.rotation);
                        throwI.transform.SetParent(this.transform);
                        throwI.AddComponent<item_MoveMovementGamepad>();
                        nextfire = Time.time + fireRate;
                        inventory.RemoveItem(new Item { itemType = Item.ItemType.Medusaeye, amount = 1 });
                    }
                }
            }
        }
    }
    void PlayerBulid()
    {
        if (int.Parse(joynum) != 0 && Input.GetAxisRaw("L2-" + joynum) > 0)
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
    void PlayerItemUse()
    {
        if (int.Parse(joynum) == 0)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                switch (inventorysign.GetComponent<InventorySign>().signitemname())
                {
                    case "Hermisboots":
                        if (inventorysign.GetComponent<InventorySign>().signitemamount() != 0)
                        {
                            StartCoroutine(HermisEffect());
                        }
                        break;
                    case "Wine":
                        if (inventorysign.GetComponent<InventorySign>().signitemamount() != 0)
                        {
                            StartCoroutine(WineEffect());
                        }
                        break;
                    case "PowerPotion":
                        if (inventorysign.GetComponent<InventorySign>().signitemamount() != 0)
                        {
                            StartCoroutine(PowerEffect());
                        }
                        break;
                }
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
                        StartCoroutine(HermisEffect());
                    }
                    break;
                case "Wine":
                    if (inventorysign.GetComponent<InventorySign>().signitemamount() != 0)
                    {
                        StartCoroutine(WineEffect());
                    }
                    break;
                case "PowerPotion":
                    if (inventorysign.GetComponent<InventorySign>().signitemamount() != 0)
                    {
                        StartCoroutine(PowerEffect());
                    }
                    break;
            }
        }
    }

    private IEnumerator Cantbuildshow()
    {
        GameObject.Find("P" + order + "UI").transform.GetChild(5).transform.GetChild(1).gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        GameObject.Find("P" + order + "UI").transform.GetChild(5).transform.GetChild(1).gameObject.SetActive(false);
    }

    private IEnumerator ResuorceNotEnoughShow()
    {
        GameObject.Find("P" + order + "UI").transform.GetChild(5).transform.GetChild(0).gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        GameObject.Find("P" + order + "UI").transform.GetChild(5).transform.GetChild(0).gameObject.SetActive(false);
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

    void spawnUI()
    {
        switch (order)
        {
            case 1:
                for (int i = 0; i < generateUI.Length; i++)
                {
                    GameObject resource = Instantiate(generateUI[i]);
                    resource.transform.parent = GameObject.Find("P" + order + "UI").gameObject.transform;
                    Canvas canvas = resource.GetComponent<Canvas>();
                    canvas.renderMode = RenderMode.ScreenSpaceCamera;
                    canvas.worldCamera = this.transform.GetChild(0).gameObject.GetComponent<Camera>();
                    canvas.sortingLayerName = "UI";
                    canvas.sortingOrder = 1;
                }
                UiandInventoryGet();
                break;
            case 2:
                for (int i = 0; i < generateUI.Length; i++)
                {
                    GameObject resource = Instantiate(generateUI[i]);
                    resource.transform.parent = GameObject.Find("P" + order + "UI").gameObject.transform;
                    Canvas canvas = resource.GetComponent<Canvas>();
                    canvas.renderMode = RenderMode.ScreenSpaceCamera;
                    canvas.worldCamera = this.transform.GetChild(0).gameObject.GetComponent<Camera>();
                    canvas.sortingLayerName = "UI";
                    canvas.sortingOrder = 1;
                }
                UiandInventoryGet();
                break;
            case 3:
                for (int i = 0; i < generateUI.Length; i++)
                {
                    GameObject resource = Instantiate(generateUI[i]);
                    resource.transform.parent = GameObject.Find("P" + order + "UI").gameObject.transform;
                    Canvas canvas = resource.GetComponent<Canvas>();
                    canvas.renderMode = RenderMode.ScreenSpaceCamera;
                    canvas.worldCamera = this.transform.GetChild(0).gameObject.GetComponent<Camera>();
                    canvas.sortingLayerName = "UI";
                    canvas.sortingOrder = 1;
                }
                UiandInventoryGet();
                break;
            case 4:
                for (int i = 0; i < generateUI.Length; i++)
                {
                    GameObject resource = Instantiate(generateUI[i]);
                    resource.transform.parent = GameObject.Find("P" + order + "UI").gameObject.transform;
                    Canvas canvas = resource.GetComponent<Canvas>();
                    canvas.renderMode = RenderMode.ScreenSpaceCamera;
                    canvas.worldCamera = this.transform.GetChild(0).gameObject.GetComponent<Camera>();
                    canvas.sortingLayerName = "UI";
                    canvas.sortingOrder = 1;
                }
                UiandInventoryGet();
                break;
        }

    }
    void UiandInventoryGet()
    {
        UI = GameObject.Find("P" + order + "UI").GetComponent<UIState>();
        UI.enabled = true;
        UI.player = this.gameObject;
        UI.playercamera = this.transform.GetChild(0).gameObject.GetComponent<Camera>();
        UI.GetGameobject();
        inventory = new Inventory();
        uiInventory = UI.GetInventory();
        uiInventory.SetInventory(inventory);
        inventorysign = UI.GetInventorySign();
        inventorysign.GetComponent<InventorySign>().inventory = this.inventory;
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
    private void spawn()
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
            this.gameObject.SetActive(true);
            this.transform.SetParent(null);
        }

    }
    public void bornSet()
    {
        GameObject time = Instantiate(timer, timer.transform.position, timer.transform.rotation);
        time.transform.Find("Num").transform.Find("P" + order).gameObject.SetActive(true);
        Canvas timecanvas = time.GetComponent<Canvas>();
        timecanvas.renderMode = RenderMode.ScreenSpaceCamera;
        timecanvas.worldCamera = this.transform.GetChild(0).gameObject.GetComponent<Camera>();
        timecanvas.sortingLayerName = "UI";
        timecanvas.sortingOrder = 1;
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

        GameObject mouse = Instantiate(v, v.transform.position, v.transform.rotation);
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
        Canvas canvas = mouse.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = this.transform.GetChild(0).gameObject.GetComponent<Camera>();
        canvas.sortingLayerName = "UI";
        canvas.sortingOrder = 2;
        mouse.transform.SetParent(GameObject.Find("UIManager").transform.Find("Mouse").transform);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIState : MonoBehaviour
{
    public static UIState Instance;
    [SerializeField] private GameObject ResourceUI;
    [SerializeField] private GameObject BuildUI;
    [SerializeField] private GameObject MonsterUI;
    [SerializeField] private GameObject InventoryUI;
    // [SerializeField] private GameObject gamemanager;

    public GameObject NoticeUI;
    public int bcount;
    public int mcount;


    public GameObject player;
    public Camera playercamera;
    public int getall = 0;
    private string joynum;
    public int b = 0;
    public int m = 0;
    public static int escc = 0;
    public int bx = 0;
    public int mx = 0;
    private RectTransform selectbpos;
    private RectTransform selectmpos;
    private Vector2 orginbpos;
    private Vector2 orginmpos;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        MonsterUI.SetActive(false);
        BuildUI.SetActive(false);
    }
    void Update()
    {
        if (getall == 1)
        {
            if (int.Parse(player.GetComponent<PlayerMovement>().joynum) == 0)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    if (b == 0)
                    {
                        MonsterUI.SetActive(false);
                        BuildUI.SetActive(true);
                        // hover.SetActive(false);
                        // Hover.mhover = 0;
                        m = 0;
                        b = 1;
                    }
                    else
                    {
                        BuildUI.SetActive(false);
                        // hover.SetActive(false);
                        // Hover.bhover = 0;
                        b = 0;
                        escc = 0;
                    }
                }
                if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    if (m == 0)
                    {
                        MonsterUI.SetActive(true);
                        BuildUI.SetActive(false);
                        // hover.SetActive(false);
                        // Hover.bhover = 0;
                        m = 1;
                        b = 0;
                    }
                    else
                    {
                        MonsterUI.SetActive(false);
                        // hover.SetActive(false);
                        // Hover.mhover = 0;
                        m = 0;
                        escc = 0;
                    }

                }

                if (Input.GetKeyDown(KeyCode.Z))
                {
                    if (b == 1)
                    {
                        bx++;
                        if (bx < 5)
                        {
                            selectbpos.anchoredPosition = new Vector2(orginbpos.x + (bx * 300), orginbpos.y);
                        }
                        else
                        {
                            selectbpos.anchoredPosition = orginbpos;
                            bx = 0;
                        }

                    }
                    else if (m == 1)
                    {
                        mx++;
                        if (mx < 7)
                        {
                            selectmpos.anchoredPosition = new Vector2(orginmpos.x + (mx * 225), orginmpos.y);
                        }
                        else
                        {
                            selectmpos.anchoredPosition = orginmpos;
                            mx = 0;
                        }
                    }
                }
                else if (Input.GetKeyDown(KeyCode.C))
                {
                    if (b == 1)
                    {
                        bx--;
                        if (bx < 5 && bx >= 0)
                        {
                            selectbpos.anchoredPosition = new Vector2(orginbpos.x + (bx * 300), orginbpos.y);
                        }
                        else
                        {
                            bx = 4;
                            selectbpos.anchoredPosition = new Vector2(orginbpos.x + (bx * 300), orginbpos.y);
                        }
                    }
                    else if (m == 1)
                    {
                        mx--;
                        if (mx < 7 && mx >= 0)
                        {
                            selectmpos.anchoredPosition = new Vector2(orginmpos.x + (mx * 225), orginmpos.y);
                        }
                        else
                        {
                            mx = 6;
                            selectmpos.anchoredPosition = new Vector2(orginmpos.x + (mx * 225), orginmpos.y);
                        }
                    }
                }

            }
            else
            {
                joynum = player.GetComponent<PlayerMovement>().joynum;
                if (Input.GetAxisRaw("AxisLeftRight" + joynum) > 0)
                {
                    if (bcount == 0)
                    {
                        if (b == 0)
                        {
                            MonsterUI.SetActive(false);
                            BuildUI.SetActive(true);
                            BuildUI.transform.Find("SelectSlot").gameObject.SetActive(true);
                            // hover.SetActive(false);
                            // Hover.mhover = 0;
                            m = 0;
                            b = 1;
                        }
                        else
                        {
                            BuildUI.SetActive(false);
                            // hover.SetActive(false);
                            // Hover.bhover = 0;
                            b = 0;
                            escc = 0;
                        }
                        bcount = 1;
                    }
                }
                if (Input.GetAxisRaw("AxisLeftRight" + joynum) == 0)
                {
                    bcount = 0;
                }
                if (Input.GetAxisRaw("AxisLeftRight" + joynum) < 0)
                {
                    if (mcount == 0)
                    {
                        if (m == 0)
                        {
                            MonsterUI.SetActive(true);
                            MonsterUI.transform.Find("SelectSlot").gameObject.SetActive(true);
                            BuildUI.SetActive(false);
                            // hover.SetActive(false);
                            // Hover.bhover = 0;
                            b = 0;
                            m = 1;
                        }
                        else
                        {
                            MonsterUI.SetActive(false);
                            // hover.SetActive(false);
                            // Hover.mhover = 0;
                            m = 0;
                            escc = 0;
                        }
                        mcount = 1;
                    }
                }
                if (mcount == 1 && Input.GetAxisRaw("AxisLeftRight" + joynum) == 0)
                {
                    mcount = 0;
                }

                if (Input.GetButtonDown("rightchoose" + joynum))
                {
                    if (b == 1)
                    {
                        bx++;
                        if (bx < 5)
                        {
                            selectbpos.anchoredPosition = new Vector2(orginbpos.x + (bx * 300), orginbpos.y);
                        }
                        else
                        {
                            selectbpos.anchoredPosition = orginbpos;
                            bx = 0;
                        }

                    }
                    else if (m == 1)
                    {
                        mx++;
                        if (mx < 7)
                        {
                            selectmpos.anchoredPosition = new Vector2(orginmpos.x + (mx * 225), orginmpos.y);
                        }
                        else
                        {
                            selectmpos.anchoredPosition = orginmpos;
                            mx = 0;
                        }
                    }
                }
                else if (Input.GetButtonDown("leftchoose" + joynum))
                {
                    if (b == 1)
                    {
                        bx--;
                        if (bx < 5 && bx >= 0)
                        {
                            selectbpos.anchoredPosition = new Vector2(orginbpos.x + (bx * 300), orginbpos.y);
                        }
                        else
                        {
                            bx = 4;
                            selectbpos.anchoredPosition = new Vector2(orginbpos.x + (bx * 300), orginbpos.y);
                        }
                    }
                    else if (m == 1)
                    {
                        mx--;
                        if (mx < 7 && mx >= 0)
                        {
                            selectmpos.anchoredPosition = new Vector2(orginmpos.x + (mx * 225), orginmpos.y);
                        }
                        else
                        {
                            mx = 6;
                            selectmpos.anchoredPosition = new Vector2(orginmpos.x + (mx * 225), orginmpos.y);
                        }
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (escc == 0)
                {
                    // hover.SetActive(false);
                    GameManager.a = 0;
                    escc = 1;
                }
                else if (escc == 1)
                {
                    BuildUI.SetActive(false);
                    MonsterUI.SetActive(false);
                    escc = 0;
                }
            }
        }

    }

    public void GetGameobject()
    {
        // hover = this.transform.GetChild(0).gameObject;
        // ResourceUI = this.transform.GetChild(1).gameObject;
        // BuildUI = this.transform.GetChild(2).gameObject;
        // MonsterUI = this.transform.GetChild(3).gameObject;
        // InventoryUI = this.transform.GetChild(4).gameObject;
        // NoticeUI = this.transform.GetChild(5).gameObject;
        // Instantiate(gamemanager).transform.parent = this.transform;
        // gamemanager.GetComponent<GameManager>().hover = hover;
        BuildUI.SetActive(false);
        MonsterUI.SetActive(false);
        // hover.GetComponent<Hover>().thiscamera = playercamera;
        // this.tag = player.tag;
        ResourceUI.GetComponentInChildren<ResourceUI>().Playertag = this.tag;
        InventoryUI.transform.Find("UI_Inventory").transform.GetChild(2).GetComponent<InventorySign>().player = player;
        getall = 1;
        selectbpos = BuildUI.transform.Find("SelectSlot").gameObject.GetComponent<RectTransform>();
        orginbpos = selectbpos.anchoredPosition;
        selectmpos = MonsterUI.transform.Find("SelectSlot").gameObject.GetComponent<RectTransform>();
        orginmpos = selectmpos.anchoredPosition;
    }

    public UI_Inventory GetInventory()
    {
        return InventoryUI.transform.GetChild(0).GetComponent<UI_Inventory>();
    }

    public GameObject GetInventorySign()
    {
        return InventoryUI.transform.Find("UI_Inventory").transform.GetChild(2).gameObject;
    }
}

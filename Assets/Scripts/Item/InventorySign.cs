using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class InventorySign : MonoBehaviour
{
    public event EventHandler OnItemListChanged;
    public Inventory inventory;
    public GameObject player;
    private List<Item> nowitememlist;
    private RectTransform signpos;
    private Vector2 orginpos;
    private Vector2 finallpos;
    private int y = 0;
    private int typecount = 0;
    private string joynum;
    private int hitcheck = 0;
    void Start()
    {
        this.GetComponent<Image>().enabled = false;
        signpos = this.GetComponent<RectTransform>();
        orginpos = signpos.anchoredPosition;
        finallpos = new Vector2(orginpos.x, orginpos.y + (-2 * 85f));
    }

    void Update()
    {
        float itemSlotCellSize = 85f;
        if (checkinventory())
        {
            this.GetComponent<Image>().enabled = true;
        }
        if (UIState.Instance.getall == 1)
        {
            joynum = player.GetComponent<PlayerMovement>().joynum;
            if (int.Parse(joynum) == 0 && Input.GetKeyDown(KeyCode.Tab))
            {
                typecount = 0;
                nowtypecount();
                y++;
                if (y < typecount)
                {
                    signpos.anchoredPosition = new Vector2(orginpos.x, orginpos.y + (-y * itemSlotCellSize));
                }
                else
                {
                    signpos.anchoredPosition = orginpos;
                    y = 0;
                }
            }

            if (int.Parse(joynum) != 0)
            {
                if (Input.GetAxisRaw("AxisUpDown" + joynum) < 0)
                {
                    if (hitcheck == 0)
                    {
                        typecount = 0;
                        nowtypecount();
                        y++;
                        if (y < typecount)
                        {
                            signpos.anchoredPosition = new Vector2(orginpos.x, orginpos.y + (-y * itemSlotCellSize));
                        }
                        else
                        {
                            signpos.anchoredPosition = orginpos;
                            y = 0;
                        }
                        hitcheck = 1;
                    }
                }
                if (Input.GetAxisRaw("AxisUpDown" + joynum) > 0)
                {
                    if (hitcheck == 0)
                    {
                        typecount = 0;
                        nowtypecount();
                        y--;
                        if (y == 0)
                        {
                            signpos.anchoredPosition = new Vector2(orginpos.x, orginpos.y + (y * itemSlotCellSize));
                        }
                        else if (y == 1)
                        {
                            signpos.anchoredPosition = new Vector2(orginpos.x, finallpos.y + (y * itemSlotCellSize));
                        }
                        else
                        {
                            signpos.anchoredPosition = finallpos;
                            y = 2;
                        }
                        hitcheck = 1;
                    }
                }
                if (Input.GetAxisRaw("AxisUpDown" + joynum) == 0)
                {
                    hitcheck = 0;
                }
            }

        }

        if (signitemamount() <= 0)
        {
            signpos.anchoredPosition = orginpos;
            y = 0;
            OnItemListChanged?.Invoke(this, EventArgs.Empty);
        }
        if (signitemamount() <= 0 && y == 0)
        {
            this.GetComponent<Image>().enabled = false;
            OnItemListChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    private bool checkinventory()
    {
        if (inventory.GetItemList() != null)
        {
            foreach (Item item in inventory.GetItemList())
            {
                if (item.amount != 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
        else
        {
            return false;
        }

    }

    public string signitemname()
    {
        int n = 0;
        foreach (Item item in inventory.GetItemList())
        {
            if (inventory.GetItemList() != null)
            {
                if (y == n)
                {
                    return item.itemType.ToString();
                }
                n++;
            }
        }
        return null;
    }

    public int signitemamount()
    {
        int n = 0;
        foreach (Item item in inventory.GetItemList())
        {
            if (y == n)
            {
                return item.amount;
            }
            n++;
        }
        return 0;
    }

    private int nowtypecount()
    {
        foreach (Item item in inventory.GetItemList())
        {
            if (item.amount != 0)
            {
                typecount++;
            }
        }
        return typecount;
    }
}

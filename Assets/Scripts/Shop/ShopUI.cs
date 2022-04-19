using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    public GameObject commodity;
    private GameObject canvas, shopui;
    private int playerin = 0;
    private string playertag;
    private string joynum;
    private Item getitem;
    private Inventory inventory;
    private string[] iteminlist = new string[] { "Wine", "TransPotion", "Firehell", "Hermisboots", "Phoneixfeather" };
    private ItemData itemData;
    public static int nowitemamount;

    void Start()
    {
        itemData = commodity.GetComponent<ItemData>();
        this.transform.Find("ShopItemImage").GetComponent<SpriteRenderer>().sprite = commodity.GetComponent<SpriteRenderer>().sprite;
        canvas = this.transform.Find("Canvas").gameObject;
        shopui = canvas.transform.Find("ItemBuyUI").gameObject;
        shopui.transform.Find("CostShowText").GetComponent<ShopCostShow>().costarray = itemData.CostArray;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerin == 1)
        {
            if (int.Parse(joynum) == 0 && Input.GetKeyDown(KeyCode.E))
            {
                GetItemAssets();
                GetItemAmount();
                if (playertag == "red")
                {
                    if (ResourceManager.Instance.RedCanAfford(itemData.CostArray) != false && InventoryCanAdd())
                    {
                        if (nowitemamount < itemData.MaxAmount)
                        {
                            ResourceManager.Instance.RedSpendResources(itemData.CostArray);
                            inventory.AddItem(getitem);
                        }
                        else
                        {
                            Debug.Log("can't buy 1");
                        }
                    }
                    else
                    {
                        Debug.Log("can't buy 2");
                    }
                }
                else if (playertag == "blue")
                {
                    if (ResourceManager.Instance.BlueCanAfford(itemData.CostArray) != false && InventoryCanAdd())
                    {
                        if (nowitemamount < itemData.MaxAmount)
                        {
                            ResourceManager.Instance.BlueSpendResources(itemData.CostArray);
                            inventory.AddItem(getitem);
                        }
                        else
                        {
                            Debug.Log("can't buy 1");
                        }
                    }
                    else
                    {
                        Debug.Log("can't buy 2");
                    }
                }

            }
            else if (int.Parse(joynum) != 0 && Input.GetButtonDown("AxisCircle" + joynum))
            {
                GetItemAssets();
                GetItemAmount();
                if (playertag == "red")
                {
                    if (ResourceManager.Instance.RedCanAfford(itemData.CostArray) != false && InventoryCanAdd())
                    {
                        if (nowitemamount < itemData.MaxAmount)
                        {
                            ResourceManager.Instance.RedSpendResources(itemData.CostArray);
                            inventory.AddItem(getitem);
                        }
                        else
                        {
                            Debug.Log("can't buy 1");
                        }
                    }
                    else
                    {
                        Debug.Log("can't buy 2");
                    }
                }
                else if (playertag == "blue")
                {
                    if (ResourceManager.Instance.BlueCanAfford(itemData.CostArray) != false && InventoryCanAdd())
                    {
                        if (nowitemamount < itemData.MaxAmount)
                        {
                            ResourceManager.Instance.BlueSpendResources(itemData.CostArray);
                            inventory.AddItem(getitem);
                        }
                        else
                        {
                            Debug.Log("can't buy 1");
                        }
                    }
                    else
                    {
                        Debug.Log("can't buy 2");
                    }
                }
            }
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == 10)
        {
            inventory = other.GetComponent<PlayerMovement>().inventory;
            playerin = 1;
            playertag = other.gameObject.tag;
            joynum = other.gameObject.GetComponent<PlayerMovement>().joynum;
            canvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceCamera;
            canvas.GetComponent<Canvas>().worldCamera = other.gameObject.GetComponent<PlayerMovement>().playercamera;
            canvas.GetComponent<Canvas>().sortingLayerName = "UI";
            canvas.GetComponent<Canvas>().sortingOrder = 1;
            shopui.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == 10)
        {
            playerin = 0;
            playertag = null;
            shopui.SetActive(false);
        }
    }

    private bool InventoryCanAdd()
    {
        int itemcount = 0;
        if (inventory.GetItemList() != null)
        {
            foreach (Item item in inventory.GetItemList())
            {
                itemcount++;
                if (getitem.itemType == item.itemType && getitem.IsStackable())
                {
                    return true;
                }
                if (itemcount >= 3)
                {
                    return false;
                }
            }
            return true;
        }
        else
        {
            return true;
        }

    }

    void GetItemAmount()
    {
        nowitemamount = 0;
        foreach (Item item in inventory.GetItemList())
        {
            if (getitem.itemType == item.itemType)
            {
                nowitemamount = item.amount;
            }
        }
    }


    void GetItemAssets()
    {
        for (int i = 0; i < iteminlist.Length; i++)
        {
            if (commodity.name == iteminlist[i])
            {
                switch (i)
                {
                    default:
                    case 0: getitem = new Item { itemType = Item.ItemType.Wine, amount = 1 }; break;
                    case 1: getitem = new Item { itemType = Item.ItemType.TransPotion, amount = 1 }; break;
                    case 2: getitem = new Item { itemType = Item.ItemType.Firehell, amount = 1 }; break;
                    case 3: getitem = new Item { itemType = Item.ItemType.Hermisboots, amount = 1 }; break;
                    case 4: getitem = new Item { itemType = Item.ItemType.Phoneixfeather, amount = 1 }; break;
                }
            }

        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Item
{

    public enum ItemType
    {
        Wine,
        TransPotion,
        Firehell,
        Hermisboots,
        Phoneixfeather,
        Medusaeye,
        PowerPotion
    }

    public ItemType itemType;
    public int amount;

    public Sprite GetSprite()
    {
        switch (itemType)
        {
            default:
            case ItemType.Wine: return ItemAssets.Instance.Wine.GetComponent<SpriteRenderer>().sprite;
            case ItemType.TransPotion: return ItemAssets.Instance.TransPotion.GetComponent<SpriteRenderer>().sprite;
            case ItemType.Firehell: return ItemAssets.Instance.Firehell.GetComponent<SpriteRenderer>().sprite;
            case ItemType.Hermisboots: return ItemAssets.Instance.Hermisboots.GetComponent<SpriteRenderer>().sprite;
            case ItemType.Phoneixfeather: return ItemAssets.Instance.Phoneixfeather.GetComponent<SpriteRenderer>().sprite;
            case ItemType.Medusaeye: return ItemAssets.Instance.Medusaeye.GetComponent<SpriteRenderer>().sprite;
            case ItemType.PowerPotion: return ItemAssets.Instance.PowerPotion.GetComponent<SpriteRenderer>().sprite;
        }
    }

    public bool IsStackable()
    {
        switch (itemType)
        {
            default:
            case ItemType.Wine:
            case ItemType.TransPotion:
            case ItemType.Firehell:
            case ItemType.Hermisboots:
            case ItemType.Phoneixfeather:
            case ItemType.Medusaeye:
            case ItemType.PowerPotion:
                return true;

        }
    }

}

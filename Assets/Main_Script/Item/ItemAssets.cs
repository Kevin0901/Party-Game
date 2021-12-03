using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAssets : MonoBehaviour
{

    public static ItemAssets Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }


    public GameObject Wine;
    public GameObject TransPotion;
    public GameObject Firehell;
    public GameObject Hermisboots;
    public GameObject Phoneixfeather;
    public GameObject Medusaeye;
    public GameObject PowerPotion;

}

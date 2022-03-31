using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using Photon.Pun;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }

    public event EventHandler OnResourceAmountChanged;

    private Dictionary<ResourceTypeSo, int> RedresourceAmountDictionary;
    private Dictionary<ResourceTypeSo, int> BlueresourceAmountDictionary;
    private float timer;
    private float timerMax = 1f;
    private int c = 0;
    public DatabaseReference reference;
    private void Awake()
    {
        Instance = this;
        reference = FirebaseDatabase.DefaultInstance.RootReference;  //定義資料庫連接
        RedresourceAmountDictionary = new Dictionary<ResourceTypeSo, int>();
        BlueresourceAmountDictionary = new Dictionary<ResourceTypeSo, int>();

        ResourceTypeListSO resourceTypeList = Resources.Load<ResourceTypeListSO>(typeof(ResourceTypeListSO).Name);

        foreach (ResourceTypeSo resourceType in resourceTypeList.list)
        {
            reference.Child("GameRoom").Child(PhotonNetwork.CurrentRoom.Name).Child("RedResource").Child("resourceType.ToString()").SetValueAsync(100);
            reference.Child("GameRoom").Child(PhotonNetwork.CurrentRoom.Name).Child("BlueResource").Child("resourceType.ToString()").SetValueAsync(100);
            RedresourceAmountDictionary[resourceType] = 100;
            BlueresourceAmountDictionary[resourceType] = 100;

            c++;
            // if (c == 4)
            // {
            //     RedresourceAmountDictionary[resourceType] = 0;
            //     BlueresourceAmountDictionary[resourceType] = 0;
            // }

        }
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            timer += timerMax;
            timeadd();
        }

        // if (Input.GetKeyDown(KeyCode.T))
        // {
        //     ResourceTypeListSO resourceTypeList = Resources.Load<ResourceTypeListSO>(typeof(ResourceTypeListSO).Name);
        //     AddResource(resourceTypeList.list[0], 2);
        //     TestLogResourceAmonutDictionary();
        // }
    }

    public void AddResource(ResourceTypeSo resourceType, int amount)
    {
        RedresourceAmountDictionary[resourceType] += amount;
        BlueresourceAmountDictionary[resourceType] += amount;

        OnResourceAmountChanged?.Invoke(this, EventArgs.Empty);

        // if (OnResourceAmountChanged != null) {
        //     OnResourceAmountChanged(this, EventArgs.Empty);
        // }
    }

    public void RedAddResource(ResourceTypeSo resourceType, int amount)
    {
        RedresourceAmountDictionary[resourceType] += amount;

        OnResourceAmountChanged?.Invoke(this, EventArgs.Empty);

        // if (OnResourceAmountChanged != null) {
        //     OnResourceAmountChanged(this, EventArgs.Empty);
        // }
    }

    public void BlueAddResource(ResourceTypeSo resourceType, int amount)
    {
        BlueresourceAmountDictionary[resourceType] += amount;

        OnResourceAmountChanged?.Invoke(this, EventArgs.Empty);

        // if (OnResourceAmountChanged != null) {
        //     OnResourceAmountChanged(this, EventArgs.Empty);
        // }
    }

    private void timeadd()
    {
        ResourceTypeListSO resourceTypeList = Resources.Load<ResourceTypeListSO>(typeof(ResourceTypeListSO).Name);
        AddResource(resourceTypeList.list[0], 2);
        AddResource(resourceTypeList.list[1], 2);
        AddResource(resourceTypeList.list[2], 2);
    }

    public int RedGetResourceAmount(ResourceTypeSo resourceType)
    {
        return RedresourceAmountDictionary[resourceType];
    }

    public int BlueGetResourceAmount(ResourceTypeSo resourceType)
    {
        return BlueresourceAmountDictionary[resourceType];
    }

    public bool RedCanAfford(ResourceAmount[] resourceAmountsArray)
    {
        foreach (ResourceAmount resourceAmount in resourceAmountsArray)
        {
            if (RedGetResourceAmount(resourceAmount.resourceType) >= resourceAmount.amount)
            {

            }
            else
            {
                return false;
            }
        }
        return true;
    }

    public bool BlueCanAfford(ResourceAmount[] resourceAmountsArray)
    {
        foreach (ResourceAmount resourceAmount in resourceAmountsArray)
        {
            if (BlueGetResourceAmount(resourceAmount.resourceType) >= resourceAmount.amount)
            {

            }
            else
            {
                return false;
            }
        }
        return true;
    }

    public void RedSpendResources(ResourceAmount[] resourceAmountsArray)
    {
        foreach (ResourceAmount resourceAmount in resourceAmountsArray)
        {
            RedresourceAmountDictionary[resourceAmount.resourceType] -= resourceAmount.amount;
        }
    }
    public void BlueSpendResources(ResourceAmount[] resourceAmountsArray)
    {
        foreach (ResourceAmount resourceAmount in resourceAmountsArray)
        {
            BlueresourceAmountDictionary[resourceAmount.resourceType] -= resourceAmount.amount;
        }
    }

}

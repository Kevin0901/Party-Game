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
    public float Rrestimes = 1;
    public float Brestimes = 1;
    public float timer;
    private float timerMax = 1f;
    private int c = 0;
    public DatabaseReference reference;
    private int RedresourceAmount, BlueresourceAmount;
    PhotonView PV;
    private void Awake()
    {
        Instance = this;
        reference = FirebaseDatabase.DefaultInstance.RootReference;  //定義資料庫連接
        RedresourceAmountDictionary = new Dictionary<ResourceTypeSo, int>();
        BlueresourceAmountDictionary = new Dictionary<ResourceTypeSo, int>();
        ResourceTypeListSO resourceTypeList = Resources.Load<ResourceTypeListSO>(typeof(ResourceTypeListSO).Name);
        foreach (ResourceTypeSo resourceType in resourceTypeList.list)
        {
            reference.Child("GameRoom").Child("123456").Child("RedResource").Child(resourceType.ToString()).SetValueAsync(100);
            reference.Child("GameRoom").Child("123456").Child("BlueResource").Child(resourceType.ToString()).SetValueAsync(100);
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
    private void Start()
    {
        PV = GetComponent<PhotonView>();
        timer = 0;
        StartCoroutine(Time_Count());
    }

    private void Update()
    {
        // timer -= Time.deltaTime;
        // if (timer <= 0f)
        // {
        //     timer += timerMax;
        //     timeadd();
        // }

        // if (Input.GetKeyDown(KeyCode.T))
        // {
        //     ResourceTypeListSO resourceTypeList = Resources.Load<ResourceTypeListSO>(typeof(ResourceTypeListSO).Name);
        //     AddResource(resourceTypeList.list[0], 2);
        //     TestLogResourceAmonutDictionary();
        // }
    }

    // public void AddResource(ResourceTypeSo resourceType, int amount)
    // {
    //     StartCoroutine(GetTeamResourceData((DataSnapshot Redinfo) =>
    //     {
    //         int RedresValue = Convert.ToInt32(Redinfo.Child(resourceType.ToString()).Value);
    //         reference.Child("GameRoom").Child("123456").Child("RedResource").Child(resourceType.ToString()).SetValueAsync(RedresValue + amount);
    //     }, (DataSnapshot Blueinfo) =>
    //     {
    //         int BlueresValue = Convert.ToInt32(Blueinfo.Child(resourceType.ToString()).Value);
    //         reference.Child("GameRoom").Child("123456").Child("BlueResource").Child(resourceType.ToString()).SetValueAsync(BlueresValue + amount);
    //     }));
    //     OnResourceAmountChanged?.Invoke(this, EventArgs.Empty);
    // }
    IEnumerator Time_Count()
    {
        timeadd();
        yield return new WaitForSeconds(1f);
        timer++;
        StartCoroutine(Time_Count());
    }
    public void RedAddResource(ResourceTypeSo resourceType, int amount)
    {
        StartCoroutine(GetTeamResourceData((DataSnapshot Redinfo) =>
        {
            int resValue = Convert.ToInt32(Redinfo.Child(resourceType.ToString()).Value);
            if(PV.IsMine)
            {
                reference.Child("GameRoom").Child("123456").Child("RedResource").Child(resourceType.ToString()).SetValueAsync(resValue + amount);
            }
        }, (DataSnapshot Blueinfo) => { }));
        OnResourceAmountChanged?.Invoke(this, EventArgs.Empty);
    }

    public void BlueAddResource(ResourceTypeSo resourceType, int amount)
    {
        StartCoroutine(GetTeamResourceData((DataSnapshot Redinfo) => { }, (DataSnapshot Blueinfo) =>
           {
               int resValue = Convert.ToInt32(Blueinfo.Child(resourceType.ToString()).Value);
               if(PV.IsMine)
               {
                   reference.Child("GameRoom").Child("123456").Child("BlueResource").Child(resourceType.ToString()).SetValueAsync(resValue + amount);
               }
           }
        ));
        OnResourceAmountChanged?.Invoke(this, EventArgs.Empty);
    }

    private void timeadd()
    {
        ResourceTypeListSO resourceTypeList = Resources.Load<ResourceTypeListSO>(typeof(ResourceTypeListSO).Name);
        for (int i = 0; i <= 2; i++)
        {
            RedAddResource(resourceTypeList.list[i], (int)(2 * Rrestimes));
            BlueAddResource(resourceTypeList.list[i], (int)(2 * Brestimes));
        }
    }

    public int RedGetResourceAmount(ResourceTypeSo resourceType)
    {
        StartCoroutine(GetTeamResourceData((DataSnapshot Redinfo) =>
        {
            RedresourceAmount = Convert.ToInt32(Redinfo.Child(resourceType.ToString()).Value);
        }, (DataSnapshot Blueinfo) => { }));
        return RedresourceAmount;
    }

    public int BlueGetResourceAmount(ResourceTypeSo resourceType)
    {
        StartCoroutine(GetTeamResourceData((DataSnapshot Redinfo) => { }, (DataSnapshot Blueinfo) =>
        {
            BlueresourceAmount = Convert.ToInt32(Blueinfo.Child(resourceType.ToString()).Value);
        }));
        return BlueresourceAmount;

    }

    public bool RedCanAfford(ResourceAmount[] resourceAmountsArray)
    {
        foreach (ResourceAmount resourceAmount in resourceAmountsArray)
        {
            if (RedGetResourceAmount(resourceAmount.resourceType) < resourceAmount.amount)
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
            if (BlueGetResourceAmount(resourceAmount.resourceType) < resourceAmount.amount)
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
            StartCoroutine(GetTeamResourceData((DataSnapshot Redinfo) =>
            {
                int RedresValue = Convert.ToInt32(Redinfo.Child(resourceAmount.resourceType.ToString()).Value);
                reference.Child("GameRoom").Child("123456").Child("RedResource").Child(resourceAmount.resourceType.ToString()).SetValueAsync(RedresValue - resourceAmount.amount);
                Debug.Log(Redinfo.Child(resourceAmount.resourceType.ToString()).Value);
            }, (DataSnapshot Blueinfo) => { }));
        }
    }
    public void BlueSpendResources(ResourceAmount[] resourceAmountsArray)
    {
        foreach (ResourceAmount resourceAmount in resourceAmountsArray)
        {
            StartCoroutine(GetTeamResourceData((DataSnapshot Redinfo) => { }, (DataSnapshot Blueinfo) =>
           {
               int BlueresValue = Convert.ToInt32(Blueinfo.Child(resourceAmount.resourceType.ToString()).Value);
               reference.Child("GameRoom").Child("123456").Child("BlueResource").Child(resourceAmount.resourceType.ToString()).SetValueAsync(BlueresValue - resourceAmount.amount);
               Debug.Log(Blueinfo.Child(resourceAmount.resourceType.ToString()).Value);
           }));
        }
    }

    public IEnumerator GetTeamResourceData(System.Action<DataSnapshot> RedonCallbacks, System.Action<DataSnapshot> BlueonCallbacks)  //從資料庫抓取
    {
        var RedresData = reference.Child("GameRoom").GetValueAsync();
        RedresData = reference.Child("GameRoom").Child("123456").Child("RedResource").GetValueAsync();
        var BlueresData = reference.Child("GameRoom").GetValueAsync();
        BlueresData = reference.Child("GameRoom").Child("123456").Child("BlueResource").GetValueAsync();
        yield return new WaitUntil(predicate: () => RedresData.IsCompleted && BlueresData.IsCompleted);
        if (RedresData != null && BlueresData != null)
        {
            DataSnapshot Redsnapshot = RedresData.Result;
            DataSnapshot Bluesnapshot = BlueresData.Result;
            RedonCallbacks.Invoke(Redsnapshot);
            BlueonCallbacks.Invoke(Bluesnapshot);
        }
    }
}

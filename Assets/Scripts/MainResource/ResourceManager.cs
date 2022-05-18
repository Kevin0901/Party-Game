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

    public float Rrestimes = 1;
    public float Brestimes = 1;

    private Dictionary<ResourceTypeSo, int> RedresourceAmountDictionary;
    private Dictionary<ResourceTypeSo, int> BlueresourceAmountDictionary;

    public float timer;
    private float timerMax = 1f;
    public DatabaseReference reference;
    private int RedresourceAmount;
    private int BlueresourceAmount;
    PhotonView PV;
    private ResourceTypeListSO resourceTypeList;


    [SerializeField] private bool testing;
    private string roomname;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();
        Instance = this;
        RedresourceAmountDictionary = new Dictionary<ResourceTypeSo, int>();
        BlueresourceAmountDictionary = new Dictionary<ResourceTypeSo, int>();
        reference = FirebaseDatabase.DefaultInstance.RootReference;
        resourceTypeList = Resources.Load<ResourceTypeListSO>(typeof(ResourceTypeListSO).Name);
        if (testing)
        {
            roomname = "123456";
        }
        else
        {
            roomname = PhotonNetwork.CurrentRoom.Name;
        }
        foreach (ResourceTypeSo resourceType in resourceTypeList.list)
        {
            reference.Child("GameRoom").Child(roomname).Child("RedResource").Child(resourceType.ToString()).SetValueAsync(100);
            reference.Child("GameRoom").Child(roomname).Child("BlueResource").Child(resourceType.ToString()).SetValueAsync(100);
            RedresourceAmountDictionary[resourceType] = 100;
            BlueresourceAmountDictionary[resourceType] = 100;
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
        if (timer <= 1f)
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
    public void RedAddResource(ResourceTypeSo resourceType, int amount)
    {
        StartCoroutine(GetTeamResourceData((DataSnapshot Redinfo) =>
        {
            int resValue = Convert.ToInt32(Redinfo.Child(resourceType.ToString()).Value) + amount;
            if (PV.IsMine)
            {
                reference.Child("GameRoom").Child(roomname).Child("RedResource").Child(resourceType.ToString()).SetValueAsync(resValue);
                RedresourceAmountDictionary[resourceType] = resValue;
                OnResourceAmountChanged?.Invoke(this, EventArgs.Empty);
            }
        }, (DataSnapshot Blueinfo) => { }));
    }
    public void BlueAddResource(ResourceTypeSo resourceType, int amount)
    {
        StartCoroutine(GetTeamResourceData((DataSnapshot Redinfo) => { }, (DataSnapshot Blueinfo) =>
        {
            int resValue = Convert.ToInt32(Blueinfo.Child(resourceType.ToString()).Value);
            if (PV.IsMine)
            {
                reference.Child("GameRoom").Child(roomname).Child("BlueResource").Child(resourceType.ToString()).SetValueAsync(resValue + amount);
                BlueresourceAmountDictionary[resourceType] = resValue;
                OnResourceAmountChanged?.Invoke(this, EventArgs.Empty);
            }
        }));
    }
    public void RedAddAllResource(int amount)
    {
        StartCoroutine(GetTeamResourceData((DataSnapshot Redinfo) =>
        {
            if (PV.IsMine)
            {
                for (int i = 0; i < resourceTypeList.list.Count - 1; i++)
                {
                    int resValue = Convert.ToInt32(Redinfo.Child(resourceTypeList.list[i].ToString()).Value) + amount;
                    reference.Child("GameRoom").Child(roomname).Child("RedResource").Child(resourceTypeList.list[i].ToString()).SetValueAsync(resValue);
                    RedresourceAmountDictionary[resourceTypeList.list[i]] = resValue;
                }
                OnResourceAmountChanged?.Invoke(this, EventArgs.Empty);
            }
        }, (DataSnapshot Blueinfo) => { }));

    }

    public void BlueAddAllResource(int amount)
    {
        StartCoroutine(GetTeamResourceData((DataSnapshot Redinfo) => { }, (DataSnapshot Blueinfo) =>
           {
               if (PV.IsMine)
               {
                   for (int i = 0; i < resourceTypeList.list.Count - 1; i++)
                   {
                       int resValue = Convert.ToInt32(Blueinfo.Child(resourceTypeList.list[i].ToString()).Value) + amount;
                       reference.Child("GameRoom").Child(roomname).Child("BlueResource").Child(resourceTypeList.list[i].ToString()).SetValueAsync(resValue);
                       BlueresourceAmountDictionary[resourceTypeList.list[i]] = resValue;
                   }
                   OnResourceAmountChanged?.Invoke(this, EventArgs.Empty);
               }
           }
        ));
    }

    private void timeadd()
    {
        RedAddAllResource((int)(2 * Rrestimes));
        BlueAddAllResource((int)(2 * Brestimes));
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
        foreach (ResourceAmount resname in resourceAmountsArray)
        {
            if (RedresourceAmountDictionary[resname.resourceType] < resname.amount)
            {
                return false;
            }
        }
        return true;
    }

    public bool BlueCanAfford(ResourceAmount[] resourceAmountsArray)
    {
        foreach (ResourceAmount resname in resourceAmountsArray)
        {
            if (BlueresourceAmountDictionary[resname.resourceType] < resname.amount)
            {
                return false;
            }
        }
        return true;
    }

    public void RedSpendResources(ResourceAmount[] resourceAmountsArray)
    {
        StartCoroutine(GetTeamResourceData((DataSnapshot Redinfo) =>
        {
            foreach (ResourceAmount resourceAmount in resourceAmountsArray)
            {
                int RedresValue = Convert.ToInt32(Redinfo.Child(resourceAmount.resourceType.ToString()).Value) - resourceAmount.amount;
                reference.Child("GameRoom").Child(roomname).Child("RedResource").Child(resourceAmount.resourceType.ToString()).SetValueAsync(RedresValue);
                RedresourceAmountDictionary[resourceAmount.resourceType] = RedresValue;
            }
            OnResourceAmountChanged?.Invoke(this, EventArgs.Empty);
        }, (DataSnapshot Blueinfo) => { }));
    }
    public void BlueSpendResources(ResourceAmount[] resourceAmountsArray)
    {
        StartCoroutine(GetTeamResourceData((DataSnapshot Redinfo) => { }, (DataSnapshot Blueinfo) =>
       {
           foreach (ResourceAmount resourceAmount in resourceAmountsArray)
           {
               int BlueresValue = Convert.ToInt32(Blueinfo.Child(resourceAmount.resourceType.ToString()).Value) - resourceAmount.amount;
               reference.Child("GameRoom").Child(roomname).Child("BlueResource").Child(resourceAmount.resourceType.ToString()).SetValueAsync(BlueresValue);
               BlueresourceAmountDictionary[resourceAmount.resourceType] = BlueresValue;
           }
           OnResourceAmountChanged?.Invoke(this, EventArgs.Empty);
       }));
    }

    public IEnumerator GetTeamResourceData(System.Action<DataSnapshot> RedonCallbacks, System.Action<DataSnapshot> BlueonCallbacks)  //從資料庫抓取
    {
        var RedresData = reference.Child("GameRoom").GetValueAsync();
        RedresData = reference.Child("GameRoom").Child(roomname).Child("RedResource").GetValueAsync();
        var BlueresData = reference.Child("GameRoom").GetValueAsync();
        BlueresData = reference.Child("GameRoom").Child(roomname).Child("BlueResource").GetValueAsync();
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

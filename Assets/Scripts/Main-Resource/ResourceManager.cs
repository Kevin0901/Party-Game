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
    private float timerMax = 0.5f;
    public DatabaseReference reference;
    private List<int> RedresourceAmount = new List<int>();
    public bool getcheck = false;
    public bool canafford = false;
    private int n = 0;
    private int BlueresourceAmount;
    PhotonView PV;
    private ResourceTypeListSO resourceTypeList;
    private void Awake()
    {
        Instance = this;
        reference = FirebaseDatabase.DefaultInstance.RootReference;  //定義資料庫連接
        RedresourceAmountDictionary = new Dictionary<ResourceTypeSo, int>();
        BlueresourceAmountDictionary = new Dictionary<ResourceTypeSo, int>();
        resourceTypeList = Resources.Load<ResourceTypeListSO>(typeof(ResourceTypeListSO).Name);
        foreach (ResourceTypeSo resourceType in resourceTypeList.list)
        {
            reference.Child("GameRoom").Child(PhotonNetwork.CurrentRoom.Name).Child("RedResource").Child(resourceType.ToString()).SetValueAsync(100);
            reference.Child("GameRoom").Child(PhotonNetwork.CurrentRoom.Name).Child("BlueResource").Child(resourceType.ToString()).SetValueAsync(100);
            RedresourceAmountDictionary[resourceType] = 100;
            BlueresourceAmountDictionary[resourceType] = 100;
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
        StartCoroutine(GetTeamResourceData((DataSnapshot Redinfo) => { }, (DataSnapshot Blueinfo) => { }));
        // StartCoroutine(Time_Count());
    }

    private void Update()
    {

        timer -= Time.deltaTime;
        if (timer <= 0.5f)
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

    // public void AddResource(ResourceTypeSo resourceType, int amount)
    // {
    //     StartCoroutine(GetTeamResourceData((DataSnapshot Redinfo) =>
    //     {
    //         int RedresValue = Convert.ToInt32(Redinfo.Child(resourceType.ToString()).Value);
    //         reference.Child("GameRoom").Child(PhotonNetwork.CurrentRoom.Name).Child("RedResource").Child(resourceType.ToString()).SetValueAsync(RedresValue + amount);
    //     }, (DataSnapshot Blueinfo) =>
    //     {
    //         int BlueresValue = Convert.ToInt32(Blueinfo.Child(resourceType.ToString()).Value);
    //         reference.Child("GameRoom").Child(PhotonNetwork.CurrentRoom.Name).Child("BlueResource").Child(resourceType.ToString()).SetValueAsync(BlueresValue + amount);
    //     }));
    //     OnResourceAmountChanged?.Invoke(this, EventArgs.Empty);
    // }
    // IEnumerator Time_Count()
    // {
    //     timeadd();
    //     yield return new WaitForSeconds(2f);
    //     timer++;
    //     StartCoroutine(Time_Count());
    // }
    public void RedAddResource(ResourceTypeSo resourceType, int amount)
    {
        StartCoroutine(GetTeamResourceData((DataSnapshot Redinfo) =>
        {
            int resValue = Convert.ToInt32(Redinfo.Child(resourceType.ToString()).Value);
            if (PV.IsMine)
            {
                reference.Child("GameRoom").Child(PhotonNetwork.CurrentRoom.Name).Child("RedResource").Child(resourceType.ToString()).SetValueAsync(resValue + amount);
                OnResourceAmountChanged?.Invoke(this, EventArgs.Empty);
            }
        }, (DataSnapshot Blueinfo) => { }));
    }
    public void RedAddAllResource(int amount)
    {
        StartCoroutine(GetTeamResourceData((DataSnapshot Redinfo) =>
        {
            int resValue = Convert.ToInt32(Redinfo.Child(resourceTypeList.list[0].ToString()).Value);
            int resValue1 = Convert.ToInt32(Redinfo.Child(resourceTypeList.list[1].ToString()).Value);
            int resValue2 = Convert.ToInt32(Redinfo.Child(resourceTypeList.list[2].ToString()).Value);
            // if (PV.IsMine)
            // {
            reference.Child("GameRoom").Child(PhotonNetwork.CurrentRoom.Name).Child("RedResource").Child(resourceTypeList.list[0].ToString()).SetValueAsync(resValue + amount);
            reference.Child("GameRoom").Child(PhotonNetwork.CurrentRoom.Name).Child("RedResource").Child(resourceTypeList.list[1].ToString()).SetValueAsync(resValue1 + amount);
            reference.Child("GameRoom").Child(PhotonNetwork.CurrentRoom.Name).Child("RedResource").Child(resourceTypeList.list[2].ToString()).SetValueAsync(resValue2 + amount);
            OnResourceAmountChanged?.Invoke(this, EventArgs.Empty);
            // }
        }, (DataSnapshot Blueinfo) => { }));
    }
    public void BlueAddAllResource(int amount)
    {
        StartCoroutine(GetTeamResourceData((DataSnapshot Redinfo) => { }, (DataSnapshot Blueinfo) =>
           {
               int resValue = Convert.ToInt32(Blueinfo.Child(resourceTypeList.list[0].ToString()).Value);
               int resValue1 = Convert.ToInt32(Blueinfo.Child(resourceTypeList.list[1].ToString()).Value);
               int resValue2 = Convert.ToInt32(Blueinfo.Child(resourceTypeList.list[2].ToString()).Value);
               // if (PV.IsMine)
               // {
               reference.Child("GameRoom").Child(PhotonNetwork.CurrentRoom.Name).Child("BlueResource").Child(resourceTypeList.list[0].ToString()).SetValueAsync(resValue + amount);
               reference.Child("GameRoom").Child(PhotonNetwork.CurrentRoom.Name).Child("BlueResource").Child(resourceTypeList.list[1].ToString()).SetValueAsync(resValue1 + amount);
               reference.Child("GameRoom").Child(PhotonNetwork.CurrentRoom.Name).Child("BlueResource").Child(resourceTypeList.list[2].ToString()).SetValueAsync(resValue2 + amount);
               OnResourceAmountChanged?.Invoke(this, EventArgs.Empty);
           }
        )); ;
    }

    public void BlueAddResource(ResourceTypeSo resourceType, int amount)
    {
        StartCoroutine(GetTeamResourceData((DataSnapshot Redinfo) => { }, (DataSnapshot Blueinfo) =>
           {
               int resValue = Convert.ToInt32(Blueinfo.Child(resourceType.ToString()).Value);
               if (PV.IsMine)
               {
                   reference.Child("GameRoom").Child(PhotonNetwork.CurrentRoom.Name).Child("BlueResource").Child(resourceType.ToString()).SetValueAsync(resValue + amount);
                   OnResourceAmountChanged?.Invoke(this, EventArgs.Empty);
               }
           }
        ));
    }

    private void timeadd()
    {
        RedAddAllResource((int)(2 * Rrestimes));
        BlueAddAllResource((int)(2 * Rrestimes));
    }

    public void RedGetResourceAmount(ResourceAmount[] resourceAmountsArray)
    {
        int c = 0;
        List<int> reslist = new List<int>();
        getcheck = false;
        StartCoroutine(GetTeamResourceData((DataSnapshot Redinfo) =>
        {
            if (Convert.ToInt32(Redinfo.Child(resourceAmountsArray[0].resourceType.ToString()).Value) > resourceAmountsArray[0].amount)
            {
                c++;
            }
            if (Convert.ToInt32(Redinfo.Child(resourceAmountsArray[1].resourceType.ToString()).Value) > resourceAmountsArray[1].amount)
            {
                c++;
            }
            if (Convert.ToInt32(Redinfo.Child(resourceAmountsArray[2].resourceType.ToString()).Value) > resourceAmountsArray[2].amount)
            {
                c++;
            }
            if (c == resourceAmountsArray.Length)
            {
                canafford = true;
            }
            getcheck = true;
            c++;
            // reslist.Add(Convert.ToInt32(Redinfo.Child(resourceAmountsArray[0].resourceType.ToString()).Value));
            // reslist.Add(Convert.ToInt32(Redinfo.Child(resourceAmountsArray[1].resourceType.ToString()).Value));
            // reslist.Add(Convert.ToInt32(Redinfo.Child(resourceAmountsArray[2].resourceType.ToString()).Value));
            // RedresourceAmount = reslist;
        }, (DataSnapshot Blueinfo) => { }));


    }

    public int BlueGetResourceAmount(ResourceTypeSo resourceType)
    {
        StartCoroutine(GetTeamResourceData((DataSnapshot Redinfo) => { }, (DataSnapshot Blueinfo) =>
        {
            BlueresourceAmount = Convert.ToInt32(Blueinfo.Child(resourceType.ToString()).Value);
        }));
        return BlueresourceAmount;

    }

    public void TestCanAfford(ResourceAmount[] resourceAmountsArray)
    {
        RedGetResourceAmount(resourceAmountsArray);
    }

    public bool RedCanAfford(ResourceAmount[] resourceAmountsArray)
    {
        Debug.Log(RedresourceAmount.Count);
        RedGetResourceAmount(resourceAmountsArray);
        if (getcheck)
        {
            Debug.Log(RedresourceAmount.Count);
        }
        // Debug.Log(RedresourceAmount[0]);
        Debug.Log(resourceAmountsArray[0].amount);
        // Debug.Log(RedresourceAmount[0] + ":" + resourceAmountsArray[0].amount);
        // Debug.Log(RedresourceAmount[1] + ":" + resourceAmountsArray[1].amount);
        // Debug.Log(RedresourceAmount[2] + ":" + resourceAmountsArray[2].amount);
        // for (int i = 0; i < RedresourceAmount.Count; i++)
        // {
        //     Debug.Log(RedresourceAmount[i] + ":" + resourceAmountsArray[i].amount);
        //     if (RedresourceAmount[i] < resourceAmountsArray[i].amount)
        //     {
        //         return false;
        //     }
        // }
        return true;
        // foreach (ResourceAmount resourceAmount in resourceAmountsArray)
        // {
        //     if (RedGetResourceAmount(resourceAmount.resourceType) < resourceAmount.amount)
        //     {
        //         return false;
        //     }
        // }
        // return true;
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
                reference.Child("GameRoom").Child(PhotonNetwork.CurrentRoom.Name).Child("RedResource").Child(resourceAmount.resourceType.ToString()).SetValueAsync(RedresValue - resourceAmount.amount);
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
               reference.Child("GameRoom").Child(PhotonNetwork.CurrentRoom.Name).Child("BlueResource").Child(resourceAmount.resourceType.ToString()).SetValueAsync(BlueresValue - resourceAmount.amount);
               Debug.Log(Blueinfo.Child(resourceAmount.resourceType.ToString()).Value);
           }));
        }
    }

    public IEnumerator GetTeamResourceData(System.Action<DataSnapshot> RedonCallbacks, System.Action<DataSnapshot> BlueonCallbacks)  //從資料庫抓取
    {
        var RedresData = reference.Child("GameRoom").GetValueAsync();
        RedresData = reference.Child("GameRoom").Child(PhotonNetwork.CurrentRoom.Name).Child("RedResource").GetValueAsync();
        var BlueresData = reference.Child("GameRoom").GetValueAsync();
        BlueresData = reference.Child("GameRoom").Child(PhotonNetwork.CurrentRoom.Name).Child("BlueResource").GetValueAsync();
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

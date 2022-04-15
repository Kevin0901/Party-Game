using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance { get; private set; }

    public event EventHandler OnResourceAmountChanged;

    public float Rrestimes=1;
    public float Brestimes=1;

    private Dictionary<ResourceTypeSo, int> RedresourceAmountDictionary;
    private Dictionary<ResourceTypeSo, int> BlueresourceAmountDictionary;
<<<<<<< Updated upstream

    private float timer;
    private float timerMax = 1f;
    private int c = 0;

=======
    public float Rrestimes = 1;
    public float Brestimes = 1;
    public float timer;
    private float timerMax = 1f;
    private int c = 0;
    public DatabaseReference reference;
    private int[] RedresourceAmount;
    private int BlueresourceAmount;
    PhotonView PV;
    private ResourceTypeListSO resourceTypeList;
<<<<<<< Updated upstream
    // [System.NonSerialized] ResourceTypeListSO resourceTypeList;
>>>>>>> Stashed changes
=======
    [SerializeField] private bool testing;
    private string roomname;
>>>>>>> Stashed changes
    private void Awake()
    {
        Instance = this;
        RedresourceAmountDictionary = new Dictionary<ResourceTypeSo, int>();
        BlueresourceAmountDictionary = new Dictionary<ResourceTypeSo, int>();
<<<<<<< Updated upstream

        ResourceTypeListSO resourceTypeList = Resources.Load<ResourceTypeListSO>(typeof(ResourceTypeListSO).Name);

        foreach (ResourceTypeSo resourceType in resourceTypeList.list)
        {
=======
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
>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
=======
    private void Start()
    {
        // PV = GetComponent<PhotonView>();
        timer = 0;
        StartCoroutine(GetTeamResourceData((DataSnapshot Redinfo) => { }, (DataSnapshot Blueinfo) => { }));
        // StartCoroutine(Time_Count());
    }
>>>>>>> Stashed changes

    private void Update()
    {
        timer -= Time.deltaTime;
<<<<<<< Updated upstream
        if (timer <= 0f)
=======
        if (timer <= 1f)
>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
        RedresourceAmountDictionary[resourceType] += amount;

        OnResourceAmountChanged?.Invoke(this, EventArgs.Empty);

        // if (OnResourceAmountChanged != null) {
        //     OnResourceAmountChanged(this, EventArgs.Empty);
        // }
=======
        StartCoroutine(GetTeamResourceData((DataSnapshot Redinfo) =>
        {
            int resValue = Convert.ToInt32(Redinfo.Child(resourceType.ToString()).Value);
<<<<<<< Updated upstream
            // if (PV.IsMine)
            // {
                reference.Child("GameRoom").Child("123456").Child("RedResource").Child(resourceType.ToString()).SetValueAsync(resValue + amount);
=======
            if (PV.IsMine)
            {
                reference.Child("GameRoom").Child(roomname).Child("RedResource").Child(resourceType.ToString()).SetValueAsync(resValue + amount);
>>>>>>> Stashed changes
                OnResourceAmountChanged?.Invoke(this, EventArgs.Empty);
            // }
        }, (DataSnapshot Blueinfo) => { }));
    }
    public void RedAddAllResource(int amount)
    {
        StartCoroutine(GetTeamResourceData((DataSnapshot Redinfo) =>
        {
<<<<<<< Updated upstream
            int resValue1 = Convert.ToInt32(Redinfo.Child(resourceTypeList.list[0].ToString()).Value);
            int resValue2= Convert.ToInt32(Redinfo.Child(resourceTypeList.list[1].ToString()).Value);
            int resValue3 = Convert.ToInt32(Redinfo.Child(resourceTypeList.list[2].ToString()).Value);
            // if (PV.IsMine)
            // {
                reference.Child("GameRoom").Child("123456").Child("RedResource").Child(resourceTypeList.list[0].ToString()).SetValueAsync(resValue1 + amount);
                reference.Child("GameRoom").Child("123456").Child("RedResource").Child(resourceTypeList.list[1].ToString()).SetValueAsync(resValue2 + amount);
                reference.Child("GameRoom").Child("123456").Child("RedResource").Child(resourceTypeList.list[2].ToString()).SetValueAsync(resValue3 + amount);
                OnResourceAmountChanged?.Invoke(this, EventArgs.Empty);
=======
            // if (PV.IsMine)
            // {
            for (int i = 0; i < resourceTypeList.list.Count - 1; i++)
            {
                int resValue = Convert.ToInt32(Redinfo.Child(resourceTypeList.list[i].ToString()).Value) + amount;
                reference.Child("GameRoom").Child(roomname).Child("RedResource").Child(resourceTypeList.list[i].ToString()).SetValueAsync(resValue);
                RedresourceAmountDictionary[resourceTypeList.list[i]] = resValue;
            }
            OnResourceAmountChanged?.Invoke(this, EventArgs.Empty);
>>>>>>> Stashed changes
            // }
        }, (DataSnapshot Blueinfo) => { }));
>>>>>>> Stashed changes
    }

    public void BlueAddAllResource(int amount)
    {
        StartCoroutine(GetTeamResourceData((DataSnapshot Redinfo) => { }, (DataSnapshot Blueinfo) =>
           {
<<<<<<< Updated upstream
               int resValue1 = Convert.ToInt32(Blueinfo.Child(resourceTypeList.list[0].ToString()).Value);
            int resValue2= Convert.ToInt32(Blueinfo.Child(resourceTypeList.list[1].ToString()).Value);
            int resValue3 = Convert.ToInt32(Blueinfo.Child(resourceTypeList.list[2].ToString()).Value);
            // if (PV.IsMine)
            // {
                reference.Child("GameRoom").Child("123456").Child("BlueResource").Child(resourceTypeList.list[0].ToString()).SetValueAsync(resValue1 + amount);
                reference.Child("GameRoom").Child("123456").Child("BlueResource").Child(resourceTypeList.list[1].ToString()).SetValueAsync(resValue2 + amount);
                reference.Child("GameRoom").Child("123456").Child("BlueResource").Child(resourceTypeList.list[2].ToString()).SetValueAsync(resValue3 + amount);
                OnResourceAmountChanged?.Invoke(this, EventArgs.Empty);
            // }
=======
               // if (PV.IsMine)
               // {
               for (int i = 0; i < resourceTypeList.list.Count - 1; i++)
               {
                   int resValue = Convert.ToInt32(Blueinfo.Child(resourceTypeList.list[i].ToString()).Value) + amount;
                   reference.Child("GameRoom").Child(roomname).Child("BlueResource").Child(resourceTypeList.list[i].ToString()).SetValueAsync(resValue);
                   BlueresourceAmountDictionary[resourceTypeList.list[i]] = resValue;
               }
               OnResourceAmountChanged?.Invoke(this, EventArgs.Empty);
               // }
>>>>>>> Stashed changes
           }
        ));
    }
    public void BlueAddResource(ResourceTypeSo resourceType, int amount)
    {
<<<<<<< Updated upstream
        BlueresourceAmountDictionary[resourceType] += amount;

        OnResourceAmountChanged?.Invoke(this, EventArgs.Empty);

        // if (OnResourceAmountChanged != null) {
        //     OnResourceAmountChanged(this, EventArgs.Empty);
        // }
=======
        StartCoroutine(GetTeamResourceData((DataSnapshot Redinfo) => { }, (DataSnapshot Blueinfo) =>
           {
               int resValue = Convert.ToInt32(Blueinfo.Child(resourceType.ToString()).Value);
<<<<<<< Updated upstream
            //    if (PV.IsMine)
            //    {
                   reference.Child("GameRoom").Child("123456").Child("BlueResource").Child(resourceType.ToString()).SetValueAsync(resValue + amount);
=======
               if (PV.IsMine)
               {
                   reference.Child("GameRoom").Child(roomname).Child("BlueResource").Child(resourceType.ToString()).SetValueAsync(resValue + amount);
>>>>>>> Stashed changes
                   OnResourceAmountChanged?.Invoke(this, EventArgs.Empty);
            //    }
           }
        ));
>>>>>>> Stashed changes
    }

    private void timeadd()
    {
<<<<<<< Updated upstream
        ResourceTypeListSO resourceTypeList = Resources.Load<ResourceTypeListSO>(typeof(ResourceTypeListSO).Name);
        for(int i = 0; i<=2;i++){
            RedAddResource(resourceTypeList.list[i], (int)(2*Rrestimes));
            BlueAddResource(resourceTypeList.list[i], (int)(2*Brestimes));
        }
=======
        RedAddAllResource((int)(2 * Rrestimes));
        BlueAddAllResource((int)(2 * Rrestimes));
>>>>>>> Stashed changes
    }

<<<<<<< Updated upstream
    public int[] RedGetResourceAmount(ResourceAmount[] resourceType)
    {
<<<<<<< Updated upstream
        return RedresourceAmountDictionary[resourceType];
=======
        // bool check=false;
        int c=0;
        StartCoroutine(GetTeamResourceData((DataSnapshot Redinfo) =>
        {
            foreach (ResourceAmount resourceAmount in resourceType)
        {
            RedresourceAmount[c] = Convert.ToInt32(Redinfo.Child(resourceAmount.ToString()).Value);
            Debug.Log(RedresourceAmount[c]);
            c++;
        }
            
        }, (DataSnapshot Blueinfo) => { }));
        return RedresourceAmount;
>>>>>>> Stashed changes
=======
    public int RedGetResourceAmount(ResourceTypeSo resourceType)
    {
        return RedresourceAmountDictionary[resourceType];
>>>>>>> Stashed changes
    }

    public int BlueGetResourceAmount(ResourceTypeSo resourceType)
    {
        return BlueresourceAmountDictionary[resourceType];
    }

    public bool RedCanAfford(ResourceAmount[] resourceAmountsArray)
    {
<<<<<<< Updated upstream
<<<<<<< Updated upstream
        foreach (ResourceAmount resourceAmount in resourceAmountsArray)
        {
            if (RedGetResourceAmount(resourceAmount.resourceType) >= resourceAmount.amount)
            {

            }
            else
=======
        foreach (ResourceAmount resname in resourceAmountsArray)
        {
            if (RedresourceAmountDictionary[resname.resourceType] < resname.amount)
>>>>>>> Stashed changes
            {
                return false;
            }
        }
        return true;
<<<<<<< Updated upstream
=======
       int[] reslist = RedGetResourceAmount(resourceAmountsArray);
    //    for(int i=0;i<reslist.Length;i++){
    //        Debug.Log("a");
    //        if(reslist[i] < resourceAmountsArray[i].amount){
    //            return false;
    //        }
    //    }
       return true;
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
    }

    public bool BlueCanAfford(ResourceAmount[] resourceAmountsArray)
    {
        foreach (ResourceAmount resname in resourceAmountsArray)
        {
<<<<<<< Updated upstream
            if (BlueGetResourceAmount(resourceAmount.resourceType) >= resourceAmount.amount)
            {

            }
            else
=======
            if (BlueresourceAmountDictionary[resname.resourceType] < resname.amount)
>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
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

=======
            foreach (ResourceAmount resourceAmount in resourceAmountsArray)
            {
                int RedresValue = Convert.ToInt32(Redinfo.Child(resourceAmount.resourceType.ToString()).Value);
                reference.Child("GameRoom").Child(roomname).Child("RedResource").Child(resourceAmount.resourceType.ToString()).SetValueAsync(RedresValue - resourceAmount.amount);
                Debug.Log(Redinfo.Child(resourceAmount.resourceType.ToString()).Value);
            }
        }, (DataSnapshot Blueinfo) => { }));
    }
    public void BlueSpendResources(ResourceAmount[] resourceAmountsArray)
    {
        StartCoroutine(GetTeamResourceData((DataSnapshot Redinfo) => { }, (DataSnapshot Blueinfo) =>
       {
           foreach (ResourceAmount resourceAmount in resourceAmountsArray)
           {
               int BlueresValue = Convert.ToInt32(Blueinfo.Child(resourceAmount.resourceType.ToString()).Value);
               reference.Child("GameRoom").Child(roomname).Child("BlueResource").Child(resourceAmount.resourceType.ToString()).SetValueAsync(BlueresValue - resourceAmount.amount);
               Debug.Log(Blueinfo.Child(resourceAmount.resourceType.ToString()).Value);
           }
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
>>>>>>> Stashed changes
}

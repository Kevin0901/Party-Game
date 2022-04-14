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
    // [System.NonSerialized] ResourceTypeListSO resourceTypeList;
>>>>>>> Stashed changes
    private void Awake()
    {
        Instance = this;
        RedresourceAmountDictionary = new Dictionary<ResourceTypeSo, int>();
        BlueresourceAmountDictionary = new Dictionary<ResourceTypeSo, int>();

        ResourceTypeListSO resourceTypeList = Resources.Load<ResourceTypeListSO>(typeof(ResourceTypeListSO).Name);

        foreach (ResourceTypeSo resourceType in resourceTypeList.list)
        {
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
            // if (PV.IsMine)
            // {
                reference.Child("GameRoom").Child("123456").Child("RedResource").Child(resourceType.ToString()).SetValueAsync(resValue + amount);
                OnResourceAmountChanged?.Invoke(this, EventArgs.Empty);
            // }
        }, (DataSnapshot Blueinfo) => { }));
    }
    public void RedAddAllResource(int amount)
    {
        StartCoroutine(GetTeamResourceData((DataSnapshot Redinfo) =>
        {
            int resValue1 = Convert.ToInt32(Redinfo.Child(resourceTypeList.list[0].ToString()).Value);
            int resValue2= Convert.ToInt32(Redinfo.Child(resourceTypeList.list[1].ToString()).Value);
            int resValue3 = Convert.ToInt32(Redinfo.Child(resourceTypeList.list[2].ToString()).Value);
            // if (PV.IsMine)
            // {
                reference.Child("GameRoom").Child("123456").Child("RedResource").Child(resourceTypeList.list[0].ToString()).SetValueAsync(resValue1 + amount);
                reference.Child("GameRoom").Child("123456").Child("RedResource").Child(resourceTypeList.list[1].ToString()).SetValueAsync(resValue2 + amount);
                reference.Child("GameRoom").Child("123456").Child("RedResource").Child(resourceTypeList.list[2].ToString()).SetValueAsync(resValue3 + amount);
                OnResourceAmountChanged?.Invoke(this, EventArgs.Empty);
            // }
        }, (DataSnapshot Blueinfo) => { }));
>>>>>>> Stashed changes
    }

    public void BlueAddAllResource(int amount)
    {
        StartCoroutine(GetTeamResourceData((DataSnapshot Redinfo) => { }, (DataSnapshot Blueinfo) =>
           {
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
            //    if (PV.IsMine)
            //    {
                   reference.Child("GameRoom").Child("123456").Child("BlueResource").Child(resourceType.ToString()).SetValueAsync(resValue + amount);
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
    }

    public int BlueGetResourceAmount(ResourceTypeSo resourceType)
    {
        return BlueresourceAmountDictionary[resourceType];
    }

    public bool RedCanAfford(ResourceAmount[] resourceAmountsArray)
    {
<<<<<<< Updated upstream
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

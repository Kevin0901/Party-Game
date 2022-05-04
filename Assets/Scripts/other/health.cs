using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Photon.Pun;
using Firebase;
using Firebase.Database;
using System;
public class health : MonoBehaviour
{
    // Start is called before the first frame update
    public int maxH, curH;
    private GameObject parentSet;
    private RectTransform proportion;
    private float newScale, baseScale;
    private Slider bar;
    public bool iswudi;//無敵
    public int playercatchsheeponhit = 0;
    PhotonView PV;
    bool isfirstload = true;
    public DatabaseReference reference;
    private void Awake()
    {
        ResourceTypeListSO resourceTypeList = Resources.Load<ResourceTypeListSO>(typeof(ResourceTypeListSO).Name);
        reference = FirebaseDatabase.DefaultInstance.RootReference;  //定義資料庫連接
    }
    private void OnEnable()
    {
        if (!isfirstload)
        {
            reference.Child("GameRoom").Child(PhotonNetwork.CurrentRoom.Name).Child("Health").Child(PV.ViewID.ToString()).SetValueAsync(curH);
            StartCoroutine(Load_Health_From_Database());
        }
    }
    void Start()
    {
        isfirstload = true;
        PV = this.GetComponent<PhotonView>();
        iswudi = false;
        healthBarSet();
        curH = maxH;
        parentSet = this.transform.parent.gameObject;
        bar = this.gameObject.GetComponentInChildren<Slider>();
        bar.maxValue = maxH;
        reference.Child("GameRoom").Child(PhotonNetwork.CurrentRoom.Name).Child("Health").Child(PV.ViewID.ToString()).SetValueAsync(curH);
        if (isfirstload)
        {
            StartCoroutine(Load_Health_From_Database());
            isfirstload = false;
        }
    }
    private void Update()
    {
        if (curH <= 0 && parentSet.layer != 14)
        {
            ResourceTypeListSO resourceTypeList = Resources.Load<ResourceTypeListSO>(typeof(ResourceTypeListSO).Name); //死掉加對方資源
            if (parentSet.tag == "red")
            {
                ResourceManager.Instance.BlueAddResource(resourceTypeList.list[0], 3);
                ResourceManager.Instance.BlueAddResource(resourceTypeList.list[1], 3);
                ResourceManager.Instance.BlueAddResource(resourceTypeList.list[2], 3);
            }
            else if (parentSet.tag == "blue")
            {
                ResourceManager.Instance.RedAddResource(resourceTypeList.list[0], 3);
                ResourceManager.Instance.RedAddResource(resourceTypeList.list[1], 3);
                ResourceManager.Instance.RedAddResource(resourceTypeList.list[2], 3);
            }
        }
        if (curH <= 0 && parentSet.layer != 10 && parentSet.layer != 14)
        {
            reference.Child("GameRoom").Child(PhotonNetwork.CurrentRoom.Name).Child("Health").Child(PV.ViewID.ToString()).SetValueAsync(null);
            Destroy(parentSet);
        }
        bar.value = curH;
    }
    public void Hurt(int damageToGive)
    {
        if (parentSet.layer == 10)
        {
            playercatchsheeponhit = 1;
        }
        if (parentSet.layer == 10 && curH - damageToGive <= 0 && parentSet.GetComponent<PlayerMovement>().phfeather == 1)
        {
            StartCoroutine(phoenix(3));
        }
        else if (!iswudi)
        {
            if (!PV.IsMine)
            {
                return;
            }
            curH -= damageToGive;
            reference.Child("GameRoom").Child(PhotonNetwork.CurrentRoom.Name).Child("Health").Child(PV.ViewID.ToString()).SetValueAsync(curH);
            // PV.RPC("RPC_Hurt", RpcTarget.All, damageToGive, PV.Owner.NickName);
        }
    }
    // [PunRPC]
    // void RPC_Hurt(int dam, string who)
    // {
    //     if (PV.IsMine)
    //     {
    //         return;
    //     }
    //     if (PV.Owner.NickName.Equals(who))
    //     {
    //         curH -= dam;
    //     }
    // }

    IEnumerator Load_Health_From_Database()
    {
        yield return new WaitForSeconds(1f);
        StartCoroutine(GetHealthInfo((DataSnapshot info) =>  //從資料庫抓取此房間內的所有資料
        {
            foreach (var health in info.Children)
            {
                if (health.Key.Equals(PV.ViewID.ToString()))
                {
                    Debug.Log((int)Int64.Parse(health.Value.ToString()));
                    curH = (int)Int64.Parse(health.Value.ToString());
                    bar.value = curH;
                }
            }
            StartCoroutine(Load_Health_From_Database());
        }));
    }

    IEnumerator GetHealthInfo(System.Action<DataSnapshot> onCallbacks)  //從資料庫抓取此房間的所有資料
    {
        var userData = reference.Child("GameRoom").Child(PhotonNetwork.CurrentRoom.Name).Child("Health").GetValueAsync();

        yield return new WaitUntil(predicate: () => userData.IsCompleted);

        if (userData != null)
        {
            DataSnapshot snapshot = userData.Result;
            onCallbacks.Invoke(snapshot);
        }
    }

    private IEnumerator phoenix(int t) //無敵
    {
        parentSet.GetComponent<PlayerMovement>().phfeatheruse = 1;
        yield return new WaitForSeconds(t);
        parentSet.GetComponent<PlayerMovement>().phfeather -= 1;
    }
    public void healthBarSet()
    {
        // baseTop = proportion.anchoredPosition.y;
        // newTop = baseTop / 1.3f;
        // proportion.anchoredPosition3D = new Vector3(0, newTop, 0);
        //大小設定
        if (this.transform.parent.gameObject.layer != 14)
        {
            proportion = this.gameObject.GetComponent<RectTransform>();
            baseScale = 0.01f;
            newScale = baseScale / gameObject.transform.parent.localScale.x;
            if (newScale != baseScale)
            {
                proportion.localScale = new Vector3(newScale, newScale, newScale);
            }
        }
        if (this.transform.parent.tag == "blue")
        {
            this.transform.GetChild(0).Find("Fill").GetComponent<Image>().color = new Color32(44, 43, 178, 255);
        }
    }
    // public bool lowHpToDie(int damege)
    // {
    //     if (curH - damege < 0)
    //     {
    //         return true;
    //     }
    //     else
    //     {
    //         return false;
    //     }
    // }
}

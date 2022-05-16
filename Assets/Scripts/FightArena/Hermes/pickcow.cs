using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Firebase.Database;
using System;
public class pickcow : MonoBehaviour
{
    public int cowScore;
    PhotonView PV;
    DatabaseReference reference;
    void Start()
    {
        PV = GetComponent<PhotonView>();  //定義PhotonView
        reference = FirebaseDatabase.DefaultInstance.RootReference;  //定義資料庫連接
        this.transform.parent = PhotonView.Find((int)PV.InstantiationData[0]).transform;
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == 10)
        {
            if (other.GetComponent<arenaPlayer>().red)
            {
                // this.transform.parent.GetComponent<HermesEvent>().R_ScoreADD(cowScore);
                StartCoroutine(GetScoreInfo((DataSnapshot info) =>  //從資料庫抓取此房間內的所有資料
                {
                    if (PV.IsMine)
                    {
                        foreach (var Team in info.Children)
                        {
                            if (Team.Key.Equals("red"))
                            {
                                int NewScore = (int)Int64.Parse(Team.Value.ToString()) + cowScore;
                                reference.Child("GameRoom").Child(PhotonNetwork.CurrentRoom.Name).Child("Arena").Child("HermesScore").Child("red").SetValueAsync(NewScore);
                            }
                        }
                        PhotonNetwork.Destroy(this.gameObject);
                    }
                }));
            }
            else
            {
                // this.transform.parent.GetComponent<HermesEvent>().B_ScoreADD(cowScore);
                StartCoroutine(GetScoreInfo((DataSnapshot info) =>  //從資料庫抓取此房間內的所有資料
                {
                    if (PV.IsMine)
                    {
                        foreach (var Team in info.Children)
                        {
                            if (Team.Key.Equals("blue"))
                            {
                                int NewScore = (int)Int64.Parse(Team.Value.ToString()) + cowScore;
                                reference.Child("GameRoom").Child(PhotonNetwork.CurrentRoom.Name).Child("Arena").Child("HermesScore").Child("blue").SetValueAsync(NewScore);
                            }
                        }
                        PhotonNetwork.Destroy(this.gameObject);
                    }
                }));
            }
        }
    }

    IEnumerator GetScoreInfo(System.Action<DataSnapshot> onCallbacks)  //從資料庫抓取此房間的所有資料
    {
        var userData = reference.Child("GameRoom").Child(PhotonNetwork.CurrentRoom.Name).Child("Arena").Child("HermesScore").GetValueAsync();

        yield return new WaitUntil(predicate: () => userData.IsCompleted);

        if (userData != null)
        {
            DataSnapshot snapshot = userData.Result;
            onCallbacks.Invoke(snapshot);
        }
    }
}

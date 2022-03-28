using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Firebase.Database;
using TMPro;
public class TeamSelect : MonoBehaviour
{
    public bool red, blue;
    private int playsort;
    private ChoosePlayer chooseP;
    PhotonView PV;
    GameObject NameText;
    DatabaseReference reference;
    TMP_Text textName;

    void Start()
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;  //定義資料庫連接
        chooseP = this.transform.parent.GetComponent<ChoosePlayer>();
        PV = this.gameObject.GetComponent<PhotonView>();  //定義PhotonView
        NameText = this.gameObject.transform.Find("Text (TMP)").gameObject;
        textName = this.gameObject.GetComponent<PlayerListItem>().text;
    }

    public void ArrowStart()  //開啟箭頭的 UI
    {
        transform.Find("leftArrow").gameObject.SetActive(true);
        transform.Find("rightArrow").gameObject.SetActive(true);
        TeamChoose();
    }

    void Update()
    {
        if (chooseP.CanvasGroup.blocksRaycasts)
        {
            if (Input.GetButtonDown("Horizontal") && PV.IsMine && NameText.activeSelf)
            {
                if (red)
                {
                    blue = true;
                    red = false;
                    //寫進資料庫
                    reference.Child("GameRoom").Child(PhotonNetwork.CurrentRoom.Name).Child("PlayerList").Child(textName.text).Child("team").SetValueAsync("blue");
                }
                else
                {
                    blue = false;
                    red = true;
                    //寫進資料庫
                    reference.Child("GameRoom").Child(PhotonNetwork.CurrentRoom.Name).Child("PlayerList").Child(textName.text).Child("team").SetValueAsync("red");
                }
                PV.RPC("RPC_RefreshTeam", RpcTarget.All, textName.text, this.gameObject.name, red); //廣播到所有玩家的電腦，說我切換了隊伍
            }
            TeamChoose();
        }
    }

    [PunRPC]  //廣播的方法前面都需要加這個開頭
    void RPC_RefreshTeam(string ChangePlayerName, string order, bool _red)
    {
        if (PV.IsMine)  //如果廣播輪到自己，直接 Skip
        {
            return;
        }
        GetTeam(order,_red);
    }
    void GetTeam(string order, bool _red)  //設定發出廣播的玩家的 Team
    {
        Player[] players = PhotonNetwork.PlayerList;
        if (order.Equals(this.gameObject.name))
        {
            if(_red)
            {
                red = true;
                blue = false;
            }
            else
            {
                blue = true;
                red = false;
            }
            TeamChoose();
        }
    }
    // public void Left()//按鈕
    // {
    //     if (chooseP.CanvasGroup.blocksRaycasts && PV.IsMine && NameText.activeSelf)
    //     {
    //         if (red)
    //         {
    //             blue = true;
    //             red = false;
    //         }
    //         else
    //         {
    //             blue = false;
    //             red = true;
    //         }
    //     }
    // }
    // public void Right()//按鈕
    // {
    //     if (chooseP.CanvasGroup.blocksRaycasts && PV.IsMine && NameText.activeSelf)
    //     {
    //         if (red)
    //         {
    //             blue = true;
    //             red = false;
    //         }
    //         else
    //         {
    //             blue = false;
    //             red = true;
    //         }
    //     }
    // }
    public void TeamChoose()
    {
        if (red)
        {
            transform.Find("red").gameObject.SetActive(true);
            transform.Find("RedPlayer").gameObject.SetActive(true);
            transform.Find("blue").gameObject.SetActive(false);
            transform.Find("BluePlayer").gameObject.SetActive(false);
        }
        else if (blue)
        {
            transform.Find("blue").gameObject.SetActive(true);
            transform.Find("BluePlayer").gameObject.SetActive(true);
            transform.Find("red").gameObject.SetActive(false);
            transform.Find("RedPlayer").gameObject.SetActive(false);
        }
    }
}

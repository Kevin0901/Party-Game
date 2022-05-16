using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using Firebase;
using Firebase.Database;
public class PlayerListItem : MonoBehaviourPunCallbacks
{
    [Header("玩家名稱")]
    public TMP_Text text;
    Player player;
    DatabaseReference reference;
    PhotonView PV;

    void Start()
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;  //定義資料庫連接
        PV = this.gameObject.GetComponent<PhotonView>();  //定義PhotonView
    }
    public void SetUp(Player _player)  //初始化玩家名稱
    {

        player = _player;
        text.text = _player.NickName;

    }

    public override void OnPlayerLeftRoom(Player otherPlayer)  //當其他玩家離開房間
    {
        if (player == otherPlayer)
        {
            CleanPlayerUI();
        }
    }

    public override void OnLeftRoom()  //當自己離開房間
    {
        CleanPlayerUI();
        if (PV.IsMine)
        {
            if (PhotonNetwork.PlayerList.Length == 1)  //如果只剩自己 1 人
            {
                CleanAllDB();  //清除此房間的資料庫內容
            }
            else
            {
                CleanDB();  //否則只清除自己在資料庫的內容
            }

        }
        // MenuManger.Instance.OpenMenu("loading");
    }

    void CleanPlayerUI()  //清除玩家 UI
    {
        TeamSelect t = this.gameObject.GetComponent<TeamSelect>();
        t.blue = false;
        t.red = false;
        for (int i = 0; i < this.gameObject.transform.childCount; i++)  //關閉所有子物件
        {
            this.gameObject.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    void CleanDB()  //只清除自己在資料庫的內容
    {
        string roomName = Launcher.Instance.roomNameText.text;
        Debug.Log(this.gameObject.name);
        if (this.gameObject.name.Equals("P1Join"))
        {
            reference.Child("GameRoom").Child(roomName).Child("Order").Child("A").SetValueAsync("Empty");
        }
        else if (this.gameObject.name.Equals("P2Join"))
        {
            reference.Child("GameRoom").Child(roomName).Child("Order").Child("B").SetValueAsync("Empty");
        }
        else if (this.gameObject.name.Equals("P3Join"))
        {
            reference.Child("GameRoom").Child(roomName).Child("Order").Child("C").SetValueAsync("Empty");
        }
        else if (this.gameObject.name.Equals("P4Join"))
        {
            reference.Child("GameRoom").Child(roomName).Child("Order").Child("D").SetValueAsync("Empty");
        }

        reference.Child("GameRoom").Child(roomName).Child("PlayerList").Child(text.text).SetValueAsync(null);
    }

    void CleanAllDB()  //清除此房間的資料庫內容
    {
        string roomName = Launcher.Instance.roomNameText.text;
        reference.Child("GameRoom").Child(roomName).SetValueAsync(null);
    }
}

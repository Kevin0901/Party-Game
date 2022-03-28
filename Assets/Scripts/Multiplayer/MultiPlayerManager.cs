using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
public class MultiPlayerManager : MonoBehaviour
{
    PhotonView PV;
    [Header("玩家")]
    [SerializeField] GameObject controller;
    [Header("玩家隊伍")]
    public string Team;
    [Header("玩家名稱陣列")]
    public string[] _PlayerNames;
    [Header("玩家隊伍陣列")]
    public string[] _PlayerTeam;
    [Header("玩家 (其他)")]
    public GameObject OherPlayer;
    [Header("Room Manager")]
    public GameObject RoomManager;

    void Awake()
    {
        PV = GetComponent<PhotonView>();  //定義PhotonView
    }

    void Start()
    {
        if (PV.IsMine)  //如果是自己的 PlayerManager，生成自己
        {
            StartCoroutine(WaitRPCValue());
        }
        else  //否則生成其他玩家 (鏡像)
        {
            StartCoroutine(WaitOtherPlayer());
        }
    }

    public void GetValue()  //從 RoomManager 抓取 PlayerNames[] 跟 PlayerTeam[]
    {
        RoomManager = GameObject.Find("RoomManager").gameObject;
        _PlayerNames = RoomManager.GetComponent<RoomManager>().PlayerNames;
        _PlayerTeam = RoomManager.GetComponent<RoomManager>().PlayerTeam;
    }

    IEnumerator WaitRPCValue()  //等待 RPC_SetArryList() 完成
    {
        RoomManager = GameObject.Find("RoomManager").gameObject;
        if (RoomManager.GetComponent<RoomManager>().PlayerNames.Length != 0)  //如果完成，就生成自己
        {
            GetValue();
            CreatController();
        }
        else  //RPC 還沒完成的話，就持續呼叫 WaitRPCValue()
        {
            yield return new WaitForSeconds(0.3f);
            StartCoroutine(WaitRPCValue());
        }
    }

    IEnumerator WaitOtherPlayer()  //等待 OtherPlayer 生成完成
    {
        GetValue();
        if (OherPlayer != null)  //如果生成了
        {
            string name = OherPlayer.GetComponent<PlayerMovement>().PV.Owner.NickName;
            for (int i = 0; i < _PlayerNames.Length; i++)
            {
                if (name.Equals(_PlayerNames[i]))
                {
                    OherPlayer.tag = _PlayerTeam[i];  //給 Tag
                    OherPlayer.GetComponent<Team>().SetEnemy();  //設定敵方 Team 是什麼
                    OherPlayer.GetComponentInChildren<health>().healthBarSet();  //血量條設定
                }
            }
        }
        else  //還沒完成的話，就持續呼叫 WaitOtherPlayer()
        {
            yield return new WaitForSeconds(0.3f);
            StartCoroutine(WaitOtherPlayer());
        }
    }

    void CreatController()  //生成自己
    {
        controller = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Player&Camera"), this.transform.position, this.transform.rotation, 0, new object[] { PV.ViewID });
        GameObject player = controller.transform.Find("player").gameObject;
        for (int i = 0; i < _PlayerNames.Length; i++)
        {
            string name = player.GetComponent<PlayerMovement>().PV.Owner.NickName;
            if (name.Equals(_PlayerNames[i]))
            {
                player.tag = _PlayerTeam[i];  //給 Tag
                player.GetComponent<PlayerMovement>().order = (i + 1);
                player.GetComponent<PlayerMovement>().spawn();  //設定重生點
            }
        }
    }

    // public void Die()
    // {
    //     PhotonNetwork.Destroy(controller);
    //     CreatController();
    // }
}

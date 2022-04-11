using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.IO;
using System;
using System.Linq;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
public class RoomManager : MonoBehaviourPunCallbacks
{
    // public static RoomManager Instance;
    [Header("玩家名稱陣列")]
    public string[] PlayerNames;
    [Header("玩家隊伍陣列")]
    public string[] PlayerTeam;
    PhotonView PV;
    public int Game_num;
    void Awake()
    {
        // if (Instance)
        // {
        //     Destroy(gameObject);
        //     return;
        // }
        DontDestroyOnLoad(gameObject);
        PV = GetComponent<PhotonView>();  //定義PhotonView
        // Instance = this;
    }

    void Update()
    {
        if (!PV.IsMine)
        {
            return;
        }
        if (SceneManager.GetActiveScene().name.Equals("MainScene") && Input.inputString.Length > 0 && Input.inputString.All(char.IsDigit))
        {
            switch (Input.inputString)
            {
                case "0":
                    Game_num = 0;
                    break;
                case "1":
                    Game_num = 1;
                    break;
                case "2":
                    Game_num = 2;
                    break;
                case "3":
                    Game_num = 3;
                    break;
                case "4":
                    Game_num = 4;
                    break;
                case "5":
                    Game_num = 5;
                    break;
                case "6":
                    Game_num = 6;
                    break;
                case "7":
                    Game_num = 7;
                    break;
                case "8":
                    Game_num = 8;
                    break;
                case "9":
                    Game_num = 9;
                    break;
            }
            Hashtable hash = new Hashtable();
            hash.Add("GameNum", Game_num);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
            PhotonNetwork.LoadLevel(2);
        }
    }
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (!PV.IsMine)
        {
            Game_num = (int)changedProps["GameNum"];
        }
    }

    public override void OnEnable()
    {
        base.OnEnable();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.buildIndex == 1) //如果在遊戲場景
        {
            if (PV.IsMine)  //只有房主的 RoomManager 執行以下程式碼
            {
                PV.RPC("RPC_SetArryList", RpcTarget.All, PlayerNames, PlayerTeam);  //廣播到所有玩家的電腦，設定 PlayerNames[] 跟 PlayerTeam[]
            }
        }
        else if (scene.buildIndex == 2)
        {
            if (PV.IsMine)
            {
                for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
                {
                    PhotonNetwork.RemoveRPCs(PhotonNetwork.PlayerList[i]);
                }
                GameObject RM = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "arena/FightManager"), Vector3.zero, Quaternion.identity);

            }
        }
    }

    [PunRPC]  //廣播的方法前面都需要加這個開頭
    void RPC_SetArryList(string[] PN, string[] PT)  //設定 PlayerNames[] 跟 PlayerTeam[]
    {
        if (PV.IsMine)  //如果是房主的 RoomManager，直接 Skip
        {
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerManager"), Vector3.zero, Quaternion.identity);
            return;
        }
        PlayerNames = PN;
        PlayerTeam = PT;
        //每個玩家實例化自己的 PlayerManager
        GameObject Manager = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerManager"), Vector3.zero, Quaternion.identity);
    }
}

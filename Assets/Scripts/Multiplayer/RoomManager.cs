using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.IO;
using Photon.Realtime;
public class RoomManager : MonoBehaviourPunCallbacks
{
    // public static RoomManager Instance;
    [Header("玩家名稱陣列")]
    public string[] PlayerNames;
    [Header("玩家隊伍陣列")]
    public string[] PlayerTeam;
    PhotonView PV;
    int Gnum;
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
        if (PV.IsMine && SceneManager.GetActiveScene().name.Equals("MainScene"))
        {
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                Gnum = 0;
                PhotonNetwork.LoadLevel(2);
            }
            else if(Input.GetKeyDown(KeyCode.Alpha1))
            {
                Gnum = 1;
                PhotonNetwork.LoadLevel(2);
            }
            else if(Input.GetKeyDown(KeyCode.Alpha2))
            {
                Gnum = 2;
                PhotonNetwork.LoadLevel(2);
            }
            else if(Input.GetKeyDown(KeyCode.Alpha3))
            {
                Gnum = 3;
                PhotonNetwork.LoadLevel(2);
            }
            else if(Input.GetKeyDown(KeyCode.Alpha4))
            {
                Gnum = 4;
                PhotonNetwork.LoadLevel(2);
            }
            else if(Input.GetKeyDown(KeyCode.Alpha5))
            {
                Gnum = 5;
                PhotonNetwork.LoadLevel(2);
            }
            else if(Input.GetKeyDown(KeyCode.Alpha6))
            {
                Gnum = 6;
                PhotonNetwork.LoadLevel(2);
            }
            else if(Input.GetKeyDown(KeyCode.Alpha7))
            {
                Gnum = 7;
                PhotonNetwork.LoadLevel(2);
            }
            else if(Input.GetKeyDown(KeyCode.Alpha8))
            {
                Gnum = 8;
                PhotonNetwork.LoadLevel(2);
            }
            else if(Input.GetKeyDown(KeyCode.Alpha9))
            {
                Gnum = 9;
                PhotonNetwork.LoadLevel(2);
            }
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
        if (scene.name.Equals("MainScene")) //如果在遊戲場景
        {
            if (PV.IsMine)  //只有房主的 RoomManager 執行以下程式碼
            {
                PV.RPC("RPC_SetArryList", RpcTarget.All, PlayerNames, PlayerTeam);  //廣播到所有玩家的電腦，設定 PlayerNames[] 跟 PlayerTeam[]
            }
        }
        else if (scene.name.Equals("FightScene"))
        {
            if (PV.IsMine)
            {
                Player[] players = PhotonNetwork.PlayerList;  //取得已加入房間的玩家陣列
                for (int i = 0; i < players.Length; i++)
                {
                    PhotonNetwork.RemoveRPCs(players[i]);
                }
                GameObject FM = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "arena/FightManager"), Vector3.zero, Quaternion.identity);
                FM.GetComponent<FightManager>().game_num = Gnum;
                // PV.RPC("RPC_SetFightManager", RpcTarget.All);
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

    // [PunRPC]
    // void RPC_SetFightManager()
    // {
    //     for (int i = 0; i < PlayerTeam.Length; i++)
    //     {
    //         if (PlayerTeam[i].Equals("red"))
    //         {
    //             Debug.Log("true");
    //             FightManager.Instance.redOrBlue.Add(true);
    //         }
    //         else
    //         {
    //             Debug.Log("false");
    //             FightManager.Instance.redOrBlue.Add(false);
    //         }
    //     }
    // }
}

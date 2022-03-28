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

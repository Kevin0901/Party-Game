using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.IO;
using Firebase.Database;
using System;
public class HermesEvent : MonoBehaviour
{
    //生成點 x=55 , y=33
    [SerializeField] private GameObject Cow, goldCow, UI;
    private float redScore, blueScore;
    [SerializeField] private int GameTime;
    [SerializeField] private float Normal_mix, Normal_max, Gold_mix, Gold_max;
    [SerializeField] GameObject StartButton;
    [SerializeField] GameObject UIBackGround;
    PhotonView PV;
    bool isEnd = false;
    DatabaseReference reference;
    void Start()
    {
        PV = GetComponent<PhotonView>();
        reference = FirebaseDatabase.DefaultInstance.RootReference;  //定義資料庫連接
        if (!PhotonNetwork.IsMasterClient)
        {
            StartButton.SetActive(false);
        }
    }
    public void StartGame()
    {
        PV.RPC("RPC_StartGame", RpcTarget.All);
    }
    [PunRPC]
    public void RPC_StartGame()
    {
        UIBackGround.SetActive(false);
        for (int i = 0; i < FightManager.Instance.plist.Count; i++)
        {
            FightManager.Instance.plist[i].GetComponent<arenaPlayer>().currentState = ArenaState.fastMode;
        }
        if (PV.IsMine)
        {
            StartCoroutine(spawnCow(UnityEngine.Random.Range(Normal_mix, Normal_max)));
            StartCoroutine(spawnGoldCow(UnityEngine.Random.Range(Gold_mix, Gold_max)));
            reference.Child("GameRoom").Child(PhotonNetwork.CurrentRoom.Name).Child("Arena").Child("HermesScore").Child("red").SetValueAsync(0);
            reference.Child("GameRoom").Child(PhotonNetwork.CurrentRoom.Name).Child("Arena").Child("HermesScore").Child("blue").SetValueAsync(0);
        }
        StartCoroutine(timeCount());
        StartCoroutine(Get_Score_From_Database());
    }
    private void Update()
    {
        if (GameTime <= 0 && !isEnd)
        {
            StopAllCoroutines();
            StartCoroutine(endGame());
            isEnd = true;
        }
    }

    IEnumerator Get_Score_From_Database()
    {
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(GetScoreInfo((DataSnapshot info) =>  //從資料庫抓取此房間內的所有資料
        {
            foreach (var Team in info.Children)
            {
                if(Team.Key.Equals("red"))
                {
                    redScore = (int)Int64.Parse(Team.Value.ToString());
                    this.transform.Find("GameUI").Find("red").GetComponent<Text>().text = "Score:" + redScore.ToString();
                }
                else if(Team.Key.Equals("blue"))
                {
                    blueScore = (int)Int64.Parse(Team.Value.ToString());
                    this.transform.Find("GameUI").Find("blue").GetComponent<Text>().text = "Score:" + blueScore.ToString();
                }
            }
            StartCoroutine(Get_Score_From_Database());
        }));
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

    IEnumerator endGame()
    {
        for (int i = 0; i < FightManager.Instance.plist.Count; i++)
        {
            FightManager.Instance.plist[i].GetComponent<arenaPlayer>().currentState = ArenaState.idle;
        }
        this.transform.Find("GameUI").Find("end").GetComponent<Text>().text =
        "遊戲結束\n" + blueScore.ToString() + "  :  " + redScore.ToString();
        yield return new WaitForSeconds(3f);
        UI.SetActive(true);
        if (redScore > blueScore)
        {
            UI.transform.Find("red").gameObject.SetActive(true);
        }
        else if ((redScore < blueScore))
        {
            UI.transform.Find("blue").gameObject.SetActive(true);
        }
        else if ((redScore == blueScore))
        {
            UI.transform.Find("draw").gameObject.SetActive(true);
        }
        this.gameObject.SetActive(false);
    }
    private IEnumerator spawnCow(float time)
    {
        yield return new WaitForSeconds(time);
        float x = UnityEngine.Random.Range(-55f, 55f);
        float y = UnityEngine.Random.Range(-33f, 33f);
        GameObject a = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "arena/Hermes/cow"), new Vector3(x, y, 0), this.transform.rotation, 0, new object[] { PV.ViewID });
        a.transform.parent = this.transform;
        // a.GetComponent<pickcow>().cowScore = 1;
        StartCoroutine(spawnCow(UnityEngine.Random.Range(Normal_mix, Normal_max)));
    }
    private IEnumerator spawnGoldCow(float time)
    {
        yield return new WaitForSeconds(time);
        float x = UnityEngine.Random.Range(-55f, 55f);
        float y = UnityEngine.Random.Range(-33f, 33f);
        GameObject a = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "arena/Hermes/goldcow"), new Vector3(x, y, 0), this.transform.rotation, 0, new object[] { PV.ViewID });
        a.transform.parent = this.transform;
        // a.GetComponent<pickcow>().cowScore = 3;
        StartCoroutine(spawnGoldCow(UnityEngine.Random.Range(Gold_mix, Gold_max)));
    }
    public void R_ScoreADD(int point)
    {
        redScore += point;
        this.transform.Find("GameUI").Find("red").GetComponent<Text>().text = "Score:" + redScore.ToString();
    }
    public void B_ScoreADD(int point)
    {
        blueScore += point;
        this.transform.Find("GameUI").Find("blue").GetComponent<Text>().text = "Score:" + blueScore.ToString();
    }
    public IEnumerator timeCount()
    {
        yield return new WaitForSeconds(1f);
        this.transform.Find("GameUI").Find("time").GetComponent<Text>().text = (--GameTime).ToString();
        StartCoroutine(timeCount());
    }
}

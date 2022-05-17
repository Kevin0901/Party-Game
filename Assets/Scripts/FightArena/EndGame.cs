using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class EndGame : MonoBehaviour
{
    PhotonView PV;
    GameObject PW;
    [SerializeField] TMP_Text Time;
    int countdown = 5;
    public string WinTeam;
    void Start()
    {
        PV = GetComponent<PhotonView>();
        PW = transform.Find("PlayerWin").gameObject;
    }
    //啟用此物件
    private void OnEnable()
    {
        this.GetComponent<CanvasGroup>().alpha = 1;
        this.GetComponent<CanvasGroup>().blocksRaycasts = true;
        // PW.GetComponent<TMP_Text>().text = FightManager.Instance.plist[0].GetComponent<arenaPlayer>().p_index.ToString() + " P WIN！";
        // PW.SetActive(true);
        Time.gameObject.SetActive(true);
        StartCoroutine(CountDown());
    }
    IEnumerator CountDown()
    {
        if (countdown <= 0)
        {
            if(transform.Find("red").gameObject.activeSelf)
            {
                WinTeam = "red";
            }
            else if(transform.Find("blue").gameObject.activeSelf)
            {
                WinTeam = "blue";
            }else{
                WinTeam = "NO";
            }
            GameObject.Find("RoomManager").GetComponent<RoomManager>().StartCoroutine("Black_fadein");
        }
        else
        {
            yield return new WaitForSeconds(1f);
            countdown--;
            Time.text = countdown.ToString();
            StartCoroutine(CountDown());
        }
    }
    //重新這個遊戲(點擊事件)
    public void ReStart()
    {
        FightManager.Instance.gameNum();
    }
    //離開遊戲(點擊事件)
    public void End()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
            Application.Quit();
#endif
    }
    //換遊戲(點擊事件)
    public void changeGame()
    {
        // GameObject.Find("GameChoose").GetComponent<CanvasGroup>().alpha = 1;
        // GameObject.Find("GameChoose").GetComponent<CanvasGroup>().blocksRaycasts = true;
        // this.GetComponent<CanvasGroup>().alpha = 0;
        // this.GetComponent<CanvasGroup>().blocksRaycasts = false;
        if (PV.IsMine)
        {
            PhotonNetwork.LoadLevel(1);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class SunMoonEvent : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float size = 0.05f;
    [SerializeField] private GameObject UI;
    [SerializeField] GameObject StartButton;
    [SerializeField] GameObject UIBackGround;
    PhotonView PV;
    void Start()
    {
        PV = GetComponent<PhotonView>();
        if (!PhotonNetwork.IsMasterClient)
        {
            StartButton.SetActive(false);
        }
    }
    //開始遊戲
    public void StartGame()
    {
        PV.RPC("RPC_StartGame",RpcTarget.All);
    }
    [PunRPC]
    public void RPC_StartGame()
    {
        UIBackGround.SetActive(false);
        StartCoroutine(changeBG());
        for (int i = 0; i < FightManager.Instance.plist.Count; i++)
        {
            FightManager.Instance.plist[i].GetComponent<arenaPlayer>().currentState = ArenaState.shoot;
        }
    }
    //改變背景大小
    private IEnumerator changeBG()
    {
        if (this.transform.localScale.x != 2)
        {
            this.transform.localScale = new Vector2(this.transform.localScale.x - Time.deltaTime * size,
                                                    this.transform.localScale.y - Time.deltaTime * size);
        }
        yield return null;
        StartCoroutine(changeBG());
    }
    //直到玩家剩一位
    private void Update()
    {
        if (FightManager.Instance.plist.Count <= 1)
        {
            UI.SetActive(true);
            if (FightManager.Instance.plist[0].GetComponent<arenaPlayer>().red)
            {
                UI.transform.Find("red").gameObject.SetActive(true);
            }
            else
            {
                UI.transform.Find("blue").gameObject.SetActive(true);
            }
            this.transform.gameObject.SetActive(false);
        }
    }
    //玩家離開地圖判定
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == 10)
        {
            other.gameObject.SetActive(false);
        }
    }
}

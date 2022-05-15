using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
public class sword : MonoBehaviour
{
    private GameObject lastplayer; //上一個玩家
    private arenaPlayer player;
    [HideInInspector] public float hurtTime;
    [HideInInspector] public float waitTime;
    private float nexthurt;
    private int num;
    void Awake()
    {
        player = this.GetComponentInParent<arenaPlayer>();
        num = player.p_index;
    }

    //如果劍碰到玩家的話，判斷不是自己 & 不是上一個傳的對象
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == 10 && (other.gameObject != player.gameObject) && (lastplayer != other.gameObject))
        {
            other.gameObject.transform.Find("sword").gameObject.SetActive(true);
            other.gameObject.transform.Find("sword").gameObject.GetComponent<sword>().StartCoroutine("Savelastplayer", player.gameObject);
            player.StartCoroutine("changeColorTitle_Sword");

            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "Sword/SwordChange"),
            Vector3.zero, this.transform.rotation, 0, new object[] { this.GetComponentInParent<PhotonView>().ViewID, other.gameObject.GetComponent<PhotonView>().ViewID });
            this.gameObject.SetActive(false);
        }
    }
    //如果劍開啟
    private void OnEnable()
    {
        player.speed *= 1.5f;
        //改變Title顏色
        player.titleColor.color = new Color32(255, 28, 28, 255);
        nexthurt = Time.time + hurtTime;
    }
    //如果劍關閉
    private void OnDisable()
    {
        player.speed /= 1.5f;
    }
    private void Update()
    {
        //如果劍在自己身上等到傷害時間
        if (Time.time > nexthurt)
        {
            player.hurt(0.5f);
            nexthurt = Time.time + hurtTime;
        }
    }
    //在被碰到劍的玩家上，記錄上一個玩家
    public IEnumerator Savelastplayer(GameObject p)
    {
        lastplayer = p;
        yield return new WaitForSeconds(waitTime);
        lastplayer = null;
    }
}

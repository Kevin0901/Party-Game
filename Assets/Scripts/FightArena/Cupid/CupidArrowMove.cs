using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupidArrowMove : MonoBehaviour
{
    public float speed = 20;
    public Vector3 Pos;
    [HideInInspector]public GameObject parent;
    void Update()
    {
        if (FightManager.Instance.plist.Count == 1)
        {
            this.gameObject.SetActive(false);
        }
        //箭矢射擊方向
        transform.position += Pos.normalized * Time.deltaTime * speed;
    }
    //碰撞設定
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 10)
        {
            arenaPlayer curPlayer = other.GetComponent<arenaPlayer>();
            //隨機追蹤玩家
            int r = Random.Range(0, FightManager.Instance.plist.Count);
            //如果等於自己或是玩家剩一位的話
            while (r == curPlayer.p_index)
            {
                if (FightManager.Instance.plist.Count == 1)
                {
                    break;
                }
                r = Random.Range(0, FightManager.Instance.plist.Count);
            }
            //給玩家設定
            curPlayer.love_index = r;
            curPlayer.transform.Find("NumTitle").GetChild(curPlayer.p_index).GetComponent<SpriteRenderer>().color = new Color32(255, 0, 255, 255);
            curPlayer.currentState = ArenaState.love;
            //回去物件池
            parent.GetComponent<CupidEvent>().BackToPool(this.gameObject);
        }
    }
    //碰撞設定
    private void OnTriggerExit2D(Collider2D other)
    {
        //碰到背景
        if (other.CompareTag("background") && this.gameObject.activeSelf)
        {
            //回去物件池
            parent.GetComponent<CupidEvent>().BackToPool(this.gameObject);
        }
    }
}

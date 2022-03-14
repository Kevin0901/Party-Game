using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupidArrowMove : MonoBehaviour
{
    private float damage = 0.5f;
    public float speed = 20;
    public Vector3 Pos;
    public GameObject parent;
    void Update()
    {
        if (FightManager.Instance.gamelist.Count == 1)
        {
            this.gameObject.SetActive(false);
        }
        //箭矢射擊方向
        transform.position += Pos.normalized * Time.deltaTime * speed;
    }
    //碰撞設定
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 10 && other.GetComponent<arenaPlayer>().currentState == ArenaState.walk)
        {
            //隨機追蹤玩家
            int r = Random.Range(0, FightManager.Instance.gamelist.Count);
            //如果等於自己或是玩家剩一位的話
            while (r == other.GetComponent<arenaPlayer>().p_index)
            {
                if (FightManager.Instance.gamelist.Count == 1)
                {
                    break;
                }
                r = Random.Range(0, FightManager.Instance.gamelist.Count);
            }
            //給玩家設定
            other.GetComponent<arenaPlayer>().love_index = r;
            other.gameObject.GetComponent<arenaPlayer>().currentState = ArenaState.love;
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

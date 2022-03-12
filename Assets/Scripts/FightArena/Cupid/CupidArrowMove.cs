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
        transform.position += Pos.normalized * Time.deltaTime * speed;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 10 && other.GetComponent<arenaPlayer>().currentState == ArenaState.walk)
        {
            int r = Random.Range(0, FightManager.Instance.gamelist.Count);
            while (r == other.GetComponent<arenaPlayer>().p_index || FightManager.Instance.gamelist.Count == 0)
            {
                r = Random.Range(0, FightManager.Instance.gamelist.Count);
            }
            other.GetComponent<arenaPlayer>().love_index = r;
            other.gameObject.GetComponent<arenaPlayer>().currentState = ArenaState.love;
            parent.GetComponent<CupidEvent>().BackToPool(this.gameObject);
        }
        // else if (other.CompareTag("wall"))
        // {
        //     this.gameObject.GetComponentInParent<CupidEvent>().BackToPool(this.gameObject);
        // }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("background") && this.gameObject.activeSelf)
        {
            parent.GetComponent<CupidEvent>().BackToPool(this.gameObject);
        }
    }
}

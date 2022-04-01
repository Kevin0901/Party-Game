using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ball : MonoBehaviour
{
    [HideInInspector] public float speed, damage;
    private Rigidbody2D rb;
    private float cnt; //速度計數器
    void Start()
    {
        cnt = 1.5f;
        rb = this.GetComponent<Rigidbody2D>();
    }
    //旋轉跟刪除物件
    void FixedUpdate()
    {
        this.transform.Rotate(0, 0, 3f);
        if (FightManager.Instance.plist.Count <= 1)
        {
            Destroy(this.gameObject);
        }
        else if (Vector3.Distance(Vector3.zero, this.transform.position) > 45)
        {
            Destroy(this.gameObject);
        }
    }
    //球的移動方向
    public void move(int num)
    {
        Vector2 pos = (FightManager.Instance.plist[num].transform.position - this.transform.position).normalized;
        rb.AddForce(pos * speed, ForceMode2D.Impulse);
    }
    //碰撞判定
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == 10)
        {
            other.gameObject.GetComponent<arenaPlayer>().hurt(damage);
        }
        else if (other.gameObject.tag == "pick")
        {
            other.gameObject.SetActive(false);
            other.gameObject.GetComponent<Spawnshield>().spawnS();
        }
        //撞到任何物體都會加速
        rb.velocity = rb.velocity.normalized * speed * cnt;
        if (cnt <= 2.5f)
        {
            cnt += 0.5f;
        }
    }
}

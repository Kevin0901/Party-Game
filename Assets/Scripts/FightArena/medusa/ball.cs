using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ball : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float damage = 1;
    private Rigidbody2D rb;
    private float cnt;
    void Awake()
    {
        cnt = 1.5f;
    }
    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(0, 0, 3f);
        if (FightManager.Instance.gamelist.Count == 1)
        {
            Destroy(this.gameObject);
        }
        else if (Vector3.Distance(Vector3.zero, this.transform.position) > 45)
        {
            Destroy(this.gameObject);
        }
    }
    public void move(int num)
    {
        rb = this.GetComponent<Rigidbody2D>();
        Vector2 pos = (FightManager.Instance.gamelist[num].transform.position - this.transform.position).normalized;
        rb.AddForce(pos * speed, ForceMode2D.Force);
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == 10)
        {
            other.gameObject.GetComponent<arenaPlayer>().hurt(damage);
        }
        else if (other.gameObject.tag == "pick")
        {
            other.gameObject.SetActive(false);
        }
        //撞到任何物體都會加速
        rb.velocity = rb.velocity.normalized * speed * cnt;
        if (cnt <= 2.5f)
        {
            cnt += 0.5f;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ball : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float damage;
    private Rigidbody2D rb;
    private arenaController game;
    private float cnt;
    // Start is called before the first frame update
    void Awake()
    {
        cnt = 1.5f;
    }
    private void Start()
    {
        game = GameObject.Find("FightGameManager").GetComponent<arenaController>();
    }
    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(0, 0, 3f);
        if (Vector3.Distance(Vector3.zero, this.transform.position) > 45)
        {
            Destroy(this.gameObject);
        }
        else if (game.isover)
        {
            Destroy(this.gameObject);
        }
    }
    public void launch(Vector2 pos)
    {
        Vector2 k;
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        k = -(new Vector2(this.transform.position.x, this.transform.position.y) - pos).normalized;
        rb.AddForce(k * speed, ForceMode2D.Force);
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
        rb.velocity = rb.velocity.normalized * speed * cnt;
        if (cnt <= 2.5f)
        {
            cnt += 0.5f;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CupidArrowMove : MonoBehaviour
{
    private float damage = 0.5f;
    public float speed = 20;
    public Vector3 Pos;
    void Update()
    {
        transform.position += Pos.normalized * Time.deltaTime * speed;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 10)
        {
            // other.gameObject.GetComponent<arenaPlayer>().hurt(damage);
            other.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            other.gameObject.GetComponent<arenaPlayer>().currentState = ArenaState.dizzy;
            other.gameObject.GetComponent<arenaPlayer>().dizzyT = 0.15f;
            this.gameObject.GetComponentInParent<Cupid>().BackToPool(this.gameObject);
        }
        else if (other.CompareTag("wall"))
        {
            this.gameObject.GetComponentInParent<Cupid>().BackToPool(this.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("background") && this.gameObject.activeSelf)
        {
            this.gameObject.GetComponentInParent<Cupid>().BackToPool(this.gameObject);
        }
    }
}

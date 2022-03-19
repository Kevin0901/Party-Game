using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class football : MonoBehaviour
{
    private Rigidbody2D mrigidbody2d;
    [SerializeField] private float maxSpeed;
    public GameObject parent;
    private void Start()
    {
        mrigidbody2d = this.GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        if (mrigidbody2d.velocity.magnitude > maxSpeed)
        {
            mrigidbody2d.velocity = mrigidbody2d.velocity.normalized * maxSpeed;
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("red"))
        {
            parent.GetComponent<FootEvent>().red_Score();
            StartCoroutine(waitBall());
        }
        else if (other.gameObject.CompareTag("blue"))
        {
            parent.GetComponent<FootEvent>().blue_Score();
            StartCoroutine(waitBall());
        }
    }
    IEnumerator waitBall()
    {
        mrigidbody2d.velocity = Vector2.zero;
        yield return new WaitForSeconds(2.5f);
        Destroy(this.gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class football : MonoBehaviour
{
    private Rigidbody2D mrigidbody2d;
    [HideInInspector] public float maxSpeed = 85;
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
    public IEnumerator waitBall()
    {
        yield return new WaitForSeconds(0.3f);
        mrigidbody2d.velocity = Vector2.zero;
        yield return new WaitForSeconds(2.5f);
        Destroy(this.gameObject);
    }
}

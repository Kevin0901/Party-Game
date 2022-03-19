using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fist : MonoBehaviour
{
    private Vector3 dir;
    private Rigidbody2D mrigidbody2D;
    private void Awake()
    {
        Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), this.transform.parent.GetComponent<Collider2D>());
    }
    private void Start()
    {
        mrigidbody2D = this.GetComponent<Rigidbody2D>();
    }
    public void punch(float power)
    {
        Quaternion k = Quaternion.AngleAxis(transform.rotation.eulerAngles.z, Vector3.forward);
        dir = k * Vector3.up;
        mrigidbody2D.velocity = dir.normalized * power;
    }
    private void LateUpdate()
    {
        this.transform.rotation = transform.parent.rotation;
    }
}

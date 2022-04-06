using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tsunami : MonoBehaviour
{
    public float wavespeed;
    private List<GameObject> enemylist;
    public float damage;

    private void Start()
    {
        enemylist = new List<GameObject>();
        enemylist.Add(this.gameObject);
    }

    void Update()
    {
        transform.position += new Vector3(wavespeed, 0, 0) * Time.deltaTime;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == 9)
        {
            health eheal = other.GetComponentInChildren<health>();
            eheal.Hurt((int)damage);
            other.transform.position += new Vector3(wavespeed / 2, 0, 0) * Time.deltaTime;
        }

        if (other.gameObject.layer == 11)
        {
            health eheal = other.GetComponentInChildren<health>();
            if (eheal.curH <= eheal.maxH * 0.3)
            {
                Destroy(other.gameObject);
            }
        }
    }
}

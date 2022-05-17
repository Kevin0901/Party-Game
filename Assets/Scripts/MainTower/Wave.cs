using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{

    public float wavespeed;

    private List<GameObject> enemylist;
    public Team t;
    public float damage;

    private void Start()
    {
        enemylist = new List<GameObject>();
        enemylist.Add(this.gameObject);
    }

    void Update()
    {
        transform.position += new Vector3(0, wavespeed, 0) * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(t.Enemyteam)&& other.gameObject.layer !=11 )
        {
            int cnt = 0;
            health eheal = other.GetComponentInChildren<health>();
            foreach (GameObject n in enemylist)
            {
                if (other.gameObject != null && n != null)
                {

                    if (other.gameObject.name == n.name)
                    {
                        cnt += 1;
                    }
                }

            }
            if (cnt == 0)
            {
                eheal.Hurt((int)damage);
                enemylist.Add(other.gameObject);
            }
            other.transform.position += new Vector3(0, wavespeed * 2, 0) * Time.deltaTime;
        }
    }


}

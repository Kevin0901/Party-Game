using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    private List<GameObject> enemyinlist = new List<GameObject>();
    private int candestory = 0;
    public string enemy;
    public int sec, secdamage;

    void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
        if (candestory != 0)
        {
            HurtEnemyInRange();
            Destroy(this.gameObject);
        }

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(enemy))
        {
            enemyinlist.Add(other.gameObject);
            candestory = 1;
        }
    }

    void HurtEnemyInRange()
    {
        foreach (GameObject enemy in enemyinlist)
        {
            enemy.GetComponent<Effect>().StartCoroutine("BurnEffect");
        }
    }


}

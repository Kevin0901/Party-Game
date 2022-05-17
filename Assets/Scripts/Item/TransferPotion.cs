using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransferPotion : MonoBehaviour
{
    public string enemy;
    private List<GameObject> enemyinlist = new List<GameObject>();
    private int candestory = 0;
    void Update()
    {
        if (candestory != 0)
        {
            TransferEnemyInRange();
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

    void TransferEnemyInRange()
    {
        foreach (GameObject enemy in enemyinlist)
        {
            if (enemy.layer == 9)
            {
                enemy.GetComponent<monsterMove>().StartCoroutine("PigeonChange");
            }
        }
    }
}

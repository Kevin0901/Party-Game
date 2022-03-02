using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medusaeye : MonoBehaviour
{
    private List<GameObject> enemyinlist = new List<GameObject>();
    private int candestory = 0;
    public string enemy;
    // Update is called once per frame
    void Update()
    {

        if (candestory != 0)
        {
            BecomeStone();
            Destroy(this.gameObject);
        }

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        enemy = this.GetComponent<item_MoveMovementGamepad>().team;
        if (other.CompareTag(enemy))
        {
            enemyinlist.Add(other.gameObject);
            candestory = 1;
        }
    }

    void BecomeStone()
    {
        foreach (GameObject enemy in enemyinlist)
        {
            if (enemy.GetComponent<monsterMove>() != null)
            {
                StartCoroutine(enemy.GetComponent<monsterMove>().MonsterStartStone());
            }
            else if (enemy.GetComponent<PlayerMovement>() != null)
            {
                StartCoroutine(enemy.GetComponent<PlayerMovement>().StoneEffect());
            }
        }
    }
}

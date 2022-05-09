using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldShoot : MonoBehaviour
{
    public List<GameObject> enemiesInRange;
    private Team t;
    private float lastShotTime;
    private TowerData towerData;

    // Use this for initialization
    void Start()
    {
        t = GetComponent<Team>();
        enemiesInRange = new List<GameObject>();
        lastShotTime = Time.time;
        towerData = gameObject.GetComponentInChildren<TowerData>();
    }

    // Update is called once per frame
    void Update()
    {
        GameObject target = null;
        // 1
        float minimalEnemyDistance = float.MaxValue;
        foreach (GameObject enemy in enemiesInRange)
        {

            float distanceToGoal = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToGoal < minimalEnemyDistance)
            {
                target = enemy;
                minimalEnemyDistance = distanceToGoal;
            }
        }
        // 2
        if (target != null)
        {
            if (Time.time - lastShotTime > towerData.CurrentLevel.fireRate)
            {
                Shoot(enemiesInRange);
                lastShotTime = Time.time;
            }
            // 3
            Vector3 direction = gameObject.transform.position - target.transform.position;
        }
    }

    private void OnEnemyDestroy(GameObject enemy)
    {
        enemiesInRange.Remove(enemy);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(t.Enemyteam) && other.gameObject.layer != 5)
        {
            enemiesInRange.Add(other.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(t.Enemyteam) && other.gameObject.layer != 5)
        {
            enemiesInRange.Remove(other.gameObject);
        }
    }

    private void Shoot(List<GameObject> enemiesInRange)
    {
        // List<GameObject> nenemy = enemiesInRange;
        // HealthBar targetheal;
        // foreach (GameObject n in nenemy)
        // {
        //     targetheal = n.GetComponentInChildren<HealthBar>();
        //     targetheal.currentHealth -= Mathf.Max(20, 0);
        //     if (targetheal.currentHealth <= 0)
        //     {
        //         Destroy(n);
        //         break;
        //     }
        // }
    }
}

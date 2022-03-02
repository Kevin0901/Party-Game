using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootEnemy : MonoBehaviour
{
    [SerializeField] private List<GameObject> enemiesInRange;
    private Animator animator;
    private float lastShotTime;
    private TowerData towerData;

    private Team t;

    [SerializeField] private GameObject bullet;
    [SerializeField] private float BulletSpeed;


    void Start()
    {
        t = GetComponent<Team>();
        animator = GetComponent<Animator>();
        enemiesInRange = new List<GameObject>();
        lastShotTime = Time.time;
        towerData = gameObject.GetComponentInChildren<TowerData>();
    }


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

                StartCoroutine(attackcold(target.GetComponentInChildren<Collider2D>()));
                lastShotTime = Time.time;
            }
        }
    }

    private IEnumerator attackcold(Collider2D target)
    {
        animator.SetBool("attack", true);
        yield return null;
        Shoot(target);
        animator.SetBool("attack", false);

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

    private void Shoot(Collider2D target)
    {

        // 1 
        Vector3 startPosition = gameObject.transform.position;
        Vector3 targetPosition = target.transform.position;
        startPosition.z = bullet.transform.position.z;
        targetPosition.z = bullet.transform.position.z;

        // 2 
        GameObject newBullet = (GameObject)Instantiate(bullet);
        newBullet.transform.position = startPosition;
        BulletBehavior bulletComp = newBullet.GetComponent<BulletBehavior>();
        bulletComp.target = target.gameObject;
        bulletComp.startPosition = startPosition;
        bulletComp.targetPosition = targetPosition;
        bulletComp.damage = towerData.CurrentLevel.damage;
        bulletComp.speed = BulletSpeed;
    }
}

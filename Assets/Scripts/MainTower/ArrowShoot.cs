using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowShoot : MonoBehaviour
{
    [SerializeField] private List<GameObject> enemiesInRange;
    private Animator animator;
    private float lastShotTime;
    public TowerData towerData;

    private Team t;

    [SerializeField] private GameObject Arrow;
    [SerializeField] private float ArrowSpeed;
    private bool issave;
    private void Awake()
    {
        t = this.GetComponent<Team>();
        t.SetEnemy();
    }

    void Start()
    {
        this.transform.SetParent(GameObject.Find("PAPA").transform);
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
                if (t.Enemyteam == "red")
                {
                    StartCoroutine(BlueAttackCold(target.GetComponentInChildren<Collider2D>()));
                    lastShotTime = Time.time;
                }
                else
                {
                    StartCoroutine(RedAttackCold(target.GetComponentInChildren<Collider2D>()));
                    lastShotTime = Time.time;
                }
            }
        }
    }

    private IEnumerator BlueAttackCold(Collider2D target)
    {
        animator.SetBool("BlueAttack", true);
        yield return new WaitForSeconds(0.2f);
        animator.SetBool("BlueAttack", false);
        Shoot(target);
    }

    private IEnumerator RedAttackCold(Collider2D target)
    {
        animator.SetBool("RedAttack", true);
        yield return new WaitForSeconds(0.2f);
        animator.SetBool("RedAttack", false);
        Shoot(target);

    }

    void OnTriggerEnter2D(Collider2D other){
        t = GetComponent<Team>();
        if (other.CompareTag(t.Enemyteam) && other.gameObject.layer != 11)
        {
            enemiesInRange.Add(other.gameObject);
        }
    }

    // void OnTriggerStay2D(Collider2D other)
    // {
    //     t = GetComponent<Team>();
    //     if (other.CompareTag(t.Enemyteam) && other.gameObject.layer != 11)
    //     {
    //         if (enemiesInRange.Count == 0)
    //         {
    //             enemiesInRange.Add(other.gameObject);
    //         }
    //         else
    //         {
    //             for (int i = 0; i < enemiesInRange.Count; i++)
    //             {
    //                 if (enemiesInRange[i].name == other.gameObject.name)
    //                 {
    //                     issave = true;
    //                     break;
    //                 }
    //             }
    //             if (!issave)
    //             {
    //                 enemiesInRange.Add(other.gameObject);
    //                 issave = false;
    //             }
    //         }
    //     }
    // }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(t.Enemyteam))
        {
            Debug.Log(other.gameObject);
            enemiesInRange.Remove(other.gameObject);
        }
    }

    private void Shoot(Collider2D target)
    {
        if (target != null)
        {
            Vector3 startPosition = gameObject.transform.position;
            Vector3 targetPosition = target.transform.position;
            startPosition.z = Arrow.transform.position.z;
            targetPosition.z = Arrow.transform.position.z;

            GameObject arrowbh = (GameObject)Instantiate(Arrow);

            arrowbh.transform.position = startPosition;
            ArrowBehavior arrowComp = arrowbh.GetComponent<ArrowBehavior>();
            arrowComp.target = target.gameObject;
            arrowComp.startPosition = startPosition;
            arrowComp.targetPosition = targetPosition;
            arrowComp.damage = towerData.CurrentLevel.damage;
            arrowComp.speed = ArrowSpeed;
        }
    }
}

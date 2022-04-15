using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class MagicShoot : MonoBehaviour
{
    [SerializeField] private List<GameObject> enemiesInRange;
    private Team t;
    private float lastShotTime;
    private TowerData towerData;
    private Animator animator;
    PhotonView PV;
    private void Awake()
    {
        PV = GetComponent<PhotonView>();  //定義PhotonView
        this.gameObject.tag = PhotonView.Find((int)PV.InstantiationData[0]).tag;
        t = this.GetComponent<Team>();
    }

    // Use this for initialization
    void Start()
    {
        enemiesInRange = new List<GameObject>();
        animator = GetComponent<Animator>();
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
                attackteamchange(enemiesInRange);
                lastShotTime = Time.time;
            }
            // 3
            Vector3 direction = gameObject.transform.position - target.transform.position;
        }
    }

    void attackteamchange(List<GameObject> enemiesInRange)
    {
        if (t.Enemyteam == "red")
        {
            StartCoroutine(BlueAttackCold(enemiesInRange));
        }
        else
        {
            StartCoroutine(RedAttackCold(enemiesInRange));
        }

    }

    private IEnumerator BlueAttackCold(List<GameObject> enemiesInRange)
    {
        animator.SetBool("BlueAttack", true);
        yield return new WaitForSeconds(0.2f);
        animator.SetBool("BlueAttack", false);
        Shoot(enemiesInRange);

    }

    private IEnumerator RedAttackCold(List<GameObject> enemiesInRange)
    {
        animator.SetBool("RedAttack", true);
        yield return new WaitForSeconds(0.2f);
        animator.SetBool("RedAttack", false);
        Shoot(enemiesInRange);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        t = GetComponent<Team>();
        if (other.CompareTag(t.Enemyteam) && other.gameObject.layer != 11)
        {
            enemiesInRange.Add(other.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(t.Enemyteam))
        {
            enemiesInRange.Remove(other.gameObject);
        }
    }

    private void Shoot(List<GameObject> enemiesInRange)
    {
        List<GameObject> nenemy = enemiesInRange;
        health targetheal;

        foreach (GameObject n in nenemy)
        {
            targetheal = n.GetComponentInChildren<health>();
            targetheal.Hurt((int)towerData.CurrentLevel.damage);
        }
    }
}

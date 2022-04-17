using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveShoot : MonoBehaviour
{
    public List<GameObject> enemiesInRange;
    private float lastShotTime;
    private TowerData towerData;
    [SerializeField]
    private GameObject attackobject;
    private Team t;
    private int firstshoot = 0;
    private Animator animator;
    // Use this for initialization

    private void Awake()
    {
        t = GetComponent<Team>();
        GameObject boxCollider2DUp = this.transform.Find("detectUp").gameObject;
        GameObject boxCollider2DDown = this.transform.Find("detectDown").gameObject;
        if (t.Enemyteam == "red")
        {
            boxCollider2DUp.SetActive(false);
            boxCollider2DDown.SetActive(true);
        }
        else
        {
            boxCollider2DUp.SetActive(true);
            boxCollider2DDown.SetActive(false);
        }
    }

    void Start()
    {
        enemiesInRange = new List<GameObject>();
        lastShotTime = Time.time;
        towerData = gameObject.GetComponentInChildren<TowerData>();
        animator = GetComponent<Animator>();
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
            if (firstshoot == 0)
            {
                attackteamchange(enemiesInRange);
                lastShotTime = Time.time;
                firstshoot = 1;
            }
            if (Time.time - lastShotTime > towerData.CurrentLevel.fireRate)
            {
                attackteamchange(enemiesInRange);
                lastShotTime = Time.time;
            }
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

    private void OnEnemyDestroy(GameObject enemy)
    {
        enemiesInRange.Remove(enemy);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag(t.Enemyteam) && other.gameObject.layer != 11)
        {
            if (enemiesInRange.Count == 0)
            {
                enemiesInRange.Add(other.gameObject);
            }
            else
            {
                foreach (GameObject enemy in enemiesInRange)
                {
                    if (enemy.name != other.gameObject.name)
                    {
                        enemiesInRange.Add(other.gameObject);
                    }
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(t.Enemyteam))
        {
            enemiesInRange.Remove(other.gameObject);
        }

        if (other.CompareTag("Wave"))
        {
            Destroy(other.gameObject);
        }
    }

    private void Shoot(List<GameObject> enemiesInRange)
    {
        GameObject wave = (GameObject)Instantiate(attackobject);
        wave.transform.position = this.transform.position;
        Wave wavedata = wave.GetComponent<Wave>();
        wavedata.damage = towerData.CurrentLevel.damage;
        wavedata.t = this.t;
        if (t.Enemyteam == "red")
        {
            wave.transform.position += new Vector3(0, -5.5f, 0);
            wavedata.wavespeed = -wavedata.wavespeed;
        }
        else
        {
            wave.transform.position += new Vector3(0, 2f, 0);
        }
    }
}

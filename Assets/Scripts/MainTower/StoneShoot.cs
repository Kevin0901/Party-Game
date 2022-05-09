using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class StoneShoot : MonoBehaviour
{
    [SerializeField] private List<GameObject> enemiesInRange;
    private Animator animator;
    private float lastShotTime;
    private TowerData towerData;

    [SerializeField] private GameObject bullet;
    [SerializeField] private float BulletSpeed;
    private Team t;
    private float firstshoot = 0;
    private bool issave;
    PhotonView PV;

    // Use this for initialization
    void Start()
    {
        this.transform.SetParent(GameObject.Find("PAPA").transform);
        PV = GetComponent<PhotonView>();  //定義PhotonView
        this.gameObject.tag = PhotonView.Find((int)PV.InstantiationData[0]).tag;
        t = GetComponent<Team>();
        t.SetEnemy();
        animator = GetComponent<Animator>();
        enemiesInRange = new List<GameObject>();
        lastShotTime = Time.time;
        towerData = gameObject.GetComponentInChildren<TowerData>();
        issave = false;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject target = null;
        // 1
        float minimalEnemyDistance = float.MaxValue;
        foreach (GameObject enemy in enemiesInRange)
        {
            if (enemy != null)
            {
                float distanceToGoal = Vector3.Distance(transform.position, enemy.transform.position);
                if (distanceToGoal < minimalEnemyDistance)
                {
                    target = enemy;
                    minimalEnemyDistance = distanceToGoal;
                }
            }

        }
        // 2
        if (target != null)
        {
            if (firstshoot == 0)
            {

                attackteamchange(target);
                lastShotTime = Time.time;
                firstshoot = 1;
            }

            if (Time.time - lastShotTime > towerData.CurrentLevel.fireRate)
            {
                attackteamchange(target);
                lastShotTime = Time.time;
            }

        }
    }

    void attackteamchange(GameObject target)
    {
        if (t.Enemyteam == "red")
        {
            StartCoroutine(BlueAttackCold(target.GetComponentInChildren<Collider2D>()));
        }
        else if (t.Enemyteam == "blue")
        {
            StartCoroutine(RedAttackCold(target.GetComponentInChildren<Collider2D>()));
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



    void OnTriggerStay2D(Collider2D other)
    {
        t = GetComponent<Team>();
        if (other.CompareTag(t.Enemyteam) && other.gameObject.layer != 11)
        {
            if (enemiesInRange.Count == 0)
            {
                enemiesInRange.Add(other.gameObject);
            }
            else
            {
                for (int i = 0; i < enemiesInRange.Count; i++)
                {
                    if (enemiesInRange[i].name == other.gameObject.name)
                    {
                        issave = true;
                        break;
                    }
                }
                if (!issave)
                {
                    enemiesInRange.Add(other.gameObject);
                    issave = false;
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
        newBullet.transform.position += new Vector3(0, 3, 0);
        rock bulletComp = newBullet.GetComponent<rock>();
        bulletComp.target = target.gameObject;
        bulletComp.startPosition = startPosition;
        bulletComp.targetPosition = targetPosition;
        bulletComp.damage = towerData.CurrentLevel.damage;
        bulletComp.speed = BulletSpeed;
        bulletComp.rt = this.t;


    }
}

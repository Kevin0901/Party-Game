using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
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
    private bool issave;
    PhotonView PV;

    private void Awake()
    {
        PV = GetComponent<PhotonView>();  //定義PhotonView
        this.gameObject.tag = PhotonView.Find((int)PV.InstantiationData[0]).tag;
        t = GetComponent<Team>();
        t.SetEnemy();
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
        this.transform.SetParent(GameObject.Find("PAPA").transform);
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

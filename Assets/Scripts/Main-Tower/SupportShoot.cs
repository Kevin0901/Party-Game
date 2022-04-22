using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class SupportShoot : MonoBehaviour
{
    public List<GameObject> TowerInRange;
    private Animator animator;
    private Team t;
    // Use this for initialization
    PhotonView PV;
    void Start()
    {
        this.transform.SetParent(GameObject.Find("PAPA").transform);
        PV = GetComponent<PhotonView>();  //定義PhotonView
        this.gameObject.tag = PhotonView.Find((int)PV.InstantiationData[0]).tag;
        TowerInRange = new List<GameObject>();
        animator = GetComponent<Animator>();
        t = GetComponent<Team>();
        t.SetEnemy();
        animator = GetComponent<Animator>();
    }

    private IEnumerator BlueAttackCold(Collider2D target)
    {
        animator.SetBool("BlueAttack", true);
        yield return new WaitForSeconds(0.2f);
        animator.SetBool("BlueAttack", false);
        Shoot(target.gameObject);

    }

    private IEnumerator RedAttackCold(Collider2D target)
    {
        animator.SetBool("RedAttack", true);
        yield return new WaitForSeconds(0.2f);
        animator.SetBool("RedAttack", false);
        Shoot(target.gameObject);
    }

    void attackteamchange(GameObject target)
    {
        if (t.Enemyteam == "red")
        {
            if (target.layer == 11)
            {
                StartCoroutine(BlueAttackCold(target.GetComponentInChildren<Collider2D>()));
            }
        }
        else
        {
            if (target.layer == 11)
            {
                StartCoroutine(RedAttackCold(target.GetComponentInChildren<Collider2D>()));
            }
        }

    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag(this.tag))
        {
            attackteamchange(other.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(this.tag))
        {
            TowerInRange.Remove(other.gameObject);
        }
    }

    void Shoot(GameObject tower)
    {
        int cnt = 0;
        TowerData towerdata = tower.GetComponent<TowerData>();
        foreach (GameObject n in TowerInRange)
        {
            if (tower != null)
            {
                if (tower.name == n.name)
                {
                    cnt += 1;
                }
            }
        }
        if (cnt == 0)
        {
            if (towerdata != null)
            {
                towerdata.CurrentLevel.damage *= 1.1f;
                TowerInRange.Add(tower);
            }
        }
    }
}

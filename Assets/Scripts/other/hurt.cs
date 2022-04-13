using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class hurt : MonoBehaviour
{
    private Team t;
    private int damageToGive;
    void Start()
    {
        t = this.GetComponentInParent<Team>();
        if (this.transform.parent.gameObject.layer == 10)
        {
            damageToGive = this.gameObject.GetComponentInParent<PlayerMovement>().attackDamage;
        }
        else
        {
            damageToGive = this.gameObject.GetComponentInParent<monsterMove>().attackDamage;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(t.Enemyteam))
        {
            other.gameObject.GetComponentInChildren<health>().Hurt(damageToGive);
        }
    }
}

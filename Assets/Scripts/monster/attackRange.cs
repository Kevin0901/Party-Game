using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attackRange : MonoBehaviour
{
    // Start is called before the first frame update
    // private monsterMove m;
    // private CapsuleCollider2D c;
    void Awake()
    {
        // c = this.GetComponentInParent<CapsuleCollider2D>();
        // this.gameObject.AddComponent(c.GetType());
        // this.GetComponent<CapsuleCollider2D>().isTrigger = true;
        // this.GetComponent<CapsuleCollider2D>().offset = c.offset;
        // this.GetComponent<CapsuleCollider2D>().direction = c.direction;
        // this.GetComponent<CapsuleCollider2D>().size = c.size;
        // m = this.GetComponentInParent<monsterMove>();
    }

    // private void OnTriggerStay2D(Collider2D other)
    // {
    //     if (m.EnemyList.Count > 0 && other.gameObject == m.EnemyList[0])
    //     {
    //         m.currentState = MonsterState.attack;
    //         m.enemyPos = other.transform.position;
    //     }
    // }
}

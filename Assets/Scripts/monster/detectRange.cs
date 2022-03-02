using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class detectRange : MonoBehaviour
{
    // Start is called before the first frame update
    private Team t; //隊伍
    private monsterMove m;
    private GameObject parent;
    [Header("與敵人距離")]
    [SerializeField] private float dis;//與敵人距離
    void Awake()
    {
        t = this.GetComponentInParent<Team>();
        m = this.GetComponentInParent<monsterMove>();
        parent = this.transform.parent.gameObject;
    }
    //進入trigger
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag(t.Enemyteam) && m.currentState != MonsterState.idle)
        {
            if (m.EnemyList.Count == 0)
            {
                m.EnemyList.Add(other.gameObject);

            }
            else if (other.gameObject == m.EnemyList[0])
            {
                dis = Vector3.Distance(parent.transform.position, other.transform.position); //兩座標之間的距離長度
                if (dis <= m.attackRange)
                {
                    m.currentState = MonsterState.attack;
                }
                else if (m.currentState != MonsterState.attack)
                {
                    m.currentState = MonsterState.track;
                }
                m.enemyPos = other.transform.position;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(t.Enemyteam))
        {
            if ((m.EnemyList.Count > 0 && other.gameObject == m.EnemyList[0]))
            {
                m.currentState = MonsterState.walk;
                m.EnemyList.Remove(other.gameObject);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class archerArrowMove : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject enemy;
    private float rotate;
    private Vector3 enemyPos;
    private Vector2 line;
    private void Start()
    {
        if (this.GetComponentInParent<monsterMove>().EnemyList.Count == 0)
        {
            Destroy(this.gameObject);
        }
        else
        {
            enemy = this.GetComponentInParent<monsterMove>().EnemyList[0];
            this.gameObject.transform.SetParent(null);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (enemy == null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            enemyPos = enemy.transform.position;
            line = this.transform.position - enemyPos;
            rotate = Mathf.Atan2(line.y, line.x) * Mathf.Rad2Deg;
            this.transform.rotation = Quaternion.AngleAxis(rotate, Vector3.forward);
            this.transform.position = Vector3.MoveTowards(transform.position, enemyPos, Time.deltaTime * 15); //往敵人方向移動
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == enemy)
        {
            other.gameObject.GetComponentInChildren<health>().Hurt(10);
            Destroy(this.gameObject);
        }
    }

}

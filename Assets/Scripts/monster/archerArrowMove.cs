using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class archerArrowMove : MonoBehaviour
{
    // Start is called before the first frame update
    [HideInInspector] public GameObject target;
    [HideInInspector] public float speed, damege;
    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Vector3 enemyPos = target.transform.position;
            Vector3 line = this.transform.position - target.transform.position;
            float rotate = Mathf.Atan2(line.y, line.x) * Mathf.Rad2Deg;
            this.transform.rotation = Quaternion.AngleAxis(rotate, Vector3.forward);
            this.transform.position = Vector3.MoveTowards(transform.position, enemyPos, Time.deltaTime * speed); //往敵人方向移動
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == target)
        {
            other.gameObject.GetComponentInChildren<health>().Hurt(10);
            Destroy(this.gameObject);
        }
    }

}

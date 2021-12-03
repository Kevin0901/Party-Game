using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rock : MonoBehaviour
{
    public float speed;
    public float damage;
    public GameObject target;
    public Vector3 startPosition;
    public Vector3 targetPosition;

    private float distance;
    private float startTime;
    [SerializeField] private List<GameObject> enemylist;
    public Team rt;
    private Animator animator;



    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        startTime = Time.time;
        startPosition = gameObject.transform.position;
        targetPosition = target.transform.position;
        distance = Vector2.Distance(startPosition, targetPosition);
        enemylist = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        // 1 
        float timeInterval = Time.time - startTime;
        gameObject.transform.position = Vector3.Lerp(startPosition, targetPosition, timeInterval * speed / distance);

        // 2 
        if (gameObject.transform.position.Equals(targetPosition))
        {
            StartCoroutine(attackcold(this.gameObject));
        }
    }

    private IEnumerator attackcold(GameObject rock)
    {
        animator.SetBool("attack", true);
        yield return new WaitForSeconds(0.2f);
        hurt(enemylist);
        Destroy(rock);

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(rt.Enemyteam))
        {
            enemylist.Add(other.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(rt.Enemyteam))
        {
            enemylist.Remove(other.gameObject);
        }
    }

    void hurt(List<GameObject> enemylist)
    {
        if (enemylist.Count != 0)
        {
            foreach (GameObject n in enemylist)
            {
                if (n.layer != 11)
                {
                    health eheal = n.GetComponentInChildren<health>();
                    eheal.Hurt((int)damage);
                }

            }
            // for (int i = 0; i < enemylist.Count; i++)
            // {
            //     Debug.Log("inif");
            //     health eheal = enemylist[i].GetComponentInChildren<health>();
            //     if (eheal.curH <= 0)
            //     {
            //         Debug.Log("chr");
            //         Destroy(enemylist[i]);
            //     }
            // }
        }
    }
}


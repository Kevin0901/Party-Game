using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalThief : MonoBehaviour
{

    [SerializeField] private GameObject[] waypoints;
    [SerializeField] private GameObject[] genres;
    private int curwaypoint = 0;
    public float speed = 3;
    private int curh, bfhurth;
    private int havespeedupyet = 0;
    private Animator animator;

    private void Start()
    {
        curh = this.GetComponentInChildren<health>().curH;
        bfhurth = curh;
        animator = this.gameObject.GetComponent<Animator>();
    }

    private void Update()
    {
        curh = this.GetComponentInChildren<health>().curH;
        Vector3 endPosition = waypoints[curwaypoint].transform.position;
        Vector2 direction = endPosition - transform.position;
        animator.SetFloat("moveX", direction.x);
        animator.SetFloat("moveY", direction.y);
        transform.position = Vector3.MoveTowards(transform.position, endPosition, Time.deltaTime * speed);
        if (transform.position.Equals(endPosition))
        {
            if (curwaypoint < waypoints.Length - 1)
            {
                curwaypoint++;
            }
            else
            {
                curwaypoint = 0;
            }
        }

        if (bfhurth != curh)
        {
            Instantiate(genres[Random.Range(0, 4)], transform.position, genres[Random.Range(0, 4)].transform.rotation);
            if (havespeedupyet == 0)
            {
                havespeedupyet = 1;
                StartCoroutine(speedupcolddown());
            }
            bfhurth = curh;
        }
    }


    IEnumerator speedupcolddown()
    {
        float orginspeed = speed;
        speed *= 2;
        yield return new WaitForSeconds(2);
        speed = orginspeed;
        havespeedupyet = 0;
    }
}

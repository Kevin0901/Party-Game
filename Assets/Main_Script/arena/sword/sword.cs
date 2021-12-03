using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sword : MonoBehaviour
{
    private GameObject lastplayer;
    private arenaPlayer master;
    private float hurt;
    // Start is called before the first frame update
    void Awake()
    {
        lastplayer = this.gameObject;
        master = this.GetComponentInParent<arenaPlayer>();
    }

    // Update is called once per frame
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == 10)
        {
            if (other.gameObject != lastplayer && other.gameObject != this.gameObject.transform.parent.gameObject)
            {
                other.gameObject.transform.Find("sword").gameObject.SetActive(true);
                other.gameObject.transform.Find("sword").gameObject.GetComponent<sword>().Savelastplayer(this.transform.parent.gameObject);
                this.gameObject.SetActive(false);
            }
        }
    }
    private void OnEnable()
    {
        master.speed *= 1.5f;
        hurt = Time.time;
    }
    private void OnDisable()
    {
        master.speed /= 1.5f;
    }

    private void Hurt()
    {
        master.hurt(0.5f);
    }
    private void Update()
    {
        if (Time.time - hurt > 1.75f)
        {
            Hurt();
            hurt = Time.time;
        }
    }
    public void Savelastplayer(GameObject player)
    {
        lastplayer = player;
        StartCoroutine(cleanlastplayer());
    }
    private IEnumerator cleanlastplayer()
    {
        yield return new WaitForSeconds(1f);
        lastplayer = this.gameObject;
    }
}

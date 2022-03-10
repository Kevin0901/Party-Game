using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] private GameObject shield;
    // private arenaController game;
    // Start is called before the first frame update
    private void Start()
    {
        // game = GameObject.Find("FightGameManager").GetComponent<arenaController>();
    }
    // Update is called once per frame
    void Update()
    {
        // if (game.isover)
        // {
        //     Destroy(this.gameObject);
        // }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == 10 && !other.gameObject.transform.Find("shield").gameObject.activeSelf)
        {
            other.gameObject.transform.Find("shield").gameObject.SetActive(true);
            Invoke("spawn", 1f);
            this.gameObject.SetActive(false);
        }
    }
    void spawn()
    {
        GameObject clone =  Instantiate(shield, new Vector3(Random.Range(-25, 25), Random.Range(-25, 25), 0), shield.transform.rotation);
        clone.SetActive(true);
        Destroy(this.gameObject);
    }
}

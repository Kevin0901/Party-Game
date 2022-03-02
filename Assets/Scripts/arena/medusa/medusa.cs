using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class medusa : MonoBehaviour
{
    [HideInInspector] public playerlist p;
    [SerializeField] private float speed, waitTime;
    [SerializeField] private GameObject ball, shield;
    [SerializeField] private float damage;
    private float rotate, hurt;
    [SerializeField] public int r;
    [SerializeField]private float changeTime, nextTime;
    private arenaController game;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(wait(waitTime));
        Instantiate(shield, new Vector3(Random.Range(-25, 25), Random.Range(-25, 25), 0), shield.transform.rotation);
        p = GameObject.Find("playerManager").GetComponent<playerlist>();
        for (int i = 0; i < p.player.Count; i++)
        {
            p.player[i].GetComponent<arenaPlayer>().currentState = ArenaState.walk;
            p.player[i].transform.Find("shield").gameObject.SetActive(true);
        }
        nextTime = changeTime;
        r = Random.Range(0, p.player.Count);
        game = GameObject.Find("FightGameManager").GetComponent<arenaController>();
        InvokeRepeating("speedUP",1,1);
    }

    // Update is called once per frame
    void Update()
    {
        if (r >= p.player.Count)
        {
            r = 0;
        }
        if (p.player.Count != 0 && p.player[r] != null)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, p.player[r].transform.position, Time.deltaTime * speed);
            Vector2 dir = this.transform.position - p.player[r].transform.position;
            rotate = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            this.transform.rotation = Quaternion.AngleAxis(rotate + 90, Vector3.forward);
            nextTime -= Time.deltaTime;
            if (nextTime < 0)
            {
                if (r == p.player.Count - 1)
                {
                    r = 0;
                }
                else
                {
                    r += 1;
                }
                spawnBall();
                nextTime = changeTime;
            }
        }
    }
    void speedUP()
    {
        speed +=0.2f;
    }
    void spawnBall()
    {
        GameObject k = Instantiate(ball, this.transform.position, this.transform.rotation);
        k.GetComponent<ball>().launch(new Vector2(p.player[r].transform.position.x, p.player[r].transform.position.y));
    }
    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.layer == 10 && Time.time - hurt > 0.5f)
        {
            hurt = Time.time;
            other.gameObject.GetComponentInChildren<arenaPlayer>().hurt(damage);
        }
    }
    IEnumerator wait(float t)
    {
        this.GetComponent<Animator>().enabled = false;
        this.enabled = false;
        yield return new WaitForSeconds(t);
        this.GetComponent<Animator>().enabled = true;
        this.enabled = true;
    }
}

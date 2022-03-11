using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedusaEvent : MonoBehaviour
{
    [SerializeField] private GameObject UI, ball, mirror;
    [SerializeField] private float waitTime, changeTime, speed, damage;
    private float rotate, hurtRate, nextTime;
    private int r;
    void Start()
    {
        nextTime = 0;
        hurtRate = 0;
        // Instantiate(shield, new Vector3(Random.Range(-25, 25), Random.Range(-25, 25), 0), shield.transform.rotation);
    }
    public void StartGame()
    {
        r = Random.Range(0, FightManager.Instance.gamelist.Count);
        for (int i = 0; i < FightManager.Instance.gamelist.Count; i++)
        {
            GameObject.Find("HealthUI").transform.GetChild(i).gameObject.SetActive(true);
            FightManager.Instance.plist[i].GetComponent<arenaPlayer>().currentState = ArenaState.walk;
            FightManager.Instance.plist[i].transform.Find("shield").gameObject.SetActive(true);
        }
        StartCoroutine(wait(waitTime));
    }
    IEnumerator wait(float t)
    {
        yield return new WaitForSeconds(t);
        this.GetComponent<MedusaEvent>().enabled = true;
        this.GetComponent<Animator>().enabled = true;
        mirror.GetComponent<circleMirror>().enabled = true;
        StartCoroutine(speedUP());
    }
    // Update is called once per frame
    void Update()
    {
        if (FightManager.Instance.gamelist.Count == 1)
        {
            UI.SetActive(true);
            if (FightManager.Instance.gamelist[0].GetComponent<arenaPlayer>().red)
            {
                UI.transform.Find("red").gameObject.SetActive(true);
            }
            else
            {
                UI.transform.Find("blue").gameObject.SetActive(true);
            }
            for (int i = 0; i < FightManager.Instance.gamelist.Count; i++)
            {
                FightManager.Instance.plist[i].transform.Find("shield").gameObject.SetActive(false);
            }
            FightManager.Instance.gamelist[0].SetActive(false);
            this.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, FightManager.Instance.gamelist[r].transform.position, Time.deltaTime * speed);
            Vector2 dir = this.transform.position - FightManager.Instance.gamelist[r].transform.position;
            rotate = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            this.transform.rotation = Quaternion.AngleAxis(rotate + 90, Vector3.forward);
            if (Time.time > nextTime)
            {
                if (r == FightManager.Instance.gamelist.Count - 1)
                {
                    r = 0;
                }
                else
                {
                    r += 1;
                }
                spawnBall();
                nextTime = Time.time + changeTime;
            }
        }
    }
    IEnumerator speedUP()
    {
        yield return new WaitForSeconds(1f);
        speed += 0.35f;
        StartCoroutine(speedUP());
    }
    public void spawnBall()
    {
        GameObject eye = Instantiate(ball, this.transform.position, this.transform.rotation);
        eye.GetComponent<ball>().move(r);
    }
    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.layer == 10 && Time.time - hurtRate > 0.5f)
        {
            hurtRate = Time.time;
            other.gameObject.GetComponent<arenaPlayer>().hurt(damage);
        }
    }
}

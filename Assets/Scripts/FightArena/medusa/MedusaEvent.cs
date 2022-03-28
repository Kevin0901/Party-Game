using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedusaEvent : MonoBehaviour
{
    [SerializeField] private GameObject UI, ball, mirror;
    [SerializeField] private float waitTime, changeTime, speed, damage;
    private float rotate, hurtRate, nextTime;
    private int randomPlayer;
    void Start()
    {
        nextTime = 0;
        hurtRate = 0;
    }
    //開始遊戲
    public void StartGame()
    {
        randomPlayer = Random.Range(0, FightManager.Instance.plist.Count);
        for (int i = 0; i < FightManager.Instance.plist.Count; i++)
        {
            GameObject.Find("HealthUI").transform.GetChild(i).gameObject.SetActive(true);
            FightManager.Instance.plist[i].GetComponent<arenaPlayer>().currentState = ArenaState.walk;
            FightManager.Instance.plist[i].transform.Find("shield").gameObject.SetActive(true);
        }
        StartCoroutine(wait(waitTime));
    }
    //等待時間
    IEnumerator wait(float t)
    {
        yield return new WaitForSeconds(t);
        this.GetComponent<Animator>().enabled = true;
        mirror.GetComponent<circleMirror>().enabled = true;
        StartCoroutine(speedUP());
        StartCoroutine(monsterMove());
    }
    void Update()
    {
        if (FightManager.Instance.plist.Count <= 1)
        {

            UI.SetActive(true);
            if (FightManager.Instance.plist[0].GetComponent<arenaPlayer>().red)
            {
                UI.transform.Find("red").gameObject.SetActive(true);
            }
            else
            {
                UI.transform.Find("blue").gameObject.SetActive(true);
            }
            this.transform.parent.gameObject.SetActive(false);
        }
    }
    IEnumerator monsterMove()
    {
        //追蹤玩家的位置
        if (randomPlayer >= FightManager.Instance.plist.Count)
        {
            randomPlayer = FightManager.Instance.plist.Count - 1;
        }
        this.transform.position = Vector3.MoveTowards(this.transform.position, FightManager.Instance.plist[randomPlayer].transform.position, Time.deltaTime * speed);
        Vector2 dir = this.transform.position - FightManager.Instance.plist[randomPlayer].transform.position;
        rotate = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        this.transform.rotation = Quaternion.AngleAxis(rotate + 90, Vector3.forward);
        if (Time.time > nextTime)
        {
            if (randomPlayer == FightManager.Instance.plist.Count - 1)
            {
                randomPlayer = 0;
            }
            else
            {
                randomPlayer += 1;
            }
            //生成球
            spawnBall();
            nextTime = Time.time + changeTime;
        }
        yield return null;
        StartCoroutine(monsterMove());
    }
    //梅杜莎速度加快
    IEnumerator speedUP()
    {
        yield return new WaitForSeconds(1f);
        speed += 0.35f;
        StartCoroutine(speedUP());
    }
    //生成球
    public void spawnBall()
    {
        GameObject eye = Instantiate(ball, this.transform.position, this.transform.rotation);
        eye.GetComponent<ball>().move(randomPlayer);
    }
    //碰到玩家給傷害
    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.layer == 10 && Time.time - hurtRate > 0.5f)
        {
            hurtRate = Time.time;
            other.gameObject.GetComponent<arenaPlayer>().hurt(damage);
        }
    }
}

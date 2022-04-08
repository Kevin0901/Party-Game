using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
public class medusa : MonoBehaviour
{
    [SerializeField] private GameObject ball;
    private int randomPlayer;
    private float rotate, nextChange, nexthurt;
    [SerializeField] private float changeRate, hurtRate, M_speed, M_damage, B_speed, B_damege;
    PhotonView PV;
    private void Start()
    {
        nextChange = 0;
        nexthurt = 0;
    }
    private void OnEnable()
    {
        PV = GetComponent<PhotonView>();
        if(!PV.IsMine)
        {
            return;
        }
        randomPlayer = Random.Range(0, FightManager.Instance.plist.Count);
        nextChange = Time.time + changeRate;
        StartCoroutine(monsterMove());
        StartCoroutine(speedUP());
    }
    IEnumerator monsterMove()
    {
        //追蹤玩家的位置
        if (randomPlayer >= FightManager.Instance.plist.Count)
        {
            randomPlayer = FightManager.Instance.plist.Count - 1;
        }
        this.transform.position = Vector3.MoveTowards(this.transform.position,
        FightManager.Instance.plist[randomPlayer].transform.position, Time.deltaTime * M_speed);

        Vector2 dir = this.transform.position - FightManager.Instance.plist[randomPlayer].transform.position;
        rotate = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        this.transform.rotation = Quaternion.AngleAxis(rotate + 90, Vector3.forward);

        if (Time.time > nextChange)
        {
            if (randomPlayer == FightManager.Instance.plist.Count - 1)
            {
                randomPlayer = 0;
            }
            else
            {
                randomPlayer += 1;
            }
            nextChange = Time.time + changeRate;
            StartCoroutine(spawnBall());
        }
        yield return null;
        StartCoroutine(monsterMove());
    }
    //梅杜莎速度加快
    IEnumerator speedUP()
    {
        yield return new WaitForSeconds(1.25f);
        M_speed += 0.35f;
        StartCoroutine(speedUP());
    }
    //生成球
    IEnumerator spawnBall()
    {
        yield return new WaitForSeconds(0.3f);
        // GameObject eye = Instantiate(ball, this.transform.position, this.transform.rotation);
        GameObject eye = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "arena/Medusa/ball"), this.transform.position, this.transform.rotation);        
        eye.GetComponent<ball>().speed = B_speed;
        eye.GetComponent<ball>().damage = B_damege;
        eye.GetComponent<ball>().move(randomPlayer);
    }

    //碰到玩家給傷害
    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.layer == 10 && Time.time > nexthurt)
        {
            nexthurt = Time.time + hurtRate;
            other.gameObject.GetComponent<arenaPlayer>().hurt(M_damage);
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CupidEvent : MonoBehaviour
{
    [SerializeField] private GameObject arrow;
    [SerializeField] private GameObject UI;
    [SerializeField] private float Cupid_Speed;
    private Quaternion k, k2, k3, k4;
    private int arrow_limit;
    private float angle, rotate, speed, cooldownTime;
    private List<GameObject> arrow_Store;
    private Rigidbody2D mrigibody;
    private Vector3 firstpos, newpos;
    private Vector2 cupidpos;
    void Awake()
    {
        mrigibody = this.GetComponent<Rigidbody2D>();
        arrow_Store = new List<GameObject>();
        arrow_limit = 45;
        cooldownTime = 1.5f;
        //初始化物件池
        for (int i = 0; i < arrow_limit; i++)
        {
            GameObject addArrow = Instantiate(arrow, this.transform.position, Quaternion.identity);
            arrow_Store.Add(addArrow);
            if (addArrow.transform.parent != this.transform)
            {
                addArrow.transform.SetParent(this.transform);
            }
            addArrow.GetComponent<CupidArrowMove>().parent = this.gameObject;
            addArrow.SetActive(false);
        }
        cupidpos = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized;
    }
    //開始遊戲
    public void StartGame()
    {
        for (int i = 0; i < FightManager.Instance.gamelist.Count; i++)
        {
            FightManager.Instance.gamelist[i].GetComponent<arenaPlayer>().currentState = ArenaState.walk;
        }
        StartCoroutine(spawnArrow(cooldownTime, Random.Range(1, 4)));//隨機一種模式
        StartCoroutine(Move(cooldownTime));  //給邱比特移動
    }
    private void Update()
    {
        if (FightManager.Instance.gamelist.Count <= 1)
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
            FightManager.Instance.gamelist[0].SetActive(false);
            for (int i = 0; i < FightManager.Instance.plist.Count; i++)
            {
                FightManager.Instance.plist[i].transform.Find("NumTitle").GetChild(i).GetComponent<SpriteRenderer>().color = Color.white;
            }
            this.gameObject.SetActive(false);
        }
    }
    //移動
    public IEnumerator Move()
    {
        mrigibody.velocity = cupidpos * Cupid_Speed;
        yield return null;
        StartCoroutine(Move());
    }
    //移動
    public IEnumerator Move(float time)
    {
        yield return new WaitForSeconds(time);
        mrigibody.velocity = cupidpos * Cupid_Speed;
        yield return null;
        StartCoroutine(Move());
    }
    //箭矢生成
    IEnumerator spawnArrow(float time, int type)
    {
        yield return new WaitForSeconds(time);
        string FunName = "FireType_" + type;
        StartCoroutine(FunName, 2); //總共幾波攻擊
    }
    //攻擊模式_1
    IEnumerator FireType_1(int number)
    {
        speed = 50f;
        for (int i = 0; i < number; i++)
        {
            for (int j = 0; j < FightManager.Instance.gamelist.Count; j++)
            {
                Vector3 playPos = FightManager.Instance.gamelist[j].transform.position;
                newpos = playPos - this.transform.position;
                rotate = (Mathf.Atan2(newpos.y, newpos.x) * Mathf.Rad2Deg) - 180;
                GetPoolInstance();
            }
            yield return new WaitForSeconds(0.5f); //下一波發射時間
        }
        StartCoroutine(spawnArrow(cooldownTime, 2));
        yield return null;
    }
    //攻擊模式_2
    IEnumerator FireType_2(int number)//發射波數
    {
        for (int i = 0; i < number; i++)
        {
            speed = 25f;
            firstpos = new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f));
            Vector2 line = Vector3.zero - firstpos;
            rotate = Mathf.Atan2(line.y, line.x) * Mathf.Rad2Deg;
            newpos = firstpos;
            angle = 36;
            k = Quaternion.AngleAxis(angle, Vector3.forward);
            for (int j = 0; j < 10; j++)
            {
                GetPoolInstance();
                rotate += angle;
                newpos = k * newpos;
            }

            yield return new WaitForSeconds(0.5f);
            k2 = Quaternion.AngleAxis(angle * 0.5f, Vector3.forward);
            speed = 35;
            rotate += angle * 0.5f;
            newpos = k2 * firstpos;
            for (int j = 0; j < 10; j++)
            {
                GetPoolInstance();
                rotate += angle;
                newpos = k * newpos;
            }
            yield return new WaitForSeconds(1f); //下一波發射時間
        }
        StartCoroutine(spawnArrow(cooldownTime, 3));
        yield return null;
    }
    //攻擊模式_3
    IEnumerator FireType_3(int number)
    {
        speed = 30f;
        for (int i = 0; i < number; i++)  //發射波數
        {
            firstpos = new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f));
            Vector2 line = Vector3.zero - firstpos;
            Vector2 line2 = Vector3.zero + firstpos;
            rotate = Mathf.Atan2(line.y, line.x) * Mathf.Rad2Deg;
            newpos = firstpos;
            angle = 12f;
            k = Quaternion.AngleAxis(angle, Vector3.forward);
            float angle2 = 90f;
            k2 = Quaternion.AngleAxis(angle2, Vector3.forward);

            for (int j = 0; j < 30; j++)
            {
                GetPoolInstance();
                rotate += angle;
                newpos = k * newpos;
                rotate += angle2;
                newpos = k2 * newpos;
                GetPoolInstance();
                rotate += angle2;
                newpos = k2 * newpos;
                GetPoolInstance();
                rotate += angle2;
                newpos = k2 * newpos;
                GetPoolInstance();
                rotate += angle2;
                newpos = k2 * newpos;
                yield return new WaitForSeconds(0.25f);
            }

            yield return new WaitForSeconds(cooldownTime);
            k3 = Quaternion.AngleAxis(-angle, Vector3.forward);
            k4 = Quaternion.AngleAxis(-angle2, Vector3.forward);

            for (int j = 0; j < 30; j++)
            {
                GetPoolInstance();
                rotate -= angle;
                newpos = k3 * newpos;
                rotate -= angle2;
                newpos = k4 * newpos;
                GetPoolInstance();
                rotate -= angle2;
                newpos = k4 * newpos;
                GetPoolInstance();
                rotate -= angle2;
                newpos = k4 * newpos;
                GetPoolInstance();
                rotate -= angle2;
                newpos = k4 * newpos;
                yield return new WaitForSeconds(0.25f);
            }
            yield return new WaitForSeconds(1); //下一波發射時間
        }
        StartCoroutine(spawnArrow(cooldownTime, 1));
        yield return null;
    }
    //物件池
    private GameObject GetPoolInstance()
    {
        int lastIndex = arrow_Store.Count - 1;
        GameObject arr = arrow_Store[lastIndex];
        arrow_Store.RemoveAt(lastIndex);
        arr.transform.SetParent(null);
        arr.transform.rotation = Quaternion.AngleAxis(rotate, Vector3.forward);
        arr.SetActive(true);
        arr.GetComponent<CupidArrowMove>().speed = speed;
        arr.GetComponent<CupidArrowMove>().Pos = newpos;
        return arr;
    }
    //返回物件池
    public void BackToPool(GameObject arr)
    {
        arrow_Store.Add(arr);
        arr.transform.position = this.transform.position;
        arr.transform.rotation = Quaternion.identity;
        arr.transform.SetParent(this.transform);
        arr.SetActive(false);
    }
    //碰撞設定,如果碰到背景，亂數移動邱比特位置
    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("background"))
        {
            cupidpos = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f)).normalized;
        }
    }
    // private IEnumerator TimeCount()
    // {
    //     while (countdownTime > 0)
    //     {
    //         countdownDisplay.text = countdownTime.ToString();
    //         yield return new WaitForSeconds(1f);
    //         countdownTime -= 1;
    //     }
    // }
}

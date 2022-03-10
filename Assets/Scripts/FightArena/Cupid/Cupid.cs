
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Cupid : MonoBehaviour
{
    [SerializeField] private GameObject arrow;
    private Quaternion k, k2, k3, k4;
    private int arrow_limit = 70;
    private float angle, rotate, speed, cooldownTime;
    [SerializeField] private List<GameObject> arrow_Store = new List<GameObject>();
    // [HideInInspector] public playerlist p;
    // public arenaController game;
    private Vector3 firstpos, newpos;
    public int countdownTime;
    private int saveTime;
    public Text countdownDisplay;
    // private arenaController ac;
    // [SerializeField] private bool istouch;
    void Awake() //初始化物件池
    {
        // p = GameObject.Find("playerManager").GetComponent<playerlist>();
        // game = GameObject.Find("FightGameManager").GetComponent<arenaController>();
        for (int i = 0; i < arrow_limit; i++)
        {
            GameObject addArrow = Instantiate(arrow, Vector3.zero, Quaternion.identity);
            arrow_Store.Add(addArrow);
            if (addArrow.transform.parent != this.transform)
            {
                addArrow.transform.SetParent(this.transform);
            }
            addArrow.SetActive(false);
        }
    }
    void Start()
    {
        StartCoroutine(change());
        cooldownTime = 1.35f;
        saveTime = countdownTime;
        StartCoroutine(TimeCount());
    }
    private void Update()
    {
        // if (game.isover)
        // {
        //     Destroy(this.gameObject, 0.1f);
        // }
    }
    //箭矢生成
    IEnumerator spawnArrow(float time)
    {
        yield return new WaitForSeconds(time);
        string FunName = "FireType_" + Random.Range(1, 4);
        StartCoroutine(FunName, 2);
    }
    IEnumerator change()
    {
        // transform.Find("CupidStage").transform.GetChild(sort - 1).gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        // transform.Find("CupidStage").transform.GetChild(sort - 1).gameObject.SetActive(false);
        // for (int i = 0; i < p.player.Count; i++)
        // {
        //     p.player[i].GetComponent<arenaPlayer>().currentState = ArenaState.walk;
        // }
        StartCoroutine(spawnArrow(0.5f));
    }
    //攻擊模式
    IEnumerator FireType_1(int number)
    {
        speed = 50f;
        for (int i = 0; i < number; i++)
        {
            // for (int j = 0; j < p.player.Count; j++)
            // {
            //     newpos = p.player[j].transform.position.normalized;
            //     Vector2 line = Vector3.zero - newpos;
            //     rotate = Mathf.Atan2(line.y, line.x) * Mathf.Rad2Deg;
            //     GetPoolInstance(this.gameObject.transform);
            // }
            yield return new WaitForSeconds(0.5f);
        }
        StartCoroutine(spawnArrow(cooldownTime));
        yield return null;
    }
    IEnumerator FireType_2(int number)//發射波數
    {
        for (int i = 0; i < number; i++)
        {
            speed = 12.5f;
            firstpos = new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f));
            Vector2 line = Vector3.zero - firstpos;
            rotate = Mathf.Atan2(line.y, line.x) * Mathf.Rad2Deg;
            newpos = firstpos;
            angle = 36;
            k = Quaternion.AngleAxis(angle, Vector3.forward);
            for (int j = 0; j < 10; j++)
            {
                GetPoolInstance(this.gameObject.transform);
                rotate += angle;
                newpos = k * newpos;
            }
            yield return new WaitForSeconds(0.5f);
            k2 = Quaternion.AngleAxis(angle * 0.5f, Vector3.forward);
            speed = 25;
            rotate += angle * 0.5f;
            newpos = k2 * firstpos;
            for (int j = 0; j < 10; j++)
            {
                GetPoolInstance(this.gameObject.transform);
                rotate += angle;
                newpos = k * newpos;
            }
            yield return new WaitForSeconds(cooldownTime); //延遲進行下一波發射
        }
        StartCoroutine(spawnArrow(cooldownTime));
        yield return null;
    }

    IEnumerator FireType_3(int number)
    {
        speed = 22.5f;
        for (int i = 0; i < number; i++)  //發射波數
        {
            firstpos = new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f));
            Vector2 line = Vector3.zero - firstpos;
            Vector2 line2 = Vector3.zero + firstpos;
            rotate = Mathf.Atan2(line.y, line.x) * Mathf.Rad2Deg;
            newpos = firstpos;
            angle = 15f;
            k = Quaternion.AngleAxis(angle, Vector3.forward);
            float angle2 = 90f;
            k2 = Quaternion.AngleAxis(angle2, Vector3.forward);

            for (int j = 0; j < 12; j++)
            {
                GetPoolInstance(this.gameObject.transform);
                rotate += angle;
                newpos = k * newpos;
                rotate += angle2;
                newpos = k2 * newpos;
                GetPoolInstance(this.gameObject.transform);
                rotate += angle2;
                newpos = k2 * newpos;
                GetPoolInstance(this.gameObject.transform);
                rotate += angle2;
                newpos = k2 * newpos;
                GetPoolInstance(this.gameObject.transform);
                rotate += angle2;
                newpos = k2 * newpos;
                yield return new WaitForSeconds(0.3f);
            }
            yield return new WaitForSeconds(cooldownTime);
            k3 = Quaternion.AngleAxis(-angle, Vector3.forward);
            k4 = Quaternion.AngleAxis(-angle2, Vector3.forward);

            for (int j = 0; j < 12; j++)
            {
                GetPoolInstance(this.gameObject.transform);
                rotate -= angle;
                newpos = k3 * newpos;
                rotate -= angle2;
                newpos = k4 * newpos;
                GetPoolInstance(this.gameObject.transform);
                rotate -= angle2;
                newpos = k4 * newpos;
                GetPoolInstance(this.gameObject.transform);
                rotate -= angle2;
                newpos = k4 * newpos;
                GetPoolInstance(this.gameObject.transform);
                rotate -= angle2;
                newpos = k4 * newpos;
                yield return new WaitForSeconds(0.3f);
            }
            yield return new WaitForSeconds(cooldownTime); //延遲進行下一波發射
        }
        StartCoroutine(spawnArrow(cooldownTime));
        yield return null;
    }
    //物件池
    private GameObject GetPoolInstance(Transform parent)
    {
        int lastIndex = arrow_Store.Count - 1;
        GameObject arr = arrow_Store[lastIndex];
        arrow_Store.RemoveAt(lastIndex);
        arr.transform.position = parent.transform.position;
        arr.transform.rotation = Quaternion.AngleAxis(rotate, Vector3.forward);
        arr.GetComponent<CupidArrowMove>().speed = speed;
        arr.SetActive(true);
        arr.GetComponent<CupidArrowMove>().Pos = newpos;
        return arr;
    }
    //返回物件池
    public void BackToPool(GameObject arr)
    {
        arrow_Store.Add(arr);
        arr.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 10)
        {
            StopAllCoroutines();
            StartCoroutine(TimeCount());
            other.gameObject.GetComponent<arenaPlayer>().CupidGamepoint += 1;
            // for (int i = 0; i < p.player.Count; i++)
            // {
            //     p.player[i].GetComponent<arenaPlayer>().SpawnPoint();
            //     p.player[i].GetComponent<arenaPlayer>().currentState = ArenaState.idle;
            //     p.player[i].GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
            // }
            for (int j = 0; j < this.gameObject.transform.childCount; j++)
            {
                if (this.gameObject.transform.GetChild(j).gameObject.tag == "arrow" && this.gameObject.transform.GetChild(j).gameObject.activeSelf)
                {
                    BackToPool(this.gameObject.transform.GetChild(j).gameObject);
                }
            }
            StartCoroutine(change());
        }
    }

    private IEnumerator TimeCount()
    {
        while (countdownTime > 0)
        {
            countdownDisplay.text = countdownTime.ToString();
            yield return new WaitForSeconds(1f);
            countdownTime -= 1;
        }
        if (countdownTime == 0)
        {
            countdownTime = saveTime;
            // ac = GameObject.Find("FightGameManager").GetComponent<arenaController>();
            // ac.isCupidEnd = true;
        }
    }
}

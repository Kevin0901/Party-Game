using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HermesEvent : MonoBehaviour
{
    //生成點 x=55 , y=33
    [SerializeField] private GameObject cow, UI;
    [HideInInspector] public float redScore, blueScore;
    [HideInInspector]public int _time;
    public float Nor_mix, Nor_max, Gold_mix, Gold_max;
    private void Start()
    {
        _time = 100;
    }
    public void StartGame()
    {
        // for (int i = 0; i < FightManager.Instance.gamelist.Count; i++)
        // {
        //     FightManager.Instance.gamelist[i].GetComponent<arenaPlayer>().currentState = ArenaState.fastMode;
        // }
        StartCoroutine(spawnCow(Random.Range(Nor_mix, Nor_max)));
        StartCoroutine(spawnGoldCow(Random.Range(Gold_mix, Gold_max)));
        StartCoroutine(timeCount());
    }
    private void Update()
    {
        if (_time < 0)
        {
            UI.SetActive(true);
            if (redScore > blueScore)
            {
                UI.transform.Find("red").gameObject.SetActive(true);
            }
            else if ((redScore < blueScore))
            {
                UI.transform.Find("blue").gameObject.SetActive(true);
            }
            else if ((redScore == blueScore))
            {
                UI.transform.Find("draw").gameObject.SetActive(true);
            }
            for (int i = 0; i < FightManager.Instance.gamelist.Count; i++)
            {
                FightManager.Instance.gamelist[i].SetActive(false);
            }

            this.gameObject.SetActive(false);
        }
    }
    private IEnumerator spawnCow(float time)
    {
        yield return new WaitForSeconds(time);
        float x = Random.Range(-55f, 55f);
        float y = Random.Range(-33f, 33f);
        GameObject a = Instantiate(cow, new Vector3(x, y, 0), this.transform.rotation);
        a.GetComponent<pickcow>().cowScore = 1;
        a.GetComponent<pickcow>().parent = this.gameObject;
        StartCoroutine(spawnCow(Random.Range(Nor_mix, Nor_max)));
    }
    private IEnumerator spawnGoldCow(float time)
    {
        yield return new WaitForSeconds(time);
        float x = Random.Range(-55f, 55f);
        float y = Random.Range(-33f, 33f);
        GameObject a = Instantiate(cow, new Vector3(x, y, 0), this.transform.rotation * Quaternion.Euler(0, 0, 180));
        a.GetComponent<pickcow>().cowScore = 3;
        a.GetComponent<pickcow>().parent = this.gameObject;
        StartCoroutine(spawnGoldCow(Random.Range(Gold_mix, Gold_max)));
    }
    public void red_Score(int point)
    {
        redScore += point;
        this.transform.GetChild(0).Find("red").GetComponent<Text>().text = "Score:" + redScore.ToString();
    }
    public void blue_Score(int point)
    {
        blueScore += point;
        this.transform.GetChild(0).Find("blue").GetComponent<Text>().text = "Score:" + blueScore.ToString();
    }
    public IEnumerator timeCount()
    {
        yield return new WaitForSeconds(1f);
        this.transform.GetChild(0).Find("time").GetComponent<Text>().text = (--_time).ToString();
        StartCoroutine(timeCount());
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HermesEvent : MonoBehaviour
{
    //生成點 x=55 , y=33
    [SerializeField] private GameObject Cow, goldCow, UI;
    private float redScore, blueScore;
    [SerializeField] private int GameTime;
    [SerializeField] private float Normal_mix, Normal_max, Gold_mix, Gold_max;
    public void StartGame()
    {
        for (int i = 0; i < FightManager.Instance.plist.Count; i++)
        {
            FightManager.Instance.plist[i].GetComponent<arenaPlayer>().currentState = ArenaState.fastMode;
        }
        StartCoroutine(spawnCow(Random.Range(Normal_mix, Normal_max)));
        StartCoroutine(spawnGoldCow(Random.Range(Gold_mix, Gold_max)));
        StartCoroutine(timeCount());
    }
    private void Update()
    {
        if (GameTime < 0)
        {
            StopAllCoroutines();
            StartCoroutine(endGame());
        }
    }
    IEnumerator endGame()
    {
        for (int i = 0; i < FightManager.Instance.plist.Count; i++)
        {
            FightManager.Instance.plist[i].GetComponent<arenaPlayer>().currentState = ArenaState.idle;
        }
        this.transform.Find("GameUI").Find("end").GetComponent<Text>().text =
        "遊戲結束\n" + blueScore.ToString() + "  ：  " + redScore.ToString();
        yield return new WaitForSeconds(3f);

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
        this.gameObject.SetActive(false);
    }
    private IEnumerator spawnCow(float time)
    {
        yield return new WaitForSeconds(time);
        float x = Random.Range(-55f, 55f);
        float y = Random.Range(-33f, 33f);
        GameObject a = Instantiate(Cow, new Vector3(x, y, 0), Cow.transform.rotation);
        a.transform.parent = this.transform;
        a.GetComponent<pickcow>().cowScore = 1;
        StartCoroutine(spawnCow(Random.Range(Normal_mix, Normal_max)));
    }
    private IEnumerator spawnGoldCow(float time)
    {
        yield return new WaitForSeconds(time);
        float x = Random.Range(-55f, 55f);
        float y = Random.Range(-33f, 33f);
        GameObject a = Instantiate(Cow, new Vector3(x, y, 0), this.transform.rotation * Quaternion.Euler(0, 0, 180));
        a.transform.parent = this.transform;
        a.GetComponent<pickcow>().cowScore = 3;
        StartCoroutine(spawnGoldCow(Random.Range(Gold_mix, Gold_max)));
    }
    public void R_ScoreADD(int point)
    {
        redScore += point;
        this.transform.Find("GameUI").Find("red").GetComponent<Text>().text = "Score:" + redScore.ToString();
    }
    public void B_ScoreADD(int point)
    {
        blueScore += point;
        this.transform.Find("GameUI").Find("blue").GetComponent<Text>().text = "Score:" + blueScore.ToString();
    }
    public IEnumerator timeCount()
    {
        yield return new WaitForSeconds(1f);
        this.transform.Find("GameUI").Find("time").GetComponent<Text>().text = (--GameTime).ToString();
        StartCoroutine(timeCount());
    }
}

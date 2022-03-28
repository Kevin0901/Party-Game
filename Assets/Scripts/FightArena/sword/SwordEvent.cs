using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordEvent : MonoBehaviour
{
    [SerializeField] private GameObject UI;
    //旋轉地圖
    private void OnEnable()
    {
        transform.Rotate(0, 0, Random.Range(0, 360));
    }
    public void StartGame()
    {
        this.GetComponent<SwordEvent>().enabled = true;
        for (int i = 0; i < FightManager.Instance.plist.Count; i++)
        {
            GameObject.Find("HealthUI").transform.GetChild(i).gameObject.SetActive(true);
            FightManager.Instance.plist[i].GetComponent<arenaPlayer>().currentState = ArenaState.walk;
        }
        StartCoroutine(randomSword(1.5f));
        StartCoroutine(changeBG());
    }
    //地圖懸轉
    private IEnumerator changeBG()
    {
        this.transform.Rotate(0, 0, -0.05f);
        yield return null;
        StartCoroutine(changeBG());
    }
    private void Update()
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
            this.gameObject.SetActive(false);
        }
    }
    //隨機給一個玩家劍
    private IEnumerator randomSword(float time)
    {
        yield return new WaitForSeconds(time);
        int num = Random.Range(0, FightManager.Instance.plist.Count);
        FightManager.Instance.plist[num].transform.Find("sword").gameObject.SetActive(true);
    }
}

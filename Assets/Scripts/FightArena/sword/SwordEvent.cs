using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordEvent : MonoBehaviour
{
    [SerializeField] private GameObject UI;
    //旋轉地圖
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float waitToSword;
    private int totalPlayer;
    private void OnEnable()
    {
        transform.Rotate(0, 0, Random.Range(0, 360));
    }
    public void StartGame()
    {
        this.GetComponent<SwordEvent>().enabled = true;
        for (int i = 0; i < FightManager.Instance.plist.Count; i++)
        {
            totalPlayer++;
            GameObject.Find("HealthUI").transform.GetChild(i).gameObject.SetActive(true);
            FightManager.Instance.plist[i].GetComponent<arenaPlayer>().currentState = ArenaState.walk;
        }
        StartCoroutine(randomSword(waitToSword));
        StartCoroutine(changeBG());
    }
    //地圖懸轉
    private IEnumerator changeBG()
    {
        this.transform.Rotate(0, 0, rotateSpeed);
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
        else if (totalPlayer > FightManager.Instance.plist.Count)
        {
            StartCoroutine(randomSword(waitToSword));
            totalPlayer = FightManager.Instance.plist.Count;
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

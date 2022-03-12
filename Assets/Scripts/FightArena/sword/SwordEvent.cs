using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordEvent : MonoBehaviour
{
    [SerializeField] private GameObject UI;
    private void OnEnable()
    {
        transform.Rotate(0, 0, Random.Range(0, 360));
    }
    public void StartGame()
    {
        this.GetComponent<SwordEvent>().enabled = true;
        for (int i = 0; i < FightManager.Instance.gamelist.Count; i++)
        {
            GameObject.Find("HealthUI").transform.GetChild(i).gameObject.SetActive(true);
            FightManager.Instance.gamelist[i].GetComponent<arenaPlayer>().currentState = ArenaState.walk;
        }
        StartCoroutine(randomSword());
        StartCoroutine(changeBG());
    }
    private IEnumerator changeBG()
    {
        this.transform.Rotate(0, 0, -0.05f);
        yield return null;
        StartCoroutine(changeBG());
    }
    private void Update()
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

            for (int i = 0; i < FightManager.Instance.plist.Count; i++)
            {
                FightManager.Instance.plist[i].transform.Find("sword").gameObject.SetActive(false);
            }
            FightManager.Instance.gamelist[0].SetActive(false);
            this.gameObject.SetActive(false);
        }
    }
    private IEnumerator randomSword()
    {
        yield return new WaitForSeconds(2f);
        int num = Random.Range(0, FightManager.Instance.gamelist.Count);
        FightManager.Instance.gamelist[num].transform.Find("sword").gameObject.SetActive(true);
    }
}

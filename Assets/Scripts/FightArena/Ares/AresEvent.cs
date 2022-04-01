using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AresEvent : MonoBehaviour
{
    [SerializeField] private GameObject UI;
    [SerializeField] private List<GameObject> helmet;
    [SerializeField] private float aresTime;
    public void StartGame()
    {
        for (int i = 0; i < FightManager.Instance.plist.Count; i++)
        {
            FightManager.Instance.plist[i].GetComponent<arenaPlayer>().currentState = ArenaState.walk;
            GameObject.Find("HealthUI").transform.GetChild(i).gameObject.SetActive(true);
        }
        StartCoroutine(aresHelmet());
    }
    private IEnumerator aresHelmet()
    {
        yield return new WaitForSeconds(1);
        transform.Find("helmets").gameObject.SetActive(true);
        int r = Random.Range(0, helmet.Count);
        helmet[r].GetComponent<helmet>().isArens = true;
        helmet[r].GetComponent<helmet>().aresT = aresTime;
    }
    public IEnumerator reset()
    {
        for (int i = 0; i < transform.Find("helmets").childCount; i++)
        {
            if (!transform.Find("helmets").GetChild(i).gameObject.activeSelf)
            {
                transform.Find("helmets").GetChild(i).gameObject.SetActive(true);
            }
        }
        yield return new WaitForSeconds(aresTime);
        StartCoroutine(aresHelmet());
    }
}

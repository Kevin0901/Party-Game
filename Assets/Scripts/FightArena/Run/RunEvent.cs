using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunEvent : MonoBehaviour
{
    [SerializeField] private GameObject UI, typeMachine;
    private void OnEnable()
    {
        SetPlayer();
    }
    private void SetPlayer()
    {
        for (int i = 0; i < FightManager.Instance.plist.Count; i++)
        {
            FightManager.Instance.plist[i].GetComponent<arenaPlayer>().enabled = false;
            FightManager.Instance.plist[i].transform.rotation = Quaternion.Euler(0, 0, -90);
            switch (i)
            {
                case 0:
                    FightManager.Instance.plist[i].GetComponent<arenaPlayer>().SpawnPoint(new Vector3(-60, 25, 0));
                    break;
                case 1:
                    FightManager.Instance.plist[i].GetComponent<arenaPlayer>().SpawnPoint(new Vector3(-60, 7.5f, 0));
                    break;
                case 2:
                    FightManager.Instance.plist[i].GetComponent<arenaPlayer>().SpawnPoint(new Vector3(-60, -10, 0));
                    break;
                case 3:
                    FightManager.Instance.plist[i].GetComponent<arenaPlayer>().SpawnPoint(new Vector3(-60, -27.5f, 0));
                    break;
            }
        }
    }
    public void startGame()
    {
        for (int i = 0; i < FightManager.Instance.plist.Count; i++)
        {
            GameObject a = Instantiate(typeMachine, FightManager.Instance.plist[i].transform.position,
            typeMachine.transform.rotation);
            a.transform.SetParent(FightManager.Instance.plist[i].transform);
        }
    }
    public IEnumerator EndGame(GameObject winner)
    {
        for (int i = 0; i < FightManager.Instance.plist.Count; i++)
        {
            FightManager.Instance.plist[i].GetComponentInChildren<typeMove>().enabled = false;
        }
        yield return new WaitForSeconds(3f);
        UI.SetActive(true);
        if (winner.GetComponent<arenaPlayer>().red)
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

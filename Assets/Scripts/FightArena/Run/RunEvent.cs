using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunEvent : MonoBehaviour
{
    [SerializeField] private GameObject UI, typeMachine;
    public void startGame()
    {
        for (int i = 0; i < FightManager.Instance.plist.Count; i++)
        {
            FightManager.Instance.plist[i].GetComponentInChildren<typeMove>().enabled = true;
        }
    }
    private void OnEnable()
    {
        SetPlayer();
    }
    private void SetPlayer()
    {
        for (int i = 0; i < FightManager.Instance.plist.Count; i++)
        {
            FightManager.Instance.plist[i].GetComponent<arenaPlayer>().enabled = false;
            Instantiate(typeMachine, FightManager.Instance.plist[i].transform);
            FightManager.Instance.plist[i].transform.localScale = new Vector3(2.8f, 2.8f, 0);
            FightManager.Instance.plist[i].transform.rotation = Quaternion.Euler(0, 0, -90);
            switch (i + 1)
            {
                case 1:
                    FightManager.Instance.plist[i].GetComponent<arenaPlayer>().SpawnPoint(new Vector3(-60, 25, 0));
                    break;
                case 2:
                    FightManager.Instance.plist[i].GetComponent<arenaPlayer>().SpawnPoint(new Vector3(-60, 7.5f, 0));
                    break;
                case 3:
                    FightManager.Instance.plist[i].GetComponent<arenaPlayer>().SpawnPoint(new Vector3(-60, -10, 0));
                    break;
                case 4:
                    FightManager.Instance.plist[i].GetComponent<arenaPlayer>().SpawnPoint(new Vector3(-60, -27.5f, 0));
                    break;
            }
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerEvent : MonoBehaviour
{
    [SerializeField] private GameObject UI, _hammer;
    public bool isEnd;
    private void OnEnable()
    {
        SetPlayer();
    }
    private void SetPlayer()
    {
        for (int i = 0; i < FightManager.Instance.plist.Count; i++)
        {
            FightManager.Instance.plist[i].GetComponent<arenaPlayer>().enabled = false;
            FightManager.Instance.plist[i].transform.rotation = Quaternion.Euler(0, 0, -180);
            switch (i)
            {
                case 0:
                    FightManager.Instance.plist[i].GetComponent<arenaPlayer>().SpawnPoint(new Vector3(-45, 28, 0));
                    break;
                case 1:
                    FightManager.Instance.plist[i].GetComponent<arenaPlayer>().SpawnPoint(new Vector3(-15, 28, 0));
                    break;
                case 2:
                    FightManager.Instance.plist[i].GetComponent<arenaPlayer>().SpawnPoint(new Vector3(15, 28, 0));
                    break;
                case 3:
                    FightManager.Instance.plist[i].GetComponent<arenaPlayer>().SpawnPoint(new Vector3(45, 28, 0));
                    break;
            }
            GameObject a = Instantiate(_hammer, FightManager.Instance.plist[i].transform.position,
            FightManager.Instance.plist[i].transform.rotation);
            a.transform.SetParent(FightManager.Instance.plist[i].transform);
            a.transform.GetChild(0).GetComponent<strike>()._event = this.gameObject.GetComponent<HammerEvent>();
        }
    }
    public void StartGame()
    {
        for (int i = 0; i < FightManager.Instance.plist.Count; i++)
        {
            FightManager.Instance.plist[i].GetComponentInChildren<strike>().enabled = true;
        }
    }
    public IEnumerator EndGame(GameObject winner)
    {
        isEnd = true;
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

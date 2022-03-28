using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerEvent : MonoBehaviour
{
    [SerializeField] private GameObject UI, _hammer;
    public void StartGame()
    {
        for (int i = 0; i < FightManager.Instance.plist.Count; i++)
        {
            FightManager.Instance.plist[i].GetComponent<arenaPlayer>().enabled = false;
            GameObject a = Instantiate(_hammer, FightManager.Instance.plist[i].transform);
            a.GetComponent<strike>()._event = this.gameObject;
            FightManager.Instance.plist[i].transform.localScale = new Vector3(2.8f, 2.8f, 0);
            FightManager.Instance.plist[i].transform.rotation = Quaternion.Euler(0, 0, -180);

            switch (i)
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
            Destroy(FightManager.Instance.plist[i].transform.Find("hammer").GetComponent<strike>());
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
        for (int i = 0; i < FightManager.Instance.plist.Count; i++)
        {
            FightManager.Instance.plist[i].GetComponent<arenaPlayer>().enabled = true;
            FightManager.Instance.plist[i].SetActive(false);
            Destroy(FightManager.Instance.plist[i].transform.Find("hammer"));
        }
        this.gameObject.SetActive(false);
    }
}

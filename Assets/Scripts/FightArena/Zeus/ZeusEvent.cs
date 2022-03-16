using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZeusEvent : MonoBehaviour
{
    [SerializeField] private GameObject UI;
    // Start is called before the first frame update
     public void StartGame()
    {
        this.GetComponent<ZeusEvent>().enabled = true;
        for (int i = 0; i < FightManager.Instance.gamelist.Count; i++)
        {
            FightManager.Instance.gamelist[i].GetComponent<arenaPlayer>().currentState = ArenaState.lighting;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (FightManager.Instance.gamelist.Count <= 1)
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
            FightManager.Instance.gamelist[0].SetActive(false);
            this.gameObject.SetActive(false);
        }
    }
}

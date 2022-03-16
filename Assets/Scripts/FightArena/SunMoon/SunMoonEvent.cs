using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunMoonEvent : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float size = 0.05f;
    [SerializeField] private GameObject UI;
    //開始遊戲
    public void StartGame()
    {
        StartCoroutine(changeBG());
        this.GetComponent<SunMoonEvent>().enabled = true;
        for (int i = 0; i < FightManager.Instance.gamelist.Count; i++)
        {
            FightManager.Instance.gamelist[i].GetComponent<arenaPlayer>().currentState = ArenaState.shoot;
        }
    }
    //改變背景大小
    private IEnumerator changeBG()
    {
        if (this.transform.localScale.x != 0)
        {
            this.transform.localScale = new Vector3(this.transform.localScale.x - Time.deltaTime * size,
                                                            this.transform.localScale.y - Time.deltaTime * size,
                                                            this.transform.localScale.z - Time.deltaTime * size);
        }
        yield return null;
        StartCoroutine(changeBG());
    }
    private void Update()
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
    //玩家離開地圖判定
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == 10)
        {
            other.gameObject.SetActive(false);
        }
    }
}

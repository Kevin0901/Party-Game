using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class countdown : MonoBehaviour
{
    // private arenaController ac;
    public int countdownTime;
    private int saveTime;
    public Text countdownDisplay;
    // Start is called before the first frame update
    void Start()
    {
        saveTime = countdownTime;
    }
    public void startcountdown(GameObject g)
    {
        StartCoroutine(startgame(g));
    }
    private IEnumerator startgame(GameObject game)
    {
        while (countdownTime > 0)
        {
            countdownDisplay.text = countdownTime.ToString();
            yield return new WaitForSeconds(1f);
            countdownTime -= 1;
        }
        if (countdownTime == 0)
        {
            countdownTime = saveTime;
            // ac = GameObject.Find("FightGameManager").GetComponent<arenaController>();
            // ac.nowgame = game;
            Instantiate(game, new Vector3(0, 0, 0), Quaternion.identity);
            this.gameObject.SetActive(false);
        }

    }
}

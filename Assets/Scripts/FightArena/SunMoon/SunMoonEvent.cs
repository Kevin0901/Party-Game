using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunMoonEvent : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private float size;
    void Start()
    {

        // game = GameObject.Find("FightGameManager").GetComponent<arenaController>();
        // p = GameObject.Find("playerManager").GetComponent<playerlist>();
        // for (int i = 0; i < p.player.Count; i++)
        // {
        //     p.player[i].GetComponent<arenaPlayer>().currentState = ArenaState.walk;
        //     p.player[i].GetComponent<playerShoot>().enabled = true;
        // }
        // bg = GameObject.Find("background");
        // bg.GetComponent<PolygonCollider2D>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        // if (game.isover)
        // {
        // bg.transform.localScale = new Vector3(8, 8, 8);
        // for (int i = 0; i < p.bornplayer.Count; i++)
        // {
        //     p.bornplayer[i].GetComponent<playerShoot>().enabled = false;
        // }
        // Destroy(this.gameObject, 0.1f);
        // }
    }
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
    public void StartGame()
    {
        StartCoroutine(changeBG());
    }
}

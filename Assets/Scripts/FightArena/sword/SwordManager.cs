using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordManager : MonoBehaviour
{
    public GameObject player;
    [HideInInspector] public playerlist p;
    private float max;
    private arenaController game;
    // Start is called before the first frame update
    void Start()
    {
        game = GameObject.Find("FightGameManager").GetComponent<arenaController>();
        p = GameObject.Find("playerManager").GetComponent<playerlist>();
        max = p.player.Count;
        for (int i = 0; i < p.player.Count; i++)
        {
            p.player[i].GetComponent<arenaPlayer>().currentState = ArenaState.walk;
        }
        Invoke("randomSword", 1f);
    }
    private void Update()
    {
        if (game.isover)
        {
            for (int i = 0; i < p.bornplayer.Count; i++)
            {
                p.bornplayer[i].transform.Find("sword").gameObject.SetActive(false);
            }
            Destroy(this.gameObject, 0.1f);
        }
        if (max > p.player.Count)
        {
            Invoke("randomSword", 1f);
            max = p.player.Count;
        }
    }
    void randomSword()
    {
        int k = Random.Range(0, p.player.Count);
        player = p.player[k];
        player.transform.Find("sword").gameObject.SetActive(true);
    }
}

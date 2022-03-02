using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class die : MonoBehaviour
{
    // Start is called before the first frame update
    private arenaController game;
    [HideInInspector] public playerlist p;
    void Start()
    {
        game = GameObject.Find("FightGameManager").GetComponent<arenaController>();
        p = GameObject.Find("playerManager").GetComponent<playerlist>();
    }

    // Update is called once per frame
    void Update()
    {
        if (game.isover)
        {
            for (int i = 0; i < p.player.Count; i++)
            {
                p.bornplayer[i].transform.Find("shield").gameObject.SetActive(false);
            }
            Destroy(this.gameObject);
        }
    }
}

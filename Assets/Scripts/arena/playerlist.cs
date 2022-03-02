using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerlist : MonoBehaviour
{
    // Start is called before the first frame update
    public List<GameObject> player, bornplayer = new List<GameObject>();
    private void Start()
    {

    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == 10)
        {
            player.Remove(other.gameObject);
        }
    }

    public void setbornplayer()
    {
        for (int i = 0; i < player.Count; i++)
        {
            bornplayer.Add(player[i]);
        }
    }
}

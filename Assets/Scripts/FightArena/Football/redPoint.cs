using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class redPoint : MonoBehaviour
{
    public GameObject scoreADD;
    //紅隊球門被得分
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("arrow"))
        {
            scoreADD.GetComponent<FootEvent>().B_Score();
            StartCoroutine(other.gameObject.GetComponent<football>().waitBall());
        }
        else if (other.gameObject.layer == 10)
        {
            if (other.gameObject.GetComponent<arenaPlayer>().red)
                scoreADD.GetComponent<FootEvent>().B_Score();
        }
    }
}

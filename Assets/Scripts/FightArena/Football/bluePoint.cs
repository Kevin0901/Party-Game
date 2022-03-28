using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bluePoint : MonoBehaviour
{
    public GameObject scoreADD;
    //藍隊球門被得分
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("arrow"))
        {
            scoreADD.GetComponent<FootEvent>().red_Score();
            StartCoroutine(other.gameObject.GetComponent<football>().waitBall());
        }
        else if (other.gameObject.layer == 10)
        {
            if (!other.gameObject.GetComponent<arenaPlayer>().red)
                scoreADD.GetComponent<FootEvent>().red_Score();
        }
    }
}

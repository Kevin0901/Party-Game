using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class redPoint : MonoBehaviour
{
    public GameObject parent;
    //紅隊球門被得分
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("arrow"))
        {
            parent.GetComponent<FootEvent>().blue_Score();
            other.gameObject.GetComponent<football>().StartCoroutine("waitBall");
        }
        else if (other.gameObject.layer == 10)
        {
            if (other.gameObject.GetComponent<arenaPlayer>().red)
                parent.GetComponent<FootEvent>().blue_Score();
        }
    }
}

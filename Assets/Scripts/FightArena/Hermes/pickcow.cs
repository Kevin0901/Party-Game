using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickcow : MonoBehaviour
{
    [HideInInspector]public int cowScore;
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == 10)
        {
            if (other.GetComponent<arenaPlayer>().red)
            {
                this.transform.parent.GetComponent<HermesEvent>().R_ScoreADD(cowScore);
                Destroy(this.gameObject);
            }
            else
            {
                this.transform.parent.GetComponent<HermesEvent>().B_ScoreADD(cowScore);
                Destroy(this.gameObject);
            }
        }
    }
}

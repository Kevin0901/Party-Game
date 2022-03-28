using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickcow : MonoBehaviour
{
    public int cowScore;
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == 10)
        {
            if (other.GetComponent<arenaPlayer>().red)
            {
                this.transform.parent.GetComponent<HermesEvent>().red_Score(cowScore);
                Destroy(this.gameObject);
            }
            else
            {
                this.transform.parent.GetComponent<HermesEvent>().blue_Score(cowScore);
                Destroy(this.gameObject);
            }
        }
    }
}

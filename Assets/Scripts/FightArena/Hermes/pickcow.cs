using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickcow : MonoBehaviour
{
    [HideInInspector] public GameObject parent;
    public int cowScore;
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == 10)
        {
            if (other.GetComponent<arenaPlayer>().red)
            {
                parent.GetComponent<HermesEvent>().red_Score(cowScore);
                Destroy(this.gameObject);
            }
            else
            {
                parent.GetComponent<HermesEvent>().blue_Score(cowScore);
                Destroy(this.gameObject);
            }
        }
    }
    private void Update()
    {
        if (parent.GetComponent<HermesEvent>()._time < 0){
             Destroy(this.gameObject);
        }
    }
}

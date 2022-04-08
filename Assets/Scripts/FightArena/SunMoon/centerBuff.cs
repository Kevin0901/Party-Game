using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class centerBuff : MonoBehaviour
{
    //在正中心
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == 10)
        {
            other.GetComponent<arenaPlayer>().isCenter = true;
        }
    }
    //離開正中心
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == 10)
        {
            other.GetComponent<arenaPlayer>().isCenter = false;
        }
    }
}

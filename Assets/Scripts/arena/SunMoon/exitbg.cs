using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class exitbg : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == 10)
        {
            other.gameObject.GetComponent<arenaPlayer>().hurt(3);
        }
    }
}

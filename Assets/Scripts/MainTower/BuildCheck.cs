using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildCheck : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == 10)
        {
            other.GetComponent<PlayerMovement>().intower = 1;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == 10)
        {
            other.GetComponent<PlayerMovement>().intower = 0;
        }
    }
}

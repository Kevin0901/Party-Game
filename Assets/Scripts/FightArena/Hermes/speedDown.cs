using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class speedDown : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 10)
        {
            other.GetComponent<arenaPlayer>().turboSpeed /= 5.5f;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == 10)
        {
            other.GetComponent<arenaPlayer>().turboSpeed *= 5.5f;
        }
    }
}

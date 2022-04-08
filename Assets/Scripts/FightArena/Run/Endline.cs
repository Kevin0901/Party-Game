using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Endline : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == 10)
        {
           StartCoroutine(this.transform.GetComponentInParent<RunEvent>().EndGame(other.gameObject));
        }
    }
}

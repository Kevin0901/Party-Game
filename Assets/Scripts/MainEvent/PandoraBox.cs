using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PandoraBox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 10)
        {
            other.gameObject.GetComponent<Effect>().StartCoroutine("RandGiveEffect");
        }
    }
}

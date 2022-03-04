using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cupid_Die : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 10)
        {
            this.transform.parent.gameObject.SetActive(false);
        }
    }
}

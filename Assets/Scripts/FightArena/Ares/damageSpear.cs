using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damageSpear : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 10)
        {
            other.GetComponent<arenaPlayer>().hurt(1.5f);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damageSpear : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == 10)
        {
            other.gameObject.GetComponent<arenaPlayer>().hurt(1.5f);
        }
    }
}
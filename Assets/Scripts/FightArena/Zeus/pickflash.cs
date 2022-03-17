using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickflash : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == 10)
        {
            if (other.GetComponent<arenaPlayer>().isGetLighting != true)
            {
                other.GetComponent<arenaPlayer>().isGetLighting = true;
                Destroy(this.gameObject);
            }
        }
    }
}

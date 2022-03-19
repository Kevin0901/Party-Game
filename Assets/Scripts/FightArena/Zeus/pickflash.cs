using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickflash : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == 10)
        {
            if (other.GetComponent<arenaPlayer>().currentState != ArenaState.lighting)
            {
                other.GetComponent<arenaPlayer>().currentState = ArenaState.lighting;
                Destroy(this.gameObject);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickflash : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == 10 && other.GetComponent<arenaPlayer>().currentState != ArenaState.lighting)
        {
            other.GetComponent<arenaPlayer>().currentState = ArenaState.lighting;
            other.GetComponent<arenaPlayer>().titleColor.color = new Color(1, 1, 0);
            Destroy(this.gameObject);
        }
    }
}

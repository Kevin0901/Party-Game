using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class helmet : MonoBehaviour
{
    public bool isArens;
    public float aresT;
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == 10 && Input.GetKey(KeyCode.Space))
        {
            if (isArens)
            {

                other.GetComponent<arenaPlayer>().StartCoroutine("ChangeARES", aresT);
                this.transform.parent.gameObject.SetActive(false);
                this.transform.parent.GetComponentInParent<AresEvent>().StartCoroutine("reset");
            }
            else
            {
                this.transform.gameObject.SetActive(false);
            }
        }
    }
}

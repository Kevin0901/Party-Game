using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damageSpear : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log(this.GetComponentInParent<spear>().isAttack);
        if (other.gameObject.layer == 10 && this.GetComponentInParent<spear>().isAttack)
        {
            Debug.Log(this.GetComponentInParent<spear>().isAttack);
            other.gameObject.GetComponent<arenaPlayer>().hurt(1f);
            this.GetComponentInParent<spear>().isAttack = false;
        }
    }
}

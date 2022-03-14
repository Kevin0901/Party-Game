using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickshield : MonoBehaviour
{
    //盾牌拾取
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == 10 && !other.gameObject.transform.Find("shield").gameObject.activeSelf)
        {
            other.gameObject.transform.Find("shield").gameObject.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }
  
}

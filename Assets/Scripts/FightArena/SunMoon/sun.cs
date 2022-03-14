using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sun : MonoBehaviour
{
    //在太陽上
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("arrow"))
        {
            other.GetComponent<SunMoonArrowMove>().isSun = true;
            other.GetComponent<SunMoonArrowMove>().noEffect = false;
        }
    }
    //離開太陽
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("arrow"))
        {
            other.GetComponent<SunMoonArrowMove>().noEffect = true;
        }
    }
}

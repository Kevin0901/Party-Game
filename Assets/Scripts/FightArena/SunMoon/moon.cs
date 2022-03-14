using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moon : MonoBehaviour
{
    //在月亮上
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("arrow"))
        {
            other.GetComponent<SunMoonArrowMove>().isSun = false;
            other.GetComponent<SunMoonArrowMove>().noEffect = false;
        }
    }
    //離開月亮
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("arrow"))
        {
            other.GetComponent<SunMoonArrowMove>().noEffect = true;
        }
    }
}

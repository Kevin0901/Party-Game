using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class centerBuff : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == 10)
        {
            other.GetComponent<playerShoot>().isCenter = true;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("arrow"))
        {
            other.GetComponent<SunMoonArrowMove>().power += 50;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == 10)
        {
            other.GetComponent<playerShoot>().isCenter = false;
        }
        else if (other.CompareTag("arrow")){
            other.GetComponent<SunMoonArrowMove>().power -= 50;
        }
    }
}

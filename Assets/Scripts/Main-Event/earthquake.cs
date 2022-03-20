using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Earthquake : MonoBehaviour
{
    private bool WaitQuake = false;


    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == 11)
        {
            // Debug.Log(other.gameObject.name);

            StartCoroutine(CameraShakeAndDestoryself());

            if (WaitQuake)
            {
                Destroy(other.gameObject);
            }
        }
        else
        {
            StartCoroutine(waitfordestory());
        }
    }

    IEnumerator CameraShakeAndDestoryself()
    {
        CameraShake.canshake = true;
        yield return new WaitForSeconds(2);
        WaitQuake = true;
        yield return new WaitForSeconds(2);
        CameraShake.canshake = false;
        Destroy(this.gameObject);
    }

    IEnumerator waitfordestory()
    {
        yield return new WaitForSeconds(5);
        if (!WaitQuake)
        {
            Destroy(this.gameObject);
        }
    }
}

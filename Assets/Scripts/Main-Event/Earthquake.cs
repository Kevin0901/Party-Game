using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Earthquake : MonoBehaviour
{
    private bool WaitQuake = false;

    private void Start()
    {
        StartCoroutine(CameraShakeAndDestoryself());
    }
    private void OnTriggerStay2D(Collider2D other)
    {

        if (other.gameObject.layer == 11)
        {
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
        CameraShake.cansand = true;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class earthquake : MonoBehaviour
{
    private List<GameObject> enemylist;
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == 11)
        {
            // Debug.Log(other.gameObject.name);
            Destroy(other.gameObject);
            StartCoroutine(CameraShakeAndDestoryself());
        }
    }

    IEnumerator CameraShakeAndDestoryself()
    {
        yield return new WaitForSeconds(2);
        Destroy(this);
    }
}

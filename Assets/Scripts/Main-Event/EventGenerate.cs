using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventGenerate : MonoBehaviour
{
    [SerializeField] private GameObject tsu;
    [SerializeField] private GameObject eq;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Vector3 tsupos = new Vector3(-18.0f, -66.0f, 0.0f);
            Instantiate(tsu, tsupos, tsu.transform.rotation);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Vector3 eqpos = new Vector3(0f, 0f, 0f);
            StartCoroutine(Beforeearthquakesay(eqpos));
        }

        IEnumerator Beforeearthquakesay(Vector3 pos)
        {

            yield return new WaitForSeconds(2);
            Instantiate(eq, pos, eq.transform.rotation);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawnshield : MonoBehaviour
{
    [SerializeField] private GameObject shield;
    private void OnDisable()
    {
        Instantiate(shield, new Vector3(Random.Range(-25, 25), Random.Range(-25, 25), 0), shield.transform.rotation);
    }
}

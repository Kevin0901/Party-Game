using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawnshield : MonoBehaviour
{
    //如果盾牌被打掉就生成盾牌
    [SerializeField] private GameObject shield;
    private void OnDisable()
    {
        Instantiate(shield, new Vector3(Random.Range(-25, 25), Random.Range(-25, 25), 0), shield.transform.rotation);
    }
}

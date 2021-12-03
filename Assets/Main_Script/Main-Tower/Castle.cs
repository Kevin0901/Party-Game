using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle : MonoBehaviour
{
    [SerializeField] public int MaxHealth, CurHealth;
    private health health;
    private void Awake()
    {
        health = transform.Find("HealthBar").GetComponent<health>();
        health.maxH = MaxHealth;
    }
    private void Update()
    {
        CurHealth = health.curH;
    }
}

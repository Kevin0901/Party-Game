﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shootflash : MonoBehaviour
{
    [HideInInspector]public GameObject shooter;
    [HideInInspector]public float damege;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 10 && other.gameObject != shooter)
        {
            other.GetComponent<arenaPlayer>().hurt(damege);
        }
    }
}
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team : MonoBehaviour
{
    private enum color
    {
        red,
        blue
    }
    // Start is called before the first frame update
    public string Enemyteam; //敵人
    private void Awake()
    {
        if (this.gameObject.tag == "red")
        {
            Enemyteam = "blue";
        }
        else if (this.gameObject.tag == "blue")
        {
            Enemyteam = "red";
        }
    }
    private void Start()
    {
        if (this.gameObject.tag == "red")
        {
            Enemyteam = "blue";
        }
        else if (this.gameObject.tag == "blue")
        {
            Enemyteam = "red";
        }
    }
}

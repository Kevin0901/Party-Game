using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Team : MonoBehaviour
{
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeTeam : MonoBehaviour
{
    //簡易換隊
    private void Update()
    {
        if (Input.GetButtonDown("Horizontal"))
        {
            choose();
        }
    }
    private void choose()
    {
        if (this.transform.Find("RedTeam").gameObject.activeSelf)
        {
            this.transform.Find("RedTeam").gameObject.SetActive(false);
            this.transform.Find("BlueTeam").gameObject.SetActive(true);
        }
        else
        {
            this.transform.Find("RedTeam").gameObject.SetActive(true);
            this.transform.Find("BlueTeam").gameObject.SetActive(false);
        }
    }
}
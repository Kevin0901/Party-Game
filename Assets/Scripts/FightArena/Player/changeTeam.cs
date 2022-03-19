using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeTeam : MonoBehaviour
{
    private GameObject ui;
    private int num;
    private void Start()
    {
        num = this.GetComponent<arenaPlayer>().p_index;
        ui = GameObject.Find("ChoosePlayer").transform.Find("P" + (num + 1)).gameObject;
    }
    private void Update()
    {
        if (Input.GetButtonDown("Horizontal") || Input.GetMouseButtonDown(0))
        {
            choose();
        }
    }
    public void choose()
    {
        if (ui.transform.Find("RedTeam").gameObject.activeSelf)
        {
            ui.transform.Find("RedTeam").gameObject.SetActive(false);
            ui.transform.Find("BlueTeam").gameObject.SetActive(true);
        }
        else
        {
            ui.transform.Find("RedTeam").gameObject.SetActive(true);
            ui.transform.Find("BlueTeam").gameObject.SetActive(false);
        }
    }
}

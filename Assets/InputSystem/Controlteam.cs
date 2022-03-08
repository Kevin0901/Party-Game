using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Controlteam : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject ui;
    PlayerInput controls;
    private void Awake()
    {
        controls = this.GetComponent<PlayerInput>();
        controls.actionEvents[1].AddListener(choose);
        ui = GameObject.Find("ChoosePlayer").transform.Find("P" + (controls.playerIndex + 1)).gameObject;
        if (ui.transform.Find("BlueTeam").gameObject.activeSelf)
        {
            ui.transform.Find("RedTeam").gameObject.SetActive(true);
            ui.transform.Find("BlueTeam").gameObject.SetActive(false);
        }
    }
    public void choose(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
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
    public void removeChoose()
    {
        controls.actionEvents[1].RemoveListener(choose);
    }
}

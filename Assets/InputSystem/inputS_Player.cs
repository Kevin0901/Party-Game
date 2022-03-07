using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class inputS_Player : MonoBehaviour
{
    private PlayerInput gamePlayer;
    public bool red;
    public GameObject ui;
    void Awake()
    {
        red = true;
        gamePlayer = this.GetComponent<PlayerInput>();
        ui = GameObject.Find("ChoosePlayer").transform.Find("P" + (gamePlayer.playerIndex + 1)).gameObject;
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
                red = false;
            }
            else
            {
                ui.transform.Find("RedTeam").gameObject.SetActive(true);
                ui.transform.Find("BlueTeam").gameObject.SetActive(false);
                red = true;
            }
        }
    }

}

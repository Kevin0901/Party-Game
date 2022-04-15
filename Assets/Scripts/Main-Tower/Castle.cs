using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Castle : MonoBehaviour
{
    [SerializeField] public int MaxHealth, CurHealth;
    private health health;

    [SerializeField] GameObject RedEndScreen, BlueEndScreen;
    [SerializeField] GameObject BackButton;
    PhotonView PV;
    bool IsEnd;
    private void Awake()
    {
        IsEnd = false;
        health = transform.Find("HealthBar").GetComponent<health>();
        health.maxH = MaxHealth;
        PV = GetComponent<PhotonView>();
    }
    private void Update()
    {
        CurHealth = health.curH;
        if (!IsEnd && CurHealth <= 0)
        {
            End();
            IsEnd = true;
        }
    }
    void End()
    {
        if (CurHealth <= 0 && this.tag.Equals("red"))
        {
            if (!RedEndScreen.activeSelf)
            {
                RedEndScreen.SetActive(true);
                if (PV.IsMine)
                {
                    BackButton.SetActive(true);
                }
            }
        }
        else if (CurHealth <= 0 && this.tag.Equals("blue"))
        {
            if (!BlueEndScreen.activeSelf)
            {
                BlueEndScreen.SetActive(true);
                if (PV.IsMine)
                {
                    BackButton.SetActive(true);
                }
            }
        }
    }
    public void Back()
    {
        GameObject.Find("RoomManager").GetComponent<RoomManager>().Back_To_Main();
    }
}

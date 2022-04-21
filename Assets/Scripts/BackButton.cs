using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackButton : MonoBehaviour
{
    public void Back()
    {
        GameObject.Find("RoomManager").GetComponent<RoomManager>().Back_To_Main();
    }
}

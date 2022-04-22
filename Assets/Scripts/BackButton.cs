using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BackButton : MonoBehaviour
{
    public void Back()
    {
        StartCoroutine(backMenu());
    }
    IEnumerator backMenu()
    {
        Time.timeScale = 1;
        yield return null;
        GameObject.Find("RoomManager").GetComponent<RoomManager>().Back_To_Main();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CreateRoomNameRandom : MonoBehaviour
{
    [SerializeField]TMP_InputField InputText;
    private void OnEnable()
    {
        InputText.text = "Room " + Random.Range(0,9999).ToString("0000");
    }
}

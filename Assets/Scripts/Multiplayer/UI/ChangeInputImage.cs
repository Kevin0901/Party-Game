using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ChangeInputImage : MonoBehaviour
{
    TMP_InputField Input;
    Image Im;
    [SerializeField] Sprite DefultIm;
    [SerializeField] Sprite InputIm;
    // Start is called before the first frame update
    void Start()
    {
        Input = this.GetComponent<TMP_InputField>();
        Im = this.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.text.Equals(""))
        {
            Im.sprite = DefultIm;
        }
        else
        {
            Im.sprite = InputIm;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class heart : MonoBehaviour
{
    [SerializeField] private List<Sprite> sprites;
    //玩家受到傷害，改變UI顯示
    public void hurt(float hp)
    {
        switch (hp)
        {
            case 3f:
                this.transform.GetChild(0).Find("heart-1").GetComponent<Image>().sprite = sprites[0];
                this.transform.GetChild(0).Find("heart-2").GetComponent<Image>().sprite = sprites[0];
                this.transform.GetChild(0).Find("heart-3").GetComponent<Image>().sprite = sprites[0];
                break;
            case 2.5f:
                this.transform.GetChild(0).Find("heart-3").GetComponent<Image>().sprite = sprites[1];
                break;
            case 2f:
                this.transform.GetChild(0).Find("heart-3").GetComponent<Image>().sprite = sprites[2];
                break;
            case 1.5f:
                this.transform.GetChild(0).Find("heart-3").GetComponent<Image>().sprite = sprites[2];
                this.transform.GetChild(0).Find("heart-2").GetComponent<Image>().sprite = sprites[1];
                break;
            case 1f:
                this.transform.GetChild(0).Find("heart-2").GetComponent<Image>().sprite = sprites[2];
                break;
            case 0.5f:
                this.transform.GetChild(0).Find("heart-2").GetComponent<Image>().sprite = sprites[2];
                this.transform.GetChild(0).Find("heart-1").GetComponent<Image>().sprite = sprites[1];
                break;
            case 0:
                this.transform.GetChild(0).Find("heart-1").GetComponent<Image>().sprite = sprites[2];
                this.transform.GetChild(0).Find("heart-2").GetComponent<Image>().sprite = sprites[2];
                this.transform.GetChild(0).Find("heart-3").GetComponent<Image>().sprite = sprites[2];
                break;
            default:
                this.transform.GetChild(0).Find("heart-1").GetComponent<Image>().sprite = sprites[2];
                this.transform.GetChild(0).Find("heart-2").GetComponent<Image>().sprite = sprites[2];
                this.transform.GetChild(0).Find("heart-3").GetComponent<Image>().sprite = sprites[2];
                break;
        }
    }
}
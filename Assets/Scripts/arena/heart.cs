using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class heart : MonoBehaviour
{
    [SerializeField] private List<Sprite> sprites;
    private GameObject player;
    // Start is called before the first frame update
    public float curH;
    void Start()
    {
        curH = 3f;
    }
    public void hurt(float damege)
    {
        curH -= damege;
        switch (curH)
        {
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
                player.SetActive(false);
                break;
        }
    }
    public void SetPlayer(GameObject p)
    {
        player = p;
    }

    public void fullHealth()
    {
        curH = 3f;
        this.transform.GetChild(0).Find("heart-1").GetComponent<Image>().sprite = sprites[0];
        this.transform.GetChild(0).Find("heart-2").GetComponent<Image>().sprite = sprites[0];
        this.transform.GetChild(0).Find("heart-3").GetComponent<Image>().sprite = sprites[0];
    }
}
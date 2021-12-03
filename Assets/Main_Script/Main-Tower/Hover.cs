using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Hover : Singleton<Hover>
{
    private Image image;

    public static int bhover = 0;
    public static int mhover = 0;
    public static Vector3 hitpos;
    private GameObject SpawnObject;
    public Camera thiscamera;
    private string playertag;
    private Vector3 imageScale;



    void Start()
    {
        this.image = GetComponentInChildren<Image>();
        this.gameObject.SetActive(false);
        playertag = this.transform.parent.tag;
        imageScale = this.image.transform.localScale;
    }

    void Update()
    {
        if (bhover == 1)
        {
            if (playertag == "red")
            {
                if ((ResourceManager.Instance.RedCanAfford(SpawnObject.GetComponent<TowerData>().CostArray) == false) || (ColliderCheck.rangename != "territory"))
                {
                    if (PlaceTower.tc != 0)
                    {
                        this.image.color = new Color32(255, 0, 0, 180);
                        image.transform.localScale = imageScale;
                    }
                }
                else
                {
                    this.image.color = new Color32(255, 255, 255, 180);
                    image.transform.localScale = imageScale;
                }
            }
            else if (playertag == "blue")
            {
                if ((ResourceManager.Instance.BlueCanAfford(SpawnObject.GetComponent<TowerData>().CostArray) == false) || (ColliderCheck.rangename != "territory"))
                {
                    if (PlaceTower.tc != 0)
                    {
                        this.image.color = new Color32(255, 0, 0, 180);
                        image.transform.localScale = imageScale;
                    }
                }
                else
                {
                    this.image.color = new Color32(255, 255, 255, 180);
                    image.transform.localScale = imageScale;
                }
            }
        }
        else if (mhover == 1)
        {
            if (playertag == "red")
            {
                if ((ResourceManager.Instance.RedCanAfford(SpawnObject.GetComponent<monsterMove>().CostArray) == false) || (ColliderCheck.rangename != "territory"))
                {
                    if (PlaceTower.tc != 0)
                    {
                        this.image.color = new Color32(255, 0, 0, 180);
                        image.GetComponent<RectTransform>().localScale = new Vector3(imageScale.x * 1, imageScale.y / 1.5f);
                    }
                }
                else
                {
                    this.image.color = new Color32(255, 255, 255, 180);
                    image.GetComponent<RectTransform>().localScale = new Vector3(imageScale.x * 1, imageScale.y / 1.5f);
                }
            }
            else if (playertag == "blue")
            {
                if ((ResourceManager.Instance.BlueCanAfford(SpawnObject.GetComponent<monsterMove>().CostArray) == false) || (ColliderCheck.rangename != "territory"))
                {
                    if (PlaceTower.tc != 0)
                    {
                        this.image.color = new Color32(255, 0, 0, 180);
                        image.GetComponent<RectTransform>().localScale = new Vector3(imageScale.x * 1, imageScale.y / 1.5f);
                    }
                }
                else
                {
                    this.image.color = new Color32(255, 255, 255, 180);
                    image.GetComponent<RectTransform>().localScale = new Vector3(imageScale.x * 1, imageScale.y / 1.5f);
                }
            }
        }
        FollowMouse();
    }
    private void FollowMouse()
    {
        image.GetComponent<RectTransform>().position = new Vector3(thiscamera.ScreenToWorldPoint(Input.mousePosition).x, thiscamera.ScreenToWorldPoint(Input.mousePosition).y, 0);
        hitpos = new Vector3(thiscamera.ScreenToWorldPoint(Input.mousePosition).x, thiscamera.ScreenToWorldPoint(Input.mousePosition).y, 0);
    }


    public void Activate(Sprite sprite, GameObject objectperfab)
    {
        this.image.sprite = sprite;
        SpawnObject = objectperfab;
        if (UIState.Instance.b == 1)
        {
            bhover = 1;
        }
        else if (UIState.Instance.m == 1)
        {
            mhover = 1;
        }
        if (UIState.escc == 1)
        {
            UIState.escc = 0;
        }

    }

}







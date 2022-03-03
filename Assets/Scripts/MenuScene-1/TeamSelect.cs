using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamSelect : MonoBehaviour
{
    public bool red, blue;
    private int playsort;
    private ChoosePlayer chooseP;

    // Start is called before the first frame update
    void Start()
    {
        chooseP = this.transform.parent.GetComponent<ChoosePlayer>();
        transform.Find("leftArrow").gameObject.SetActive(true);
        transform.Find("rightArrow").gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (chooseP.CanvasGroup.blocksRaycasts)
        {
            if (Input.GetButtonDown("Vertical"))
            {
                if (red)
                {
                    blue = true;
                    red = false;
                }
                else
                {
                    blue = false;
                    red = true;
                }
            }
            TeamChoose();
        }
    }
    public void Left()//按鈕
    {
        if (chooseP.CanvasGroup.blocksRaycasts)
        {
            if (red)
            {
                blue = true;
                red = false;
            }
            else
            {
                blue = false;
                red = true;
            }
        }
    }
    public void Right()//按鈕
    {
        if (chooseP.CanvasGroup.blocksRaycasts)
        {
            if (red)
            {
                blue = true;
                red = false;
            }
            else
            {
                blue = false;
                red = true;
            }
        }
    }
    public void TeamChoose()
    {
        if (red)
        {
            transform.Find("red").gameObject.SetActive(true);
            transform.Find("RedPlayer").gameObject.SetActive(true);
            transform.Find("blue").gameObject.SetActive(false);
            transform.Find("BluePlayer").gameObject.SetActive(false);
        }
        else if (blue)
        {
            transform.Find("blue").gameObject.SetActive(true);
            transform.Find("BluePlayer").gameObject.SetActive(true);
            transform.Find("red").gameObject.SetActive(false);
            transform.Find("RedPlayer").gameObject.SetActive(false);
        }
    }
}

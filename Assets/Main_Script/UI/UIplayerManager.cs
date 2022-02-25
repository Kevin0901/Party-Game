using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIplayerManager : MonoBehaviour
{
    public bool iconjudge;
    public bool red, blue;
    public int playersort;
    public string Joysticknum;
    private ChoosePlayer CP;

    // Start is called before the first frame update
    void Start()
    {
        ResetValue();
        CP = this.transform.parent.GetComponent<ChoosePlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playersort != 0 && CP.CanvasGroup.blocksRaycasts)
        {
            if (Input.GetButtonDown("leftchoose" + Joysticknum))
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
            else if (Input.GetButtonDown("rightchoose" + Joysticknum))
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
        if (CP.CanvasGroup.blocksRaycasts)
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
        if (CP.CanvasGroup.blocksRaycasts)
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

        if (iconjudge)
        {
            if (Joysticknum.Equals("0"))
            {
                transform.Find("leftArrow").gameObject.SetActive(true);
                transform.Find("rightArrow").gameObject.SetActive(true);
            }
            else
            {
                transform.Find("L1").gameObject.SetActive(true);
                transform.Find("R1").gameObject.SetActive(true);
            }
        }
    }
    public void ResetValue()
    {
        playersort = 0;
        Joysticknum = null;
        red = false;
        blue = false;
        iconjudge = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIplayerManager : MonoBehaviour
{
    public bool isChooseTeam, iconjudge;
    public bool red, blue;
    public int playersort;
    public string Joysticknum;
    private ChoosePlayer CP;

    // Start is called before the first frame update
    void Start()
    {
        red = false;
        blue = false;
        isChooseTeam = false;
        iconjudge = false;
        playersort = 0;
        Joysticknum = "";
        CP = this.transform.parent.GetComponent<ChoosePlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isChooseTeam && CP.CanvasGroup.blocksRaycasts)
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
    public void Left()
    {
        if (isChooseTeam && CP.CanvasGroup.blocksRaycasts)
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
    public void Right()
    {
        if (isChooseTeam && CP.CanvasGroup.blocksRaycasts)
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
            // iconjudge = false;
        }
    }
}

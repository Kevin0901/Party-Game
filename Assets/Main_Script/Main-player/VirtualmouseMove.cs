using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VirtualmouseMove : MonoBehaviour
{
    // Start is called before the first frame update
    private RectTransform t;
    private Vector2 change, spwanPoint;
    private float speed = 20f;
    public string num = "1";
    public int totalplayer;
    private float h, w;
    private Canvas canvasRect;
    void Start()
    {
        t = this.transform.GetChild(0).GetComponent<RectTransform>();
        canvasRect = this.gameObject.GetComponent<Canvas>();
        w = Screen.width;
        h = Screen.height;
        if (totalplayer == 2)
        {
            spwanPoint = new Vector2(w / 2, h);
        }
        else
        {
            spwanPoint = new Vector2(w / 2, h / 2);
        }
        t.anchoredPosition = spwanPoint;
    }

    // Update is called once per frame
    void Update()
    {
        change = Vector2.zero;
        change.x = Input.GetAxis("X-virtualMouse" + num) * speed;
        change.y = Input.GetAxis("Y-virtualMouse" + num) * speed;
        if (change != Vector2.zero)
        {
            t.anchoredPosition += change;
            Vector2 clamped = t.anchoredPosition;
            clamped.x = Mathf.Clamp(clamped.x, t.sizeDelta.x * t.pivot.x, w - t.sizeDelta.y * t.pivot.y);
            if (totalplayer == 2)
            {
                clamped.y = Mathf.Clamp(clamped.y, t.sizeDelta.y * t.pivot.y, (h - t.sizeDelta.x * t.pivot.x) * 2);
            }
            else
            {
                clamped.y = Mathf.Clamp(clamped.y, t.sizeDelta.y * t.pivot.y, (h - t.sizeDelta.x * t.pivot.x));
            }
            t.anchoredPosition = clamped;
        }
        if (Input.GetKeyDown("joystick " + num + " button 9"))
        {
            t.anchoredPosition = spwanPoint;
        }
    }
}

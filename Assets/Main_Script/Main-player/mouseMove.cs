using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class mouseMove : MonoBehaviour
{
    // Start is called before the first frame update
    private RectTransform t;
    public int totalplayer;
    private float h, w;
    private Canvas canvasRect;
    void Start()
    {
        t = this.gameObject.transform.GetChild(0).GetComponent<RectTransform>();
        canvasRect = this.gameObject.GetComponent<Canvas>();
        w = Screen.currentResolution.width;
        h = Screen.currentResolution.height;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 movePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect.transform as RectTransform,
            Input.mousePosition, canvasRect.worldCamera,
            out movePos);
        t.transform.position = canvasRect.transform.TransformPoint(movePos);

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
}

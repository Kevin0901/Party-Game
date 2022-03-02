using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMovementController : MonoBehaviour
{
    public float speed = 5.0f;

    new private RectTransform transform;
    private Rect canvasRect;
    
    private void Start()
    {
        transform = GetComponent<RectTransform>();
        canvasRect = GetComponentInParent<Canvas>().pixelRect;
    }

    void Update()
    {
        // Keyboard Input (Arrows)

        Vector2 move = new Vector2(0,0);
        if (Input.GetKey(KeyCode.UpArrow)) { move.y += speed; }
        if (Input.GetKey(KeyCode.DownArrow)) { move.y -= speed; }
        if (Input.GetKey(KeyCode.LeftArrow)) { move.x -= speed; }
        if (Input.GetKey(KeyCode.RightArrow)) { move.x += speed; }
        transform.anchoredPosition += move;

        // Position clamping
        
        Vector2 clamped = transform.anchoredPosition;
        clamped.x = Mathf.Clamp(clamped.x, transform.rect.width / 2,   - transform.rect.width / 2);
        clamped.y = Mathf.Clamp(clamped.y, transform.rect.height / 2, 1080 - transform.rect.height / 2);
        Debug.Log(canvasRect);
        transform.anchoredPosition = clamped;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class item_MoveMovementGamepad : MonoBehaviour
{
    [Header("投擲物參數")]
    private float speed = 10f, dis = 15f;
    private float rotate;
    private Vector3 startPos, dir;
    // private Camera cam;
    private GameObject mouse;
    public string team;
    void Awake()
    {
        team = this.GetComponentInParent<Team>().Enemyteam;
        mouse = this.GetComponentInParent<PlayerMovement>().mouse;
        // cam = this.GetComponentInParent<PlayerMovement>().playercamera;
        setPos();
    }

    // Update is called once per frame
    void Update()
    {
        float a = Vector3.Distance(this.transform.position, startPos);
        if (dis < a)
        {
            Destroy(this.gameObject);
        }
        transform.position += dir.normalized * Time.deltaTime * speed;
    }
    public void setPos()
    {
        RectTransform mPos = mouse.transform.GetChild(0).GetComponent<RectTransform>();
        this.transform.SetParent(null);
        startPos = this.transform.position;
        dir = mPos.position - startPos;
        dir.z = 0;
        rotate = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(rotate, Vector3.forward);
    }
}

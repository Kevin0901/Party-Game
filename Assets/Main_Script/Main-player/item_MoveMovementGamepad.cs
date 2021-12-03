using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class item_MoveMovementGamepad : MonoBehaviour
{
    [Header("投擲物參數")]
    private float speed = 10f, dis = 15f;
    private float rotate;
    private Vector3 worldPosition, startPos, dir;
    private Camera cam;
    private int sort;
    public string team;
    void Awake()
    {
        sort = this.GetComponentInParent<PlayerMovement>().order;
        team = this.GetComponentInParent<Team>().Enemyteam;
        cam = this.transform.parent.transform.Find("Camera").GetComponent<Camera>();
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
        RectTransform mPos = GameObject.Find("UIManager").transform.Find("Mouse").GetChild(sort - 1).GetChild(0).GetComponent<RectTransform>();
        this.transform.SetParent(null);
        startPos = this.transform.position;
        dir = mPos.position - startPos;
        dir.z = 0;
        rotate = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(rotate, Vector3.forward);
    }
}

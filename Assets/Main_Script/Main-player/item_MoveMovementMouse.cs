using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class item_MoveMovementMouse : MonoBehaviour
{
    [Header("投擲物參數")]
    [SerializeField] private float speed = 10f, dis = 15f;
    private float rotate;
    private Vector3 startPos, dir;
    public string team;

    private Vector3 worldPosition, mousePos;
    // Start is called before the first frame update

    private void Awake()
    {
        team = this.GetComponentInParent<Team>().Enemyteam;
        startPos = this.transform.position;
        mousePos = Input.mousePosition;  //得到螢幕滑鼠位置
        worldPosition = Camera.main.ScreenToWorldPoint(mousePos); //遊戲內世界座標滑鼠位置
        dir = worldPosition - startPos;
        dir.z = 0;
        rotate = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(rotate, Vector3.forward);
        this.transform.SetParent(null);
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
}

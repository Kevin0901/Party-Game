using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunMoonArrowMove : MonoBehaviour
{
    public bool isSun, noEffect;
    [HideInInspector] public float speed, power;
    [HideInInspector] public string enemy;
    private float rotate;
    private GameObject parent;
    private Vector3 worldPosition, mousePos, dir;
    // Update is called once per frame
    void Update()
    {
        if (noEffect)
        {
            transform.position += dir.normalized * Time.deltaTime * speed;
        }
        else if (isSun)
        {
            transform.position += dir.normalized * Time.deltaTime * speed * 2f;
        }
        else
        {
            transform.position += dir.normalized * Time.deltaTime * speed * 0.15f;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 10 && other.gameObject != parent)
        {
            other.GetComponent<arenaPlayer>().repeldir = dir;
            other.GetComponent<arenaPlayer>().repelpower = power;
            other.GetComponent<arenaPlayer>().currentState = ArenaState.repel;
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("background"))
        {
            Destroy(this.gameObject);
        }
    }
    public void setArrow(GameObject master)
    {
        Quaternion k = Quaternion.AngleAxis(rotate, Vector3.forward);
        dir = k * Vector3.up;
        parent = master;
        noEffect = true;
        transform.rotation = Quaternion.AngleAxis(rotate + 90, Vector3.forward);
        // mousePos = Input.mousePosition;  //得到螢幕滑鼠位置
        // worldPosition = Camera.main.ScreenToWorldPoint(mousePos); //遊戲內世界座標滑鼠位置
        // dir = worldPosition - transform.position;
        // dir.z = 0;
        // rotate = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
    }
    public void setRotate(float r)
    {
        rotate = r;
    }
}

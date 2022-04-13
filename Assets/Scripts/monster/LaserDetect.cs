using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDetect : MonoBehaviour
{
    private monsterMove m;
    RaycastHit2D[] raycast2D = new RaycastHit2D[3]; //雷射
    LayerMask mask;  //遮罩
    private float angle;
    [Header("發射長度")]
    [SerializeField] private float dis; //發射長度
    public bool isfix; //是否修正
    void Start()
    {
        m = this.gameObject.GetComponent<monsterMove>();
        dis *= this.gameObject.transform.localScale.x;
        mask = 1 << 9 | 1 << 10 | 1 << 11;
        angle = 35;
        isfix = false;
    }
    void fixedUpdate()
    {
        if (m.currentState == MonsterState.track || m.currentState == MonsterState.walk)
        {
            fixPosition();
        }
    }
    // Update is called once per frame
    void fixPosition()
    {
        float x = m.animator.GetFloat("moveX");
        float y = m.animator.GetFloat("moveY");
        Vector2 enemyPos = new Vector2(x, y);
        var line = Vector2.zero - enemyPos;
        float rotate = Mathf.Atan2(line.y, line.x) * Mathf.Rad2Deg;
        Quaternion k = Quaternion.AngleAxis(angle, Vector3.forward);
        Vector3 Left = k * enemyPos;
        k = Quaternion.AngleAxis(-angle, Vector3.forward);
        Vector3 Right = k * enemyPos;

        raycast2D[0] = Physics2D.Raycast(transform.position, Left, dis, mask);
        raycast2D[1] = Physics2D.Raycast(transform.position, enemyPos, dis, mask);
        raycast2D[2] = Physics2D.Raycast(transform.position, Right, dis, mask);
        Debug.DrawRay(transform.position, enemyPos * dis, Color.black);
        Debug.DrawRay(transform.position, Left * dis, Color.black);
        Debug.DrawRay(transform.position, Right * dis, Color.black);

        foreach (RaycastHit2D i in raycast2D)
        {
            if ((i.collider != null) && (i.collider.tag == this.gameObject.tag))
            {
                float posL = Vector3.Distance(i.collider.transform.position, transform.position + Left);
                float posR = Vector3.Distance(i.collider.transform.position, transform.position + Right);
                if (posL > posR)
                {
                    transform.position += Left * m.speed * Time.deltaTime;
                }
                else
                {
                    transform.position += Right * m.speed * Time.deltaTime;
                }
                isfix = true;
                break;
            }
            else
            {
                isfix = false;
            }
        }
    }
}


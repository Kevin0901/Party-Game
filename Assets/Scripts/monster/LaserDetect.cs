using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserDetect : MonoBehaviour
{
    private monsterMove m;
    RaycastHit2D[] h = new RaycastHit2D[3]; //雷射
    LayerMask mask;  //遮罩
    [Header("發射長度")]
    [SerializeField] private float dis = 1.5f; //發射長度
    private float x, y;
    private Vector3 enemyPos, Left, Right;
    private float rotate, angle;
    public bool isfix; //是否修正

    void Start()
    {
        m = this.gameObject.GetComponent<monsterMove>();
        dis *= this.gameObject.transform.localScale.x;
        mask = 1 << 9 | 1 << 10 | 1 << 11;
        angle = 35;
        isfix = false;
    }
    void Update()
    {
        if (m.currentState == MonsterState.track || m.currentState == MonsterState.walk)
        {
            fixPosition();
        }
    }
    // Update is called once per frame
    void fixPosition()
    {
        x = m.animator.GetFloat("moveX");
        y = m.animator.GetFloat("moveY");

        enemyPos = new Vector2(x, y);
        Vector2 line = Vector3.zero - enemyPos;
        rotate = Mathf.Atan2(line.y, line.x) * Mathf.Rad2Deg;
        Quaternion k = Quaternion.AngleAxis(angle, Vector3.forward);
        Left = k * enemyPos;
        k = Quaternion.AngleAxis(-angle, Vector3.forward);
        Right = k * enemyPos;

        h[0] = Physics2D.Raycast(transform.position, Left, dis, mask);
        h[1] = Physics2D.Raycast(transform.position, enemyPos, dis, mask);
        h[2] = Physics2D.Raycast(transform.position, Right, dis, mask);
        Debug.DrawRay(transform.position, enemyPos * dis, Color.black);
        Debug.DrawRay(transform.position, Left * dis, Color.black);
        Debug.DrawRay(transform.position, Right * dis, Color.black);

        foreach (RaycastHit2D i in h)
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


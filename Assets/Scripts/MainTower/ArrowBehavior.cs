using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using Photon.Pun;
public class ArrowBehavior : MonoBehaviour
{
    public float speed;
    public float damage;
    public GameObject target;
    public Vector3 startPosition;
    public Vector3 targetPosition;
    private Vector2 line;
    private float rotate;
    // PhotonView PV;
    void Awake()
    {
        // PV = GetComponent<PhotonView>();
        // GameObject tower = PhotonView.Find((int)PV.InstantiationData[0]).gameObject;
        // damage = tower.GetComponent<ArrowShoot>().towerData.CurrentLevel.damage;
        // this.gameObject.tag = tower.tag;
    }
    void Start()
    {
    }

    void Update()
    {
        // if (!PV.IsMine)
        // {
        //     return;
        // }
        if (target == null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            line = startPosition - targetPosition;
            rotate = Mathf.Atan2(line.y, line.x) * Mathf.Rad2Deg;
            this.transform.rotation = Quaternion.AngleAxis(rotate, Vector3.forward);
            this.transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * speed);

            if (gameObject.transform.position.Equals(targetPosition))
            {
                if (target != null)
                {
                    target.GetComponentInChildren<health>().Hurt((int)damage);
                }
                Destroy(gameObject);
            }
        }

    }
}

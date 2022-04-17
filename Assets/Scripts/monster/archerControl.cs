using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class archerControl : MonoBehaviour
{
    [SerializeField] private GameObject Arrow;
    [SerializeField] private float ArrowSpeed;
    [SerializeField] private int damege;
    private Animator animator;
    private void Awake()
    {
        animator = this.gameObject.GetComponentInParent<Animator>();
    }
    public void OnEnable()
    {
        GameObject arr = Instantiate(Arrow, this.transform.position, Arrow.transform.rotation);
        if (animator.GetFloat("moveY") == 1)       //上
        {
            arr.transform.position += new Vector3(-0.03f, 1.6f, 0);
            arr.transform.rotation = Quaternion.AngleAxis(-94.36f, Vector3.forward);
        }
        else if (animator.GetFloat("moveY") == -1) //下
        {
            arr.transform.position += new Vector3(0.03f, -0.98f, 0);
            arr.transform.rotation = Quaternion.AngleAxis(76.793f, Vector3.forward);
        }
        else if (animator.GetFloat("moveX") == 1)  //右
        {
            arr.transform.position += new Vector3(1.48f, 0.24f, 0);
            arr.transform.rotation = Quaternion.AngleAxis(180f, Vector3.forward);
        }
        else if (animator.GetFloat("moveX") == -1) //左
        {
            arr.transform.position += new Vector3(-1.48f, 0.24f, 0);
        }
        arr.GetComponent<archerArrowMove>().speed = ArrowSpeed;
        arr.GetComponent<archerArrowMove>().damege = damege;
        arr.GetComponent<archerArrowMove>().target = this.GetComponentInParent<monsterMove>().enemy;
    }
}

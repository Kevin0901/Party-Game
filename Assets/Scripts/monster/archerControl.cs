using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class archerControl : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject Arrow;
    [SerializeField] private float speed, damege;
    private Animator animator;//動畫
    private void Awake()
    {
        animator = this.gameObject.GetComponentInParent<Animator>();
    }
    private void OnEnable()
    {
        GameObject arr = Instantiate(Arrow, this.transform.position, Arrow.transform.rotation);
        if (animator.GetFloat("moveY") == 1)
        {
            arr.transform.position += new Vector3(0.03f, 1.32f, 0);
        }
        else if (animator.GetFloat("moveY") == -1)
        {
            arr.transform.position += new Vector3(0.053f, -1.355f, 0);
        }
        else if (animator.GetFloat("moveX") == 1)
        {
            arr.transform.position += new Vector3(1.428f, -0.126f, 0);
        }
        else if (animator.GetFloat("moveX") == -1)
        {
            arr.transform.position += new Vector3(-1.366f, -0.086f, 0);
        }
        arr.GetComponent<archerArrowMove>().speed = speed;
        arr.GetComponent<archerArrowMove>().damege = damege;
        arr.GetComponent<archerArrowMove>().target = this.GetComponentInParent<monsterMove>().enemy;
    }
}

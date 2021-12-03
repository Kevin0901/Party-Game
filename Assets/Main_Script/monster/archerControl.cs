using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class archerControl : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject Arrow;
    private Animator animator;//動畫
    private void Awake()
    {
        animator = this.gameObject.GetComponentInParent<Animator>();
    }
    private void OnEnable()
    {
        if (animator.GetFloat("moveY") == 1)
        {
            GameObject arr = Instantiate(Arrow, this.transform.position + new Vector3(0.03f, 1.32f, 0), Arrow.transform.rotation);
            arr.transform.SetParent(this.transform.parent);
        }
        else if(animator.GetFloat("moveY") == -1)
        {
            GameObject arr = Instantiate(Arrow, this.transform.position + new Vector3(0.053f, -1.355f, 0), Arrow.transform.rotation);
            arr.transform.SetParent(this.transform.parent);
        }
        else if(animator.GetFloat("moveX") == 1)
        {
            GameObject arr = Instantiate(Arrow, this.transform.position + new Vector3(1.428f, -0.126f, 0), Arrow.transform.rotation);
            arr.transform.SetParent(this.transform.parent);
        }
        else if(animator.GetFloat("moveX") == -1)
        {
            GameObject arr = Instantiate(Arrow, this.transform.position + new Vector3(-1.366f, -0.086f, 0), Arrow.transform.rotation);
            arr.transform.SetParent(this.transform.parent);
        }
    }
}

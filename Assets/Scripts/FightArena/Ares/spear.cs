using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spear : MonoBehaviour
{
    private Animator mAnimator;
    public GameObject player;
    private void Start()
    {
        mAnimator = GetComponent<Animator>();
    }
    private void Update()
    {
        aresSpear();
    }
    //戰神揮矛
    private void aresSpear()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mAnimator.SetTrigger("rotate");
        }
    }
    public void LateUpdate()
    {
        transform.position = player.transform.position;
    }
}

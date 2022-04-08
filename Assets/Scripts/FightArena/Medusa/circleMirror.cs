using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class circleMirror : MonoBehaviour
{
    //旋轉鏡子
    void FixedUpdate()
    {
        this.transform.Rotate(0, 0, 0.7f);
    }
}

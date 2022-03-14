using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumSetNoRotation : MonoBehaviour
{
    //讓頭上的標籤不要跟著轉動
    private void LateUpdate()
    {   
        transform.rotation = Quaternion.identity;
    }
}

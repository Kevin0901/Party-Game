using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumSetNoRotation : MonoBehaviour
{
    private void LateUpdate()
    {   
        transform.rotation = Quaternion.identity;
    }
}

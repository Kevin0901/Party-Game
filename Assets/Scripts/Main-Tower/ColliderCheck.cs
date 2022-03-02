using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderCheck : MonoBehaviour
{

    public static ColliderCheck Instance { get; private set; }

    public static string rangename;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        Vector3 mpos = Hover.hitpos;
        if (Hover.bhover == 1)
        {
            CanSpawnBuilding(mpos);
        }

    }

    private void CanSpawnBuilding(Vector3 position)
    {
        Collider2D collider2D = Physics2D.OverlapPoint(position);
        if (collider2D != null)
        {
            rangename = collider2D.name;
        }

    }

}

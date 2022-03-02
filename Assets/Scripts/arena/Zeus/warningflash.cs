using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class warningflash : MonoBehaviour
{
    // Start is called before the first frame update
    SpriteRenderer sprite;
    private byte a;
    void Start()
    {
        a = 0;
        sprite = GetComponent<SpriteRenderer>();
        sprite.color = new Color32(111,1,1,0);
        InvokeRepeating("Changea",0,0.015f);
    }

    // Update is called once per frame
    void Update()
    {
        if(a < 75)
        {
            sprite.color = new Color32(111,1,1,a);
        }
        else
        {
            sprite.color = new Color32(111,1,1,75);
        }
        
    }
    void Changea()
    {
        a += 3;
    }
}

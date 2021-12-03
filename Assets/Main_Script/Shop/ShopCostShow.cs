using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopCostShow : MonoBehaviour
{
    public ResourceAmount[] costarray;
    private int c = 0;

    // Update is called once per frame
    void Update()
    {
        foreach (ResourceAmount resourceAmount in costarray)
        {
            transform.GetChild(c).gameObject.GetComponent<Text>().text = resourceAmount.amount.ToString();
            c++;
        }
        c = 0;
    }
}

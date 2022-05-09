using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CostShow : MonoBehaviour
{
    private GameObject parentobject;
    private ResourceAmount[] costarray;
    private TowerBtn towerBtn;
    private int c = 0;
    private void Awake()
    {
        costarray = new ResourceAmount[3];
    }
    void Start()
    {
        parentobject = this.transform.parent.parent.parent.parent.parent.GetChild(0).gameObject;
        if (parentobject.GetComponent<UIState>().b == 1)
        {
            costarray = gameObject.transform.parent.GetComponent<TowerBtn>().ObjectPrefab.GetComponent<TowerData>().CostArray;
            foreach (ResourceAmount resourceAmount in costarray)
            {
                this.transform.GetChild(c).GetComponent<Text>().text = resourceAmount.amount.ToString();
                c++;
            }
            c = 0;
        }


        if (parentobject.GetComponent<UIState>().m == 1)
        {
            costarray = gameObject.transform.parent.GetComponent<TowerBtn>().ObjectPrefab.GetComponent<monsterMove>().CostArray;
            foreach (ResourceAmount resourceAmount in costarray)
            {
                this.transform.GetChild(c).GetComponent<Text>().text = resourceAmount.amount.ToString();
                c++;
            }
            c = 0;
        }
        // foreach (ResourceAmount resourceAmount in costarray)
        // {
        //     this.transform.GetChild(c).GetComponent<Text>().text = resourceAmount.amount.ToString();
        //     c++;
        // }
        // c = 0;
    }

}

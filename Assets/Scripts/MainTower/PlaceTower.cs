using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PlaceTower : MonoBehaviour
{
    [SerializeField]
    private GameObject territory;
    private GameObject tower, monster;
    private Vector3 orgin;
    public static int tc;

    private void Start()
    {
        tc = 0;
    }

    // private void OnMouseDown()
    // {
    //     orgin = territory.transform.position;
    //     if (GameManager.a != 0 && UIState.Instance.b != 0)
    //     {
    //         tower = GameManager.perfab;
    //         if (this.tag == "red")
    //         {
    //             if (ResourceManager.Instance.RedCanAfford(tower.GetComponent<TowerData>().CostArray) != false)
    //             {
    //                 ResourceManager.Instance.RedSpendResources(tower.GetComponent<TowerData>().CostArray);
    //                 placetower(tower);
    //             }
    //             else
    //             {
    //                 Debug.Log("can't spawn");
    //             }
    //         }
    //         else if (this.tag == "blue")
    //         {
    //             if (ResourceManager.Instance.BlueCanAfford(tower.GetComponent<TowerData>().CostArray) != false)
    //             {
    //                 ResourceManager.Instance.BlueSpendResources(tower.GetComponent<TowerData>().CostArray);
    //                 placetower(tower);
    //             }
    //             else
    //             {
    //                 Debug.Log("can't spawn");
    //             }
    //         }

    //     }

    //     if (GameManager.a != 0 && UIState.Instance.m != 0)
    //     {
    //         monster = GameManager.perfab;
    //         if (this.tag == "red")
    //         {
    //             if (ResourceManager.Instance.RedCanAfford(monster.GetComponent<monsterMove>().CostArray) != false)
    //             {
    //                 ResourceManager.Instance.RedSpendResources(monster.GetComponent<monsterMove>().CostArray);
    //                 placetower(monster);
    //             }
    //             else
    //             {
    //                 Debug.Log("can't spawn");
    //             }
    //         }
    //         else if (this.tag == "blue")
    //         {
    //             if (ResourceManager.Instance.BlueCanAfford(monster.GetComponent<monsterMove>().CostArray) != false)
    //             {
    //                 ResourceManager.Instance.BlueSpendResources(monster.GetComponent<monsterMove>().CostArray);
    //                 placetower(monster);
    //             }
    //             else
    //             {
    //                 Debug.Log("can't spawn");
    //             }
    //         }

    //     }


    // }

    public void placetower(GameObject spawnobject)
    {
        // transform.position = Camera.current.ScreenToWorldPoint(Input.mousePosition);
        // transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        // Instantiate(spawnobject, Hover.hitpos, Quaternion.identity);
        // transform.position = orgin;
        // tc = 1;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == 10 && other.gameObject.tag + this.tag == this.gameObject.name)
        {
            other.GetComponent<PlayerMovement>().interritory = 1;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == 10 && other.gameObject.tag + this.tag == this.gameObject.name)
        {
            other.GetComponent<PlayerMovement>().interritory = 0;
        }
    }
}

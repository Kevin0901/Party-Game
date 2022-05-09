using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerBtn : MonoBehaviour
{
    [SerializeField]
    private GameObject objectPrefab;
    private Sprite sprite;


    public GameObject ObjectPrefab
    {
        get
        {
            return objectPrefab;
        }
    }


    public Sprite Sprite
    {
        get
        {
            return objectPrefab.GetComponent<SpriteRenderer>().sprite;
        }
    }

    private void Start()
    {
        this.transform.GetChild(1).gameObject.GetComponent<Image>().sprite = Sprite;
    }
}

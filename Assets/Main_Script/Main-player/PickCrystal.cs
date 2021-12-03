using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickCrystal : MonoBehaviour
{
    private PlayerMovement player;
    private void Awake()
    {
        ResourceTypeListSO resourceTypeList = Resources.Load<ResourceTypeListSO>(typeof(ResourceTypeListSO).Name);
    }
    private void Start()
    {
        player = this.GetComponentInParent<PlayerMovement>();
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        ResourceTypeListSO resourceTypeList = Resources.Load<ResourceTypeListSO>(typeof(ResourceTypeListSO).Name);
        if (other.CompareTag("pick") && Input.GetButtonDown("pick" + player.joynum))
        {
            if (player.tag == "red")
            {
                ResourceManager.Instance.RedAddResource(resourceTypeList.list[3], 3);
            }
            else if (player.tag == "blue")
            {
                ResourceManager.Instance.BlueAddResource(resourceTypeList.list[3], 3);
            }
            Destroy(other.gameObject);
        }
    }
}

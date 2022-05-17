using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
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
            for(int i =0 ;i<resourceTypeList.list.Count-1 ; i++){
                if(other.gameObject.name+" (ResourceTypeSo)" == resourceTypeList.list[i].name)
                if (player.tag == "red")
                {
                    ResourceManager.Instance.RedAddResource(resourceTypeList.list[i], 3);
                    PhotonNetwork.Destroy(other.gameObject);
                }else if (player.tag == "blue")
                {
                    ResourceManager.Instance.BlueAddResource(resourceTypeList.list[i], 3);
                    PhotonNetwork.Destroy(other.gameObject);
                } 
            } 
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
public class GoldenApple : MonoBehaviour
{
    [SerializeField] private GameObject Horse;
    PhotonView PV;
    bool isCalled = false;
    private void Start()
    {
        PV = this.GetComponent<PhotonView>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 10)
        {
            other.gameObject.GetComponent<Effect>().StartCoroutine("PowerUPEffect");
            if(PV.IsMine)
            {
                RandCallHorse(other.gameObject);
            }
        }
    }

    void RandCallHorse(GameObject Player)
    {
        int randnum = Random.Range(1, 11);
        Debug.Log(randnum);
        if (randnum < 6)
        {
            // Horse.GetComponent<TrojanHorse>().TargetTeam = Player.tag;
            // Instantiate(Horse, transform.position, transform.rotation);
            GameObject horse =  PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "MainEvent/TrojanHorse"), new Vector3(-20, 0, 0), this.transform.rotation);
            horse.GetComponent<TrojanHorse>().TargetTeam = Player.tag;
        }
        PhotonNetwork.Destroy(this.gameObject);
    }
}

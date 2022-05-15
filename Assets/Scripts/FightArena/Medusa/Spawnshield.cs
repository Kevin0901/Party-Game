using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
public class Spawnshield : MonoBehaviour
{
    //如果盾牌被打掉就生成盾牌
    [SerializeField] private GameObject shield;
    public void spawnS()
    {
        // Instantiate(shield, new Vector3(Random.Range(-25, 25), Random.Range(-25, 25), 0), shield.transform.rotation);
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "arena/Medusa/pickShield"), new Vector3(Random.Range(-25, 25), Random.Range(-25, 25), 0), this.transform.rotation);
    }
}

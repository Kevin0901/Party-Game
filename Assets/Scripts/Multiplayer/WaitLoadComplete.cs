using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
public class WaitLoadComplete : MonoBehaviour
{
    PhotonView PV;
    // Start is called before the first frame update
    void Start()
    {
        PV = this.GetComponent<PhotonView>();
        if(SceneManager.GetActiveScene().name.Equals("MainScene"))
        {
            transform.SetParent(GameObject.Find("LoadSceneCompletePool").transform);
        }
        else if(SceneManager.GetActiveScene().name.Equals("FightScene"))
        {
            transform.SetParent(GameObject.Find("LoadFightSceneCompletePool").transform);
        }
    }
}

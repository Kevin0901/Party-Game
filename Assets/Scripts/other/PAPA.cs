using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PAPA : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    public void OpenChild()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            if(this.transform.GetChild(i).gameObject.name.Equals("Teaching"))
            {

            }else
            {
                this.transform.GetChild(i).gameObject.SetActive(true);
            }
        }
    }
}

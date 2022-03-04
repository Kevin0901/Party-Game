using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goldensheep : MonoBehaviour
{
    [SerializeField]
    private string bonusteam;
    private bool nowtstatic = true;
    private GameObject player;
    private int playoncatheal;


    private void Update()
    {

        if (nowtstatic == false)
        {
            if (player.GetComponent<PlayerMovement>().health.playercatchsheeponhit != 0)
            {
                Debug.Log("res1");
                if (bonusteam == "blue")
                {
                    ResourceManager.Instance.Brestimes = 1;        
                }
                else if (bonusteam == "red")
                {
                    ResourceManager.Instance.Rrestimes = 1;
                }
                nowtstatic = true;
                bonusteam = null;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("in");
        if (other.gameObject.layer == 10 && Input.GetKeyDown(KeyCode.P) && nowtstatic == true)
        {
            Debug.Log("Pin");
            nowtstatic = false;
            player = other.gameObject;
            bonusteam = player.tag;
            player.GetComponent<PlayerMovement>().health.playercatchsheeponhit = 0;
            playoncatheal = player.GetComponent<PlayerMovement>().health.playercatchsheeponhit;
            if (bonusteam == "blue")
            {
                ResourceManager.Instance.Brestimes = 2;
            }
            else if (bonusteam == "red")
            {
                ResourceManager.Instance.Rrestimes = 2;
            }
            // if (m.EnemyList.Count == 0)
            // {
            //     m.EnemyList.Add(other.gameObject);

            // }
            // else if (other.gameObject == m.EnemyList[0])
            // {
            //     dis = Vector3.Distance(parent.transform.position, other.transform.position); //兩座標之間的距離長度
            //     if (dis <= m.attackRange)
            //     {
            //         m.currentState = MonsterState.attack;
            //     }
            //     else if (m.currentState != MonsterState.attack)
            //     {
            //         m.currentState = MonsterState.track;
            //     }
            //     m.enemyPos = other.transform.position;
            // }
        }
    }


}

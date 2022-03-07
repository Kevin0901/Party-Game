using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class goldensheep : MonoBehaviour
{
    [SerializeField]
    private string bonusteam;
    [SerializeField]
    private float speed;
    private bool nowtstatic = true;
    private GameObject player;
    private int playoncatheal;
    private int mc = 0;
    private Vector3 targetpos;
    private int fc = 0;
    private List<Vector3> playerposlist = new List<Vector3>();

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
                playerposlist.Add(player.transform.position);
            if(playerposlist.Count > 50){
                playerposlist.RemoveAt(0);
                transform.position = playerposlist[0];
            }   
        }

        

        if (nowtstatic == true)
        {           
            if (mc == 0)
            {
                if (fc == 0)
                {
                    fc = 1;
                    float x = Random.Range(-40, -0.75f);
                    float y = Random.Range(-11, 11);
                    targetpos = new Vector3(x, y, 0);
                    Debug.Log(targetpos);
                }
                transform.position = Vector3.MoveTowards(transform.position, targetpos, Time.deltaTime * speed);
                
                if (transform.position == targetpos)
                {
                    mc = 1;
                    StartCoroutine(movecolddown());
                }

                
            }
        }
    }

    IEnumerator movecolddown()
    {
        yield return new WaitForSeconds(3);
        fc = 0;
        mc = 0;
    }

    IEnumerator gobackcolddown()
    {
        yield return new WaitForSeconds(10);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == 10 && Input.GetKeyDown(KeyCode.P) && nowtstatic == true)
        {
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
        }
    }


}

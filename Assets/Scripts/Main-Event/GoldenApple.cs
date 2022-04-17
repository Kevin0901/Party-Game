using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldenApple : MonoBehaviour
{
    [SerializeField] private GameObject Horse;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 10)
        {
            other.gameObject.GetComponent<Effect>().StartCoroutine("PowerUPEffect");
            RandCallHorse(other.gameObject);
        }
    }

    void RandCallHorse(GameObject Player)
    {
        int randnum = Random.Range(1, 11);
        Debug.Log(randnum);
        if (randnum < 6)
        {
            Horse.GetComponent<TrojanHorse>().TargetTeam = Player.tag;
            Instantiate(Horse, transform.position, transform.rotation);
        }
    }
}

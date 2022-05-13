using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
public class TrojanHorse : MonoBehaviour
{
    public string TargetTeam = "red";
    [SerializeField] private GameObject TargetBlue, TargetRed;
    [SerializeField] private Sprite TargetUp, TargetDown;
    private GameObject SoliderGenerator;
    public float speed = 5f;
    private health health;
    [SerializeField] private int MaxHealth, CurHealth;
    [SerializeField] private float damagepersen = 20;
    private float dir;
    public GameObject target;
    private float direction;
    PhotonView PV;

    void Start()
    {
        PV = GetComponent<PhotonView>();
        health = this.GetComponentInChildren<health>();
        if (TargetTeam == "red")
        {
            dir = -1f;
            SoliderGenerator = TargetRed;
            this.GetComponent<SpriteRenderer>().sprite = TargetDown;
        }
        else if (TargetTeam == "blue")
        {
            dir = 1f;
            SoliderGenerator = TargetBlue;
            this.GetComponent<SpriteRenderer>().sprite = TargetUp;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!PV.IsMine)
        {
            return;
        }
        if (target == null)
        {
            transform.position += new Vector3(0, speed, 0) * Time.deltaTime * dir;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, Time.deltaTime * speed);
            direction = Vector3.Distance(target.transform.position, transform.position);

            if (direction < 2)
            {
                target.GetComponentInChildren<health>().curH -= (int)(target.GetComponentInChildren<health>().maxH * (damagepersen / 100));
                // Instantiate(SoliderGenerator, target.transform.position, SoliderGenerator.transform.rotation);
                SpawnSoldier();
                PhotonNetwork.Destroy(this.gameObject);
            }
        }

        if (this.GetComponentInChildren<health>().curH < 50)
        {
            // Instantiate(SoliderGenerator, this.transform.position, SoliderGenerator.transform.rotation);
            SpawnSoldier();
            PhotonNetwork.Destroy(this.gameObject);
        }
    }

    void SpawnSoldier()
    {
        if (TargetTeam == "red")
        {
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "MainEvent/SoliderTargetIsRed"), target.transform.position, this.transform.rotation);
        }
        else if (TargetTeam == "blue")
        {
            PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "MainEvent/SoliderTargetIsBlue"), target.transform.position, this.transform.rotation);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 11 && other.tag == TargetTeam && PV.IsMine)
        {
            target = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == target && PV.IsMine)
        {
            target = null;
        }
    }

}

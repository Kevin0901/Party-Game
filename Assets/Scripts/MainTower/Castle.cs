using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class Castle : MonoBehaviour
{
    [SerializeField] public int MaxHealth, CurHealth;
    public health health;

    [SerializeField] GameObject RedEndScreen, BlueEndScreen;
    [SerializeField] GameObject BackButton;
    bool IsEnd;
    private void Awake()
    {
        IsEnd = false;
        health = transform.Find("HealthBar").GetComponent<health>();
        health.maxH = MaxHealth;
        this.transform.SetParent(GameObject.Find("PAPA").transform);
    }
    void Start()
    {
        GameObject UI = GameObject.Find("PAPA").transform.Find("Canvas").gameObject;
        RedEndScreen = UI.transform.Find("Red").gameObject;
        BlueEndScreen = UI.transform.Find("Blue").gameObject;
        BackButton = UI.transform.Find("Button").gameObject;
    }
    private void Update()
    {
        if (health != null)
        {
            CurHealth = health.curH;
            if (!IsEnd && CurHealth <= 0)
            {
                End();
                IsEnd = true;
            }
        }
        else
        {
            Debug.Log(this.transform.childCount);
        }
    }
    void End()
    {
        Cursor.visible = true;
        if (CurHealth <= 0 && this.tag.Equals("blue"))
        {
            if (!RedEndScreen.activeSelf)
            {
                RedEndScreen.SetActive(true);
                BackButton.SetActive(true);
                RedEndScreen.transform.Find("Image").GetComponent<Animator>().SetTrigger("RedWin");
            }
        }
        else if (CurHealth <= 0 && this.tag.Equals("red"))
        {
            if (!BlueEndScreen.activeSelf)
            {
                BlueEndScreen.SetActive(true);
                BackButton.SetActive(true);
                BlueEndScreen.transform.Find("Image").GetComponent<Animator>().SetTrigger("BlueWin");
            }
        }
        StartCoroutine(StopALL());
    }

    IEnumerator StopALL()
    {
        yield return new WaitForSeconds(1f);
        PhotonNetwork.MinimalTimeScaleToDispatchInFixedUpdate = 1f;
        Time.timeScale = 0;
    }
    private IEnumerator bluewin()
    {
        Animator animator = BlueEndScreen.transform.Find("Image").GetComponent<Animator>();
        animator.SetBool("BlueWin", true);
        yield return null;
        animator.SetBool("BlueWin", false);
    }
    private IEnumerator redwin()
    {
        Animator animator = RedEndScreen.transform.Find("Image").GetComponent<Animator>();
        animator.SetBool("RedWin", true);
        yield return null;
        animator.SetBool("RedWin", false);
    }
}

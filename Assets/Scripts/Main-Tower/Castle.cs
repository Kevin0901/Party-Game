using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
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
        for (int i = 0; i < this.transform.parent.childCount; i++)
        {
            string name = transform.parent.GetChild(i).gameObject.name;
            if (!name.Equals("background") && !name.Equals("Canvas") && name.Equals("Player&Camera (Clone)")
            && this.gameObject && !name.Equals("EventSystem"))
            {
                transform.parent.GetChild(i).gameObject.SetActive(false);
            }
        }
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
    public void Back()
    {
        GameObject.Find("RoomManager").GetComponent<RoomManager>().Back_To_Main();
    }
}

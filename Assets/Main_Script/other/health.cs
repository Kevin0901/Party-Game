using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class health : MonoBehaviour
{
    // Start is called before the first frame update
    public int maxH = 30, curH;
    private GameObject parentSet;
    private RectTransform proportion;
    private float newScale, baseScale;
    private Slider bar;
    private int burnstack = 0;
    public bool iswudi;//無敵

    void Start()
    {
        iswudi = false;
        if (this.gameObject.transform.parent.gameObject.layer != 14)
        {
            healthBarSet();
        }
        curH = maxH;
        bar = this.gameObject.GetComponentInChildren<Slider>();
        parentSet = this.transform.parent.gameObject;
        bar.maxValue = maxH;
    }
    private void Update()
    {
        if (curH <= 0 && parentSet.layer != 10 && parentSet.layer != 14)
        {
            Destroy(parentSet);
        }
        bar.value = curH;
    }
    public void Hurt(int damageToGive)
    {
        if (parentSet.layer == 10 && curH - damageToGive <= 0 && parentSet.GetComponent<PlayerMovement>().phfeather == 1)
        {
            StartCoroutine(phoenix());
        }
        else if (!iswudi)
        {
            curH -= damageToGive;
        }

    }
    public void burnhurt(int sec, int secdamage)
    {
        if (burnstack == 0)
        {
            burnstack = 1;
            StartCoroutine(burn(sec, secdamage));
        }

    }
    private IEnumerator phoenix()
    {
        parentSet.GetComponent<PlayerMovement>().phfeatheruse = 1;
        for (int i = 0; i < 3; i++)
        {
            yield return new WaitForSeconds(1);
        }
        parentSet.GetComponent<PlayerMovement>().phfeather -= 1;

    }
    private IEnumerator burn(int sec, int secdamage)
    {
        for (int i = 0; i < sec; i++)
        {
            Hurt(secdamage);
            yield return new WaitForSeconds(1);
        }
        burnstack = 0;
    }
    void healthBarSet()
    {
        // baseTop = proportion.anchoredPosition.y;
        // newTop = baseTop / 1.3f;
        // proportion.anchoredPosition3D = new Vector3(0, newTop, 0);
        //大小設定
        proportion = this.gameObject.GetComponent<RectTransform>();
        baseScale = 0.01f;
        newScale = baseScale / gameObject.transform.parent.localScale.x;
        if (newScale != baseScale)
        {
            proportion.localScale = new Vector3(newScale, newScale, newScale);
        }
        if (this.gameObject.transform.parent.tag == "blue")
        {
            this.gameObject.transform.GetChild(0).GetChild(1).gameObject.GetComponent<Image>().color = new Color32(44, 43, 178, 255);
        }
    }
    public bool lowHpToDie(int damege)
    {
        if (curH - damege < 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

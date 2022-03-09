using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class health : MonoBehaviour
{
    // Start is called before the first frame update
    public int maxH, curH;
    private GameObject parentSet;
    private RectTransform proportion;
    private float newScale, baseScale;
    private Slider bar;
    private int burnstack = 0;
    public bool iswudi;//無敵
    public int playercatchsheeponhit = 0;
    private void Awake()
    {
        ResourceTypeListSO resourceTypeList = Resources.Load<ResourceTypeListSO>(typeof(ResourceTypeListSO).Name);
    }
    void Start()
    {
        iswudi = false;
        healthBarSet();
        curH = maxH;
        parentSet = this.transform.parent.gameObject;
        bar = this.gameObject.GetComponentInChildren<Slider>();
        bar.maxValue = maxH;
    }
    private void Update()
    {
        if (curH <= 0)
        {
            ResourceTypeListSO resourceTypeList = Resources.Load<ResourceTypeListSO>(typeof(ResourceTypeListSO).Name); //死掉加對方資源
            if (parentSet.tag == "red")
            {
                ResourceManager.Instance.BlueAddResource(resourceTypeList.list[0], 3);
                ResourceManager.Instance.BlueAddResource(resourceTypeList.list[1], 3);
                ResourceManager.Instance.BlueAddResource(resourceTypeList.list[2], 3);
            }
            else if (parentSet.tag == "blue")
            {
                ResourceManager.Instance.RedAddResource(resourceTypeList.list[0], 3);
                ResourceManager.Instance.RedAddResource(resourceTypeList.list[1], 3);
                ResourceManager.Instance.RedAddResource(resourceTypeList.list[2], 3);
            }
        }
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
            StartCoroutine(phoenix(3));
        }
        else if (!iswudi)
        {
            curH -= damageToGive;
        }

        if (parentSet.layer == 10)
        {
            playercatchsheeponhit = 1;
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
    private IEnumerator phoenix(int t) //無敵
    {
        parentSet.GetComponent<PlayerMovement>().phfeatheruse = 1;
        yield return new WaitForSeconds(t);
        parentSet.GetComponent<PlayerMovement>().phfeather -= 1;
    }
    private IEnumerator burn(int sec, int secdamage)
    {
        for (int i = 0; i < sec; i++)
        {
            Hurt(secdamage);
            yield return new WaitForSeconds(0.5f);
        }
        burnstack = 0;
    }
    void healthBarSet()
    {
        // baseTop = proportion.anchoredPosition.y;
        // newTop = baseTop / 1.3f;
        // proportion.anchoredPosition3D = new Vector3(0, newTop, 0);
        //大小設定
        if (this.transform.parent.gameObject.layer != 14)
        {
            proportion = this.gameObject.GetComponent<RectTransform>();
            baseScale = 0.01f;
            newScale = baseScale / gameObject.transform.parent.localScale.x;
            if (newScale != baseScale)
            {
                proportion.localScale = new Vector3(newScale, newScale, newScale);
            }
        }
        if (this.transform.parent.tag == "blue")
        {
            this.transform.GetChild(0).Find("Fill").GetComponent<Image>().color = new Color32(44, 43, 178, 255);
        }
    }
}

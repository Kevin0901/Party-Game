using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IllustratedBook : MonoBehaviour  //圖鑑
{
    // Start is called before the first frame update
    public bool inIllustratedBookMenu;
    private Animator IllustratedBookAnimator;
    private CanvasGroup CanvasGroup;
    void Start()
    {
        inIllustratedBookMenu = false;
        IllustratedBookAnimator = this.GetComponent<Animator>();
        CanvasGroup = this.GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        if (inIllustratedBookMenu)
        {
            fadein();
        }
    }
    public void biology() //怪物
    {
        disablebuttons();
        transform.Find("biologyPage").gameObject.SetActive(true);
    }
    public void item() //物品
    {
        disablebuttons();
        transform.Find("itemPage").gameObject.SetActive(true);
    }
    public void god() //神
    {
        disablebuttons();
        transform.Find("godPage").gameObject.SetActive(true);
    }

    private void disablebuttons()//關閉三個按鈕，顯示圖鑑
    {
        transform.Find("biology").gameObject.SetActive(false);
        transform.Find("item").gameObject.SetActive(false);
        transform.Find("god").gameObject.SetActive(false);
    }

    public void back() //點擊事件
    {
        StartCoroutine(fadeout());
    }
    private void fadein()
    {
        CanvasGroup.blocksRaycasts = true;
        CanvasGroup.alpha = 1;
        inIllustratedBookMenu = false;
    }
    private IEnumerator fadeout()
    {
        bool button1 = transform.Find("biology").gameObject.activeSelf;
        bool button2 = transform.Find("item").gameObject.activeSelf;
        bool button3 = transform.Find("god").gameObject.activeSelf;

        if (button1 && button2 && button3)
        {
            CanvasGroup.blocksRaycasts = false;
            GameObject.Find("TranPageAnimation").GetComponent<Animator>().SetTrigger("change");
            yield return new WaitForSeconds(0.5f);
            GameObject.Find("GameMenu").GetComponent<GameMenu>().inGameMenu = true;
            CanvasGroup.alpha = 0;
        }
        else
        {
            //開啟三個按鈕
            transform.Find("biology").gameObject.SetActive(true);
            transform.Find("item").gameObject.SetActive(true);
            transform.Find("god").gameObject.SetActive(true);
            //關閉頁面
            transform.Find("biologyPage").gameObject.SetActive(false);
            transform.Find("itemPage").gameObject.SetActive(false);
            transform.Find("godPage").gameObject.SetActive(false);
            // transform.Find("slide").gameObject.SetActive(false);
            // transform.Find("slide2").gameObject.SetActive(false);
        }
    }
}

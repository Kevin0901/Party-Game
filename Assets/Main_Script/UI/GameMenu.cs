using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenu : MonoBehaviour
{
    public bool inGameMenu;
    private Animator GameAnimator;
    private CanvasGroup CanvasGroup;
    void Start()
    {
        inGameMenu = false;
        GameAnimator = this.GetComponent<Animator>();
        CanvasGroup = this.GetComponent<CanvasGroup>();
    }
    void Update()
    {
        if (inGameMenu)
        {
            StartCoroutine(fadein());
            inGameMenu = false;
        }
    }
    private IEnumerator fadein() //淡入畫面
    {
        GameAnimator.SetBool("fadein", true);
        yield return null;
        GameAnimator.SetBool("fadein", false);
        GameObject.Find("LoadingCircle").transform.Find("Image").gameObject.SetActive(false);
        yield return new WaitForSeconds(1f);
        GameObject.Find("TranPageAnimation").transform.Find("Image").gameObject.SetActive(false);//關閉換頁動畫
        CanvasGroup.blocksRaycasts = true;
    }
    private IEnumerator fadeout(string canvas) //淡出畫面
    {
        CanvasGroup.blocksRaycasts = false;
        GameAnimator.SetBool("fadeout", true);
        yield return null;
        GameObject.Find("LoadingCircle").transform.Find("Image").gameObject.SetActive(true);
        GameObject.Find("TranPageAnimation").transform.Find("Image").gameObject.SetActive(true);
        GameAnimator.SetBool("fadeout", false);

        if (canvas.Equals("ChoosePlayer"))
        {
            GameObject.Find(canvas).GetComponent<ChoosePlayer>().inChoosePlayer = true;
        }
        else if (canvas.Equals("Noviceteaching"))
        {
            GameObject.Find(canvas).GetComponent<Noviceteaching>().inTeachingMenu = true;
        }
        else if (canvas.Equals("IllustratedBook"))
        {
            GameObject.Find(canvas).GetComponent<IllustratedBook>().inIllustratedBookMenu = true;
        }
    }
    public void JoinGame()//點擊事件
    {
        StartCoroutine(fadeout("ChoosePlayer"));
    }
    public void Noviceteaching()//點擊事件
    {
        StartCoroutine(fadeout("Noviceteaching"));
    }
    public void IllustratedBook()//點擊事件
    {
        StartCoroutine(fadeout("IllustratedBook"));
    }
    public void back()//點擊事件
    {
        GameObject.Find("StartMenu").GetComponent<MainMenu>().inMainMenu = true;
    }
    public void Setting()//點擊事件
    {
        GameObject.Find("SettingMenu").transform.Find("Settings").gameObject.SetActive(true);
    }
}

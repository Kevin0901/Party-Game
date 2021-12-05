using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public bool inMainMenu;
    private Animator MainAnimator;
    private CanvasGroup CanvasGroup;
    void Start()
    {
        inMainMenu = false;
        MainAnimator = this.GetComponent<Animator>();
        CanvasGroup = this.GetComponent<CanvasGroup>();
        GameObject.Find("LoadingCircle").transform.Find("Image").gameObject.SetActive(true);
        StartCoroutine(fadein());
    }
    void Update()
    {
        if (inMainMenu)
        {
            StartCoroutine(fadein());
            inMainMenu = false;
        }
        if (Input.GetKeyDown(KeyCode.Space) && CanvasGroup.blocksRaycasts)
        {
            StartCoroutine(fadeout());
        }
    }
    private IEnumerator fadein() //淡入畫面
    {
        MainAnimator.SetBool("fadein", true);
        yield return null;
        MainAnimator.SetBool("fadein", false);
        GameObject.Find("LoadingCircle").transform.Find("Image").gameObject.SetActive(false);
        yield return new WaitForSeconds(1f);
        GameObject.Find("TranPageAnimation").transform.Find("Image").gameObject.SetActive(false);//關閉換頁動畫
        CanvasGroup.blocksRaycasts = true;
    }
    private IEnumerator fadeout() //淡出畫面       
    {
        CanvasGroup.blocksRaycasts = false;
        MainAnimator.SetBool("fadeout", true);
        GameObject.Find("LoadingCircle").transform.Find("Image").gameObject.SetActive(true);
        GameObject.Find("TranPageAnimation").transform.Find("Image").gameObject.SetActive(true); //開啟換頁動畫
        yield return null;
        MainAnimator.SetBool("fadeout", false);
        GameObject.Find("GameMenu").GetComponent<GameMenu>().inGameMenu = true;
    }
    public void PlayGame() //點擊事件
    {
        if (CanvasGroup.blocksRaycasts)
        {
            StartCoroutine(fadeout());
        }
    }
    public void QuitGame() //點擊事件
    {
        Application.Quit();
    }
}

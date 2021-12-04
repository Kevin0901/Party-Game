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
            StartCoroutine(GoGameMenu());
        }
    }
    private IEnumerator fadein() //淡入畫面
    {
        MainAnimator.SetBool("fadein", true);
        yield return null;
        MainAnimator.SetBool("fadein", false);
        GameObject.Find("LoadingCircle").transform.Find("Image").gameObject.SetActive(false);
        GameObject.Find("TranPageAnimation").transform.Find("Image").gameObject.SetActive(false);//關閉換頁動畫
        CanvasGroup.blocksRaycasts = true;
    }
    private IEnumerator GoGameMenu() //前往主選單       
    {
        CanvasGroup.blocksRaycasts = false;
        MainAnimator.SetBool("exit", true);
        GameObject.Find("LoadingCircle").transform.Find("Image").gameObject.SetActive(true);
        GameObject.Find("TranPageAnimation").transform.Find("Image").gameObject.SetActive(true); //開啟換頁動畫
        yield return null;
        MainAnimator.SetBool("exit", false);
        GameObject.Find("GameMenu").GetComponent<GameMenu>().inGameMenu = true; 
    }
    public void PlayGame() //點擊事件
    {
        StartCoroutine(GoGameMenu());
    }
    public void QuitGame() //點擊事件
    {
        Application.Quit();
    }
}

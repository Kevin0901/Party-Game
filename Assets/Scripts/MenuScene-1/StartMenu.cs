using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class StartMenu : MonoBehaviour
{
    public bool inMainMenu;
    private Animator MainAnimator;
    private CanvasGroup CanvasGroup;
    void Start()
    {
        inMainMenu = false;
        MainAnimator = this.GetComponent<Animator>();
        CanvasGroup = this.GetComponent<CanvasGroup>();
        StartCoroutine(fadein());
    }
    void Update()
    {
        if (inMainMenu) //判斷是否切換畫面
        {
            StartCoroutine(fadein());
        }
        if (Input.GetKeyDown(KeyCode.Space) && CanvasGroup.blocksRaycasts)
        {
            StartCoroutine(Connected());
        }
    }
    private IEnumerator fadein() //淡入畫面
    {

        MainAnimator.SetTrigger("fade");
        inMainMenu = false;
        yield return new WaitForSeconds(0.5f);
        CanvasGroup.blocksRaycasts = true;
    }
    private IEnumerator fadeout() //淡出畫面       
    {
        CanvasGroup.blocksRaycasts = false;
        MainAnimator.SetTrigger("fade");
        GameObject.Find("TranPageAnimation").GetComponent<Animator>().SetTrigger("change");
        yield return new WaitForSeconds(0.5f);
        if (PlayerPrefs.HasKey("username") && PlayerPrefs.HasKey("password"))
        {
            GameObject.Find("GameMenu").GetComponent<GameMenu>().inGameMenu = true;
        }
        else
        {
            GameObject.Find("LoginMenu").GetComponent<Login>().inLoginMenu = true;
        }

    }

    IEnumerator Connected()
    {
        if (PhotonNetwork.IsConnected)
        {
            GameObject.Find("LoadingMenu").GetComponent<CanvasGroup>().alpha = 0;
            StartCoroutine(fadeout());
        }
        else
        {
            if (GameObject.Find("LoadingMenu").GetComponent<CanvasGroup>().alpha == 0)
            {
                GameObject.Find("LoadingMenu").GetComponent<CanvasGroup>().alpha = 1;
            }
            yield return new WaitForSeconds(0.01f);
            //PhotonNetwork.ConnectUsingSettings();  //開啟連線
            StartCoroutine(Connected());
        }
    }
    public void PlayGame() //如果點擊畫面
    {
        if (CanvasGroup.blocksRaycasts)
        {
            StartCoroutine(Connected());
        }
    }
    public void QuitGame() //如果按離開按鈕
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}

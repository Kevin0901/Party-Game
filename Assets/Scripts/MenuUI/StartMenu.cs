using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Firebase;
using Firebase.Database;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class StartMenu : MonoBehaviour
{
    private Animator MainAnimator;
    private CanvasGroup CanvasGroup;
    float waitTime = 0;
    DatabaseReference reference;
    void Start()
    {
        MainAnimator = this.GetComponent<Animator>();
        CanvasGroup = this.GetComponent<CanvasGroup>();
        StartCoroutine(fadein());
        reference = FirebaseDatabase.DefaultInstance.RootReference;  //定義資料庫連接
        if (PlayerPrefs.HasKey("username") && PlayerPrefs.HasKey("password"))
        {
            transform.Find("LogoutButton").gameObject.SetActive(true);
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && CanvasGroup.blocksRaycasts)
        {
            StartCoroutine(fadeout());
        }
    }
    private IEnumerator fadein() //淡入畫面
    {
        MainAnimator.SetTrigger("fade");
        yield return new WaitForSeconds(0.5f);
        CanvasGroup.blocksRaycasts = true;
    }
    private IEnumerator fadeout() //淡出畫面       
    {
        CanvasGroup.blocksRaycasts = false;
        MainAnimator.SetTrigger("fade");
        if (PlayerPrefs.HasKey("username") && PlayerPrefs.HasKey("password"))
        {
            GameObject.Find("LoadingCircle").transform.GetChild(0).gameObject.SetActive(true);
            GameObject.Find("music").SetActive(false);
            yield return new WaitForSeconds(2.5f);
            GameObject.Find("LoadingCircle").transform.GetChild(0).gameObject.SetActive(false);
            yield return new WaitForSeconds(0.3f);
            SceneManager.LoadScene(1);
        }
        else
        {
            GameObject.Find("TranPageAnimation").GetComponent<Animator>().SetTrigger("change");
            yield return new WaitForSeconds(0.5f);
            GameObject.Find("LoginMenu").GetComponent<Login>().inLoginMenu = true;
        }

    }
    // IEnumerator Connected()
    // {
    //     GameObject LM = GameObject.Find("LoadingMenu");
    //     if (PhotonNetwork.IsConnected)
    //     {
    //         LM.GetComponent<CanvasGroup>().alpha = 0;
    //         LM.GetComponent<CanvasGroup>().blocksRaycasts = false;
    //         StartCoroutine(fadeout());
    //     }
    //     else
    //     {
    //         if (LM.GetComponent<CanvasGroup>().alpha == 0)
    //         {
    //             LM.GetComponent<CanvasGroup>().alpha = 1;
    //         }
    //         yield return new WaitForSeconds(0.1f);
    //         waitTime += 0.1f;
    //         if (waitTime > 10f && !LM.transform.Find("Reload").gameObject.activeSelf)
    //         {
    //             LM.transform.Find("Reload").gameObject.SetActive(true);
    //             LM.GetComponent<CanvasGroup>().blocksRaycasts = true;
    //         }
    //         //PhotonNetwork.ConnectUsingSettings();  //開啟連線
    //         StartCoroutine(Connected());
    //     }
    // }
    public void PlayGame() //如果點擊畫面
    {
        if (CanvasGroup.blocksRaycasts)
        {
            StartCoroutine(fadeout());
        }
    }

    public void LogOut()
    {
        PlayerPrefs.DeleteKey("password");
        StartCoroutine(fadeout());
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

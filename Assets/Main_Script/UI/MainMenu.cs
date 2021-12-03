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
        MainAnimator.SetBool("fadein", true);
        StartCoroutine(fadein());
    }
    void Update()
    {
        if (inMainMenu)
        {
            MainAnimator.SetBool("fadein", true);
            StartCoroutine(fadein());
            inMainMenu = false;
        }
        if (Input.GetKeyDown(KeyCode.Space) && CanvasGroup.blocksRaycasts)
        {
            StartCoroutine(waitonesec());
        }
    }
    public void PlayGame()
    {
        StartCoroutine(waitonesec());
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    private IEnumerator fadein()
    {
        yield return new WaitForSeconds(0.5f);
        MainAnimator.SetBool("fadein", false);
        GameObject.Find("LoadingCircle").transform.Find("Image").gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        GameObject.Find("TranPageAnimation").transform.Find("Image").gameObject.SetActive(false);
        CanvasGroup.blocksRaycasts = true;
    }

    private IEnumerator waitonesec()
    {
        CanvasGroup.blocksRaycasts = false;
        MainAnimator.SetBool("exit", true);
        yield return new WaitForSeconds(0.2f);
        GameObject.Find("LoadingCircle").transform.Find("Image").gameObject.SetActive(true);
        GameObject.Find("TranPageAnimation").transform.Find("Image").gameObject.SetActive(true);

        yield return new WaitForSeconds(0.5f);
        MainAnimator.SetBool("exit", false);

        GameObject.Find("GameMenu").GetComponent<GameMenu>().inGameMenu = true;

    }
}

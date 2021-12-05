using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noviceteaching : MonoBehaviour //新手教學
{
    // Start is called before the first frame update
    public bool inTeachingMenu;
    private Animator TeachingAnimator;
    private CanvasGroup CanvasGroup;
    void Start()
    {
        inTeachingMenu = false;
        TeachingAnimator = this.GetComponent<Animator>();
        CanvasGroup = this.GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        if (inTeachingMenu)
        {
            StartCoroutine(fadein());
            inTeachingMenu = false;
        }
    }
    private IEnumerator fadein()//淡入畫面
    {
        TeachingAnimator.SetBool("fadein", true);
        yield return null;
        TeachingAnimator.SetBool("fadein", false);
        GameObject.Find("LoadingCircle").transform.Find("Image").gameObject.SetActive(false);
        yield return new WaitForSeconds(1f);
        GameObject.Find("TranPageAnimation").transform.Find("Image").gameObject.SetActive(false);
        CanvasGroup.blocksRaycasts = true;
    }

    private IEnumerator fadeout()//淡出畫面  
    {
        CanvasGroup.blocksRaycasts = false;
        TeachingAnimator.SetBool("fadeout", true);
        GameObject.Find("LoadingCircle").transform.Find("Image").gameObject.SetActive(true);
        GameObject.Find("TranPageAnimation").transform.Find("Image").gameObject.SetActive(true);
        yield return null;
        TeachingAnimator.SetBool("fadeout", false);
        GameObject.Find("GameMenu").GetComponent<GameMenu>().inGameMenu = true;
    }
    public void back() //點擊事件
    {
        StartCoroutine(fadeout());
    }
}

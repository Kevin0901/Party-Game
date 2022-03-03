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
            fadein();
        }
    }
    public void fadein()//淡入畫面
    {
        CanvasGroup.blocksRaycasts = true;
        CanvasGroup.alpha = 1;
        inTeachingMenu = false;
    }

    private IEnumerator fadeout()//淡出畫面  
    {
        CanvasGroup.blocksRaycasts = false;
        GameObject.Find("TranPageAnimation").GetComponent<Animator>().SetTrigger("change");
        yield return new WaitForSeconds(0.5f);
        GameObject.Find("GameMenu").GetComponent<GameMenu>().inGameMenu = true;
        CanvasGroup.alpha = 0;
    }
    public void back() //點擊事件
    {
        StartCoroutine(fadeout());
    }
}

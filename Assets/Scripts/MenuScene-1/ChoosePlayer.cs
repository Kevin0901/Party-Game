using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChoosePlayer : MonoBehaviour
{
    // Start is called before the first frame update
    public bool inChoosePlayer;
    private Animator ChoosePlayerAnimator;
    [HideInInspector] public CanvasGroup CanvasGroup;
    void Start()
    {
        inChoosePlayer = false;
        ChoosePlayerAnimator = this.GetComponent<Animator>();
        CanvasGroup = this.GetComponent<CanvasGroup>();
    }
    // Update is called once per frame
    void Update()
    {
        if (inChoosePlayer) //判斷是否切換畫面
        {
            fadein();
        }
    }
    private void fadein() //淡入畫面
    {
        CanvasGroup.blocksRaycasts = true;
        CanvasGroup.alpha = 1;
        inChoosePlayer = false;
    }

    private IEnumerator fadeout() //淡出畫面
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
    public void Setting()//點擊事件
    {
        GameObject.Find("SettingMenu").transform.Find("Settings").gameObject.SetActive(true);
    }
    private IEnumerator Warning() //配置不符警告
    {
        transform.Find("PlayerWaring").gameObject.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        transform.Find("PlayerWaring").gameObject.SetActive(false);
    }
}
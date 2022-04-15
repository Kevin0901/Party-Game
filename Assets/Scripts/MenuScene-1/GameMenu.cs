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
        if (inGameMenu) //判斷是否切換畫面
        {
            fadein();
        }
    }
    private void fadein() //淡入畫面
    {
        CanvasGroup.blocksRaycasts = true;
        CanvasGroup.alpha = 1;
        inGameMenu = false;
    }
    private IEnumerator fadeout() //淡出畫面
    {
        CanvasGroup.blocksRaycasts = false;
        GameObject.Find("TranPageAnimation").GetComponent<Animator>().SetTrigger("change");
        yield return new WaitForSeconds(0.5f);
        CanvasGroup.alpha = 0;
    }
    public void Teach()//點擊事件
    {
        StartCoroutine(fadeout());
        GameObject.Find("Noviceteaching").GetComponent<Noviceteaching>().inTeachingMenu = true;
    }
    public void Picture()//點擊事件
    {
        StartCoroutine(fadeout());
        GameObject.Find("IllustratedBook").GetComponent<IllustratedBook>().inIllustratedBookMenu = true;
    }
    public void PlayGame()//點擊事件
    {
        StartCoroutine(fadeout());
        GameObject.Find("RoomMenu").GetComponent<RoomMenu>().inRoomMenu = true;
        // GameObject.Find("ChoosePlayer").GetComponent<ChoosePlayer>().inChoosePlayer = true;
    }
    public void back()//點擊事件
    {
        CanvasGroup.blocksRaycasts = false;
        GameObject.Find("StartMenu").GetComponent<StartMenu>().inMainMenu = true;
    }
    public void Setting()//點擊事件
    {
        GameObject.Find("SettingMenu").transform.Find("Settings").gameObject.SetActive(true);
    }
}

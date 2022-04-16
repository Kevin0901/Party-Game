using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class RoomMenu : MonoBehaviour
{
    public bool inRoomMenu;
    private CanvasGroup CanvasGroup;
    [Header("創建房間名字輸入")]
    [SerializeField] TMP_InputField roomNameInputField;
    void Start()
    {
        inRoomMenu = false;
        CanvasGroup = this.GetComponent<CanvasGroup>();
    }

    void Update()
    {
        if (inRoomMenu) //判斷是否切換畫面
        {
            fadein();
        }
    }

    private void fadein() //淡入畫面
    {
        CanvasGroup.blocksRaycasts = true;
        CanvasGroup.alpha = 1;
        inRoomMenu = false;
    }
    private IEnumerator fadeout(string UIName) //淡出畫面
    {
        CanvasGroup.blocksRaycasts = false;
        GameObject.Find("TranPageAnimation").GetComponent<Animator>().SetTrigger("change");
        yield return new WaitForSeconds(0.5f);
        if(UIName.Equals("GameMenu"))
        {
            GameObject.Find("GameMenu").GetComponent<GameMenu>().inGameMenu = true;
        }
        CanvasGroup.alpha = 0;
    }
    public void back()//點擊事件
    {
        if (transform.Find("Create").gameObject.activeSelf || transform.Find("RoomList").gameObject.activeSelf)
        {
            transform.Find("Create").gameObject.SetActive(false);
            transform.Find("RoomList").gameObject.SetActive(false);
        }
        else
        {
            StartCoroutine(fadeout("GameMenu"));
        }
    }

    public void CreateRoomUI()
    {
        transform.Find("Create").gameObject.SetActive(true);
    }

    public void JoinRoomUI()
    {
        transform.Find("RoomList").gameObject.SetActive(true);
    }

    public void Create_Btn()
    {
        if (string.IsNullOrEmpty(roomNameInputField.text))  //如果沒輸入房間名稱則不給創建
        {
            return;
        }
        transform.Find("Create").gameObject.SetActive(false);
        transform.Find("RoomList").gameObject.SetActive(false);
        StartCoroutine(fadeout("ChoosePlayer"));
        GameObject.Find("ChoosePlayer").GetComponent<ChoosePlayer>().inChoosePlayer = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class EndGame : MonoBehaviour
{
    //遊戲結束會發生的事情
    private void OnEnable()
    {
        this.GetComponent<CanvasGroup>().alpha = 1;
        this.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
    //重新這個遊戲
    public void ReStart()
    {
        FightManager.Instance.gameNum();
    }
    //離開遊戲
    public void End()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
            Application.Quit();
#endif
    }
    //換遊戲
    public void changeGame()
    {
        GameObject.Find("GameChoose").GetComponent<CanvasGroup>().alpha = 1;
        GameObject.Find("GameChoose").GetComponent<CanvasGroup>().blocksRaycasts = true;
        this.GetComponent<CanvasGroup>().alpha = 0;
        this.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }
}

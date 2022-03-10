using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class EndGame : MonoBehaviour
{
    private void OnEnable()
    {
        this.GetComponent<CanvasGroup>().alpha = 1;
        this.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
    // Start is called before the first frame update
    public void ReStart()
    {
        FightManager.Instance.gameNum();
    }
    public void End()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
            Application.Quit();
#endif
    }
    public void changeGame()
    {
        GameObject.Find("GameChoose").GetComponent<CanvasGroup>().alpha = 1;
        GameObject.Find("GameChoose").GetComponent<CanvasGroup>().blocksRaycasts = true;
        this.GetComponent<CanvasGroup>().alpha = 0;
        this.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }
    public void changeGame(int num)
    {
        FightManager.Instance.gameNum(num);
    }
}

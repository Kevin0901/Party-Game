using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;
    private void Awake() //靜態實例宣告
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    [Header("主場景玩家")]
    [SerializeField] private GameObject player;
    private float h, w, proportion_W, proportion_H, factor;
    public IEnumerator waitSceneLoad(string level) //等場景載入完
    {
        if (SceneManager.GetActiveScene().name != level)
        {
            yield return null;
            StartCoroutine(waitSceneLoad(level));
        }
    }
    void ScreenSet() //螢幕比例
    {
        h = Screen.height;
        w = Screen.width;
        factor = screenScale(w, h);
        proportion_W = w / factor;
        proportion_H = h / factor;
    }
    float screenScale(float a, float b)
    {
        while (a != 0 && b != 0)
        {
            if (a > b)
            {
                a %= b;
            }
            else if (b > a)
            {
                b %= a;
            }
        }
        if (a == 0)
        {
            return b;
        }
        else
        {
            return a;
        }
    }
}

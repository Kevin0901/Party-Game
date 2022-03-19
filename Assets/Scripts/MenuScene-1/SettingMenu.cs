using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class SettingMenu : MonoBehaviour
{
    [SerializeField] private AudioSource audios;
    [SerializeField] private Slider soundSlider;
    private float volume;
    // Start is called before the first frame update
    void Start()
    {
        if (audios != null)
        {
            volume = audios.volume;
            soundSlider.value = volume;
            audios.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) //ese叫設定出來
        {
            if (transform.Find("Settings").gameObject.activeSelf)
            {
                transform.Find("Settings").gameObject.SetActive(false);
            }
            else
            {
                transform.Find("Settings").gameObject.SetActive(true);
            }
        }
        if (audios != null)
        {
            audios.volume = volume;
        }
    }

    public void updateVolume(float musicVolume) //音量調節
    {
        volume = musicVolume;
    }

    public void ExitGame() //離開遊戲
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
            Application.Quit();
#endif
    }
    public void Back() //關閉設定
    {
        transform.Find("Settings").gameObject.SetActive(false);
    }
}

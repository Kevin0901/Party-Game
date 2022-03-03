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
        volume = audios.volume;
        soundSlider.value = volume;
        audios.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
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
        audios.volume = volume;
    }

    public void updateVolume(float musicVolume)
    {
        volume = musicVolume;
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
            Application.Quit();
#endif
    }
    public void Back()
    {
        transform.Find("Settings").gameObject.SetActive(false);
    }
}

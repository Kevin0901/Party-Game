using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingMenu : MonoBehaviour
{
    public AudioSource audios;
    private float volume;
    // Start is called before the first frame update
    void Start()
    {
        volume = 0.3f;
        this.transform.GetChild(0).Find("Volume").GetComponent<Slider>().value = volume;
        audios.Play();
    }

    // Update is called once per frame
    void Update()
    {
        audios.volume = volume;
    }

    public void updateVolume(float musicvolume)
    {
        volume = musicvolume;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
    public void Back()
    {
        transform.Find("Settings").gameObject.SetActive(false);
    }
}

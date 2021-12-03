using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingMenu : MonoBehaviour
{
    public AudioSource audios;
    private float volume  = 1f;
    // Start is called before the first frame update
    void Start()
    {
        audios.Play();
    }

    // Update is called once per frame
    void Update()
    {
        audios.volume = volume;
    }

    public void updateVolume( float musicvolume)
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

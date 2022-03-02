using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MainSettingMenu : MonoBehaviour
{
    public AudioSource audios;
    private float volume;
    private bool showUI;
    private int cntChange;
    // Start is called before the first frame update
    void Start()
    {
        volume = 0.1f;
        this.transform.Find("Settings").Find("Volume").GetComponent<Slider>().value = volume;
        showUI = false;
        cntChange = 0;
        audios.Play();
    }

    // Update is called once per frame
    void Update()
    {
        audios.volume = volume;
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton7)) //開啟UI
        {
            if (showUI)
            {
                showUI = false;
            }
            else
            {
                showUI = true;
            }
            showSettingUI();
        }

        if (Input.GetKeyDown(KeyCode.JoystickButton6) && showUI) //切換新手教學頁面
        {
            float SetTeachingPagePos = 0;
            cntChange += 1;
            switch (cntChange % 3)
            {
                case 0:
                    SetTeachingPagePos += (-1920) * 3;
                    break;
                case 1:
                    SetTeachingPagePos += -1920;
                    break;
                case 2:
                    SetTeachingPagePos += -(1920 * 2);
                    break;
            }
            SetTeachingPag(SetTeachingPagePos);
        }
    }
    private void SetTeachingPag(float pos)
    {
        GameObject page = transform.Find("Panel").transform.Find("Scroll snap").transform.Find("Container").gameObject;
        RectTransform RTpage = page.GetComponent<RectTransform>();
        RTpage.offsetMin = new Vector2(pos, RTpage.offsetMin.y);
        RTpage.offsetMax = new Vector2(-(-5760 + (-pos)), RTpage.offsetMax.y);
    }
    private void showSettingUI() //開啟UI
    {
        if (showUI)
        {
            Cursor.visible = true;
            for (int i = 0; i < this.transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
            Time.timeScale = 0;
        }
        else
        {
            Cursor.visible = false;
            for (int i = 0; i < this.transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
            Time.timeScale = 1;
        }
    }
    public void updateVolume(float musicvolume) //點擊事件
    {
        volume = musicvolume;
    }

    public void ExitGameToUI()   //點擊事件   
    {
        SceneManager.LoadScene("UI");
    }
    public void Back() //點擊事件
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}

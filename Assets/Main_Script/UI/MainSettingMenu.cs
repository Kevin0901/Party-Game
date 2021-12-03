using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainSettingMenu : MonoBehaviour
{
    public AudioSource audios;
    private float volume = 0.3f;
    private bool showUI;
    private int Btn6Cnt;
    // Start is called before the first frame update
    void Start()
    {
        audios.Play();
        showUI = false;
        Btn6Cnt = 0;
    }

    // Update is called once per frame
    void Update()
    {
        audios.volume = volume;
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton7))
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

        if (Input.GetKeyDown(KeyCode.JoystickButton6) && showUI)
        {
            float SetTeachingPagePos = 0;
            Btn6Cnt += 1;
            switch (Btn6Cnt % 3)
            {
                case 1:
                    SetTeachingPagePos += -1920;
                    break;
                case 2:
                    SetTeachingPagePos += -(1920*2);
                    break;
                case 0:
                    SetTeachingPagePos += (-1920)*3;
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
        RTpage.offsetMax = new Vector2(-(-5760+(-pos)), RTpage.offsetMax.y);
    }
    private void showSettingUI()
    {
        if (showUI)
        {
            Cursor.visible = true;
            for (int i = 0; i < this.transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
        }
        else
        {
            Cursor.visible = false;
            for (int i = 0; i < this.transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }
    public void updateVolume(float musicvolume)
    {
        volume = musicvolume;
    }

    public void ExitGameToUI()
    {
        SceneManager.LoadScene("UI");
    }
    public void Back()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}

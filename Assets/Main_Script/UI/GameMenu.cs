using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenu : MonoBehaviour
{
    // Start is called before the first frame update
    public bool inGameMenu;
    private Animator GameAnimator;
    private CanvasGroup CanvasGroup;
    void Start()
    {
        inGameMenu = false;
        GameAnimator = this.GetComponent<Animator>();
        CanvasGroup = this.GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        if (inGameMenu)
        {
            GameAnimator.SetBool("fadein", true);
            StartCoroutine(fadein());
            inGameMenu = false;
        }
    }
    private IEnumerator fadein()
    {
        yield return new WaitForSeconds(0.5f);
        GameAnimator.SetBool("fadein", false);
        GameObject.Find("LoadingCircle").transform.Find("Image").gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        GameObject.Find("TranPageAnimation").transform.Find("Image").gameObject.SetActive(false);
        CanvasGroup.blocksRaycasts = true;
    }
    public void Setting()
    {
        GameObject.Find("SettingMenu").transform.Find("Settings").gameObject.SetActive(true);
    }
    public void JoinGame()
    {
        StartCoroutine(fadeout("ChoosePlayer"));
        // "ChoosePlayer"
    }
    public void Noviceteaching()
    {
        StartCoroutine(fadeout("Noviceteaching"));
    }

    public void IllustratedBook()
    {
        StartCoroutine(fadeout("IllustratedBook"));
    }

    public void back()
    {
        StartCoroutine(fadeout("StartMenu"));
    }

    private IEnumerator fadeout(string canvas)
    {
        CanvasGroup.blocksRaycasts = false;
        GameAnimator.SetBool("fadeout", true);
        yield return new WaitForSeconds(0.2f);
        GameObject.Find("LoadingCircle").transform.Find("Image").gameObject.SetActive(true);
        GameObject.Find("TranPageAnimation").transform.Find("Image").gameObject.SetActive(true);

        yield return new WaitForSeconds(0.5f);
        GameAnimator.SetBool("fadeout", false);

        if (canvas.Equals("ChoosePlayer"))
        {
            GameObject.Find(canvas).GetComponent<ChoosePlayer>().inChoosePlayer = true;
        }
        else if (canvas.Equals("Noviceteaching"))
        {
            GameObject.Find(canvas).GetComponent<Noviceteaching>().inTeachingMenu = true;
        }
        else if(canvas.Equals("StartMenu"))
        {
            GameObject.Find(canvas).GetComponent<MainMenu>().inMainMenu = true;
        }
        else if(canvas.Equals("IllustratedBook"))
        {
            GameObject.Find(canvas).GetComponent<IllustratedBook>().inIllustratedBookMenu = true;
        }
    }
}

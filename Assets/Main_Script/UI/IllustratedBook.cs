using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IllustratedBook : MonoBehaviour
{
    // Start is called before the first frame update
    public bool inIllustratedBookMenu;
    private Animator IllustratedBookAnimator;
    private CanvasGroup CanvasGroup;
    void Start()
    {
        inIllustratedBookMenu = false;
        IllustratedBookAnimator = this.GetComponent<Animator>();
        CanvasGroup = this.GetComponent<CanvasGroup>();

    }

    // Update is called once per frame
    void Update()
    {
        if (inIllustratedBookMenu)
        {
            IllustratedBookAnimator.SetBool("fadein", true);
            StartCoroutine(fadein());
            inIllustratedBookMenu = false;
        }
    }
    public void biology()
    {
        disablebuttons();
        transform.Find("biologyPage").gameObject.SetActive(true);
    }
    public void item()
    {
        disablebuttons();
        transform.Find("itemPage").gameObject.SetActive(true);
    }
    public void god()
    {
        disablebuttons();
        transform.Find("godPage").gameObject.SetActive(true);
    }

    private void disablebuttons()
    {
        transform.Find("biology").gameObject.SetActive(false);
        transform.Find("item").gameObject.SetActive(false);
        transform.Find("god").gameObject.SetActive(false);
    }

    public void back()
    {
        StartCoroutine(fadeout("GameMenu"));
    }
    private IEnumerator fadein()
    {
        yield return new WaitForSeconds(0.5f);
        IllustratedBookAnimator.SetBool("fadein", false);
        GameObject.Find("LoadingCircle").transform.Find("Image").gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        GameObject.Find("TranPageAnimation").transform.Find("Image").gameObject.SetActive(false);
        CanvasGroup.blocksRaycasts = true;
    }
    private IEnumerator fadeout(string canvas)
    {
        if (canvas.Equals("GameMenu"))
        {
            bool button1 = transform.Find("biology").gameObject.activeSelf;
            bool button2 = transform.Find("item").gameObject.activeSelf;
            bool button3 = transform.Find("god").gameObject.activeSelf;

            if (button1 && button2 && button3)
            {
                CanvasGroup.blocksRaycasts = false;
                IllustratedBookAnimator.SetBool("fadeout", true);
                yield return new WaitForSeconds(0.2f);
                GameObject.Find("LoadingCircle").transform.Find("Image").gameObject.SetActive(true);
                GameObject.Find("TranPageAnimation").transform.Find("Image").gameObject.SetActive(true);

                yield return new WaitForSeconds(0.5f);
                IllustratedBookAnimator.SetBool("fadeout", false);

                GameObject.Find(canvas).GetComponent<GameMenu>().inGameMenu = true;
            }
            else
            {
                transform.Find("biology").gameObject.SetActive(true);
                transform.Find("item").gameObject.SetActive(true);
                transform.Find("god").gameObject.SetActive(true);

                transform.Find("biologyPage").gameObject.SetActive(false);
                transform.Find("itemPage").gameObject.SetActive(false);
                transform.Find("godPage").gameObject.SetActive(false);

                transform.Find("slide").gameObject.SetActive(false);
                transform.Find("slide2").gameObject.SetActive(false);
            }
        }
    }
}

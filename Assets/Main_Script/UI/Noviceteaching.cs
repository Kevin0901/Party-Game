using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Noviceteaching : MonoBehaviour
{
    // Start is called before the first frame update
    public bool inTeachingMenu;
    private Animator TeachingAnimator;
    private CanvasGroup CanvasGroup;
    void Start()
    {
        inTeachingMenu = false;
        TeachingAnimator = this.GetComponent<Animator>();
        CanvasGroup = this.GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        if(inTeachingMenu)
        {
            TeachingAnimator.SetBool("fadein",true);
            StartCoroutine(fadein());
            inTeachingMenu = false;
            // this.transform.Find("Image").gameObject.SetActive(true);
        }
    }
    private IEnumerator fadein()
    {
        yield return new WaitForSeconds(0.5f);
        TeachingAnimator.SetBool("fadein",false);
        GameObject.Find("LoadingCircle").transform.Find("Image").gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        GameObject.Find("TranPageAnimation").transform.Find("Image").gameObject.SetActive(false);
        CanvasGroup.blocksRaycasts = true;
    }

    private IEnumerator fadeout()
    {
        CanvasGroup.blocksRaycasts = false;
        TeachingAnimator.SetBool("fadeout", true);
        yield return new WaitForSeconds(0.2f);
        GameObject.Find("LoadingCircle").transform.Find("Image").gameObject.SetActive(true);
        GameObject.Find("TranPageAnimation").transform.Find("Image").gameObject.SetActive(true);

        yield return new WaitForSeconds(0.5f);
        TeachingAnimator.SetBool("fadeout", false);

        GameObject.Find("GameMenu").GetComponent<GameMenu>().inGameMenu = true;
    }

    public void back()
    {
        StartCoroutine(fadeout());
    }
}

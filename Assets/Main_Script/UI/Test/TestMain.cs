using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestMain : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(fadeout("ChoosePlayer"));
    }

    private IEnumerator fadeout(string canvas)
    {
        yield return new WaitForSeconds(0.2f);
        GameObject.Find("LoadingCircle").transform.Find("Image").gameObject.SetActive(true);
        GameObject.Find("TranPageAnimation").transform.Find("Image").gameObject.SetActive(true);

        yield return new WaitForSeconds(1f);
        GameObject.Find(canvas).GetComponent<ChoosePlayer>().inChoosePlayer = true;
    }
}

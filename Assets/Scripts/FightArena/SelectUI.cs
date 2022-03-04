using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectUI : MonoBehaviour
{
    // Start is called before the first frame update
    public void medusa()
    {
        this.transform.parent.transform.Find("Medusa").gameObject.SetActive(true);
        this.gameObject.SetActive(false);
    }
    public void sunmoon()
    {
        this.transform.parent.transform.Find("SunMoon").gameObject.SetActive(true);
        this.gameObject.SetActive(false);
    }
    public void sword()
    {
        this.transform.parent.transform.Find("Sword").gameObject.SetActive(true);
        this.gameObject.SetActive(false);
    }
    public void cupid()
    {
        this.transform.parent.transform.Find("Cupid").gameObject.SetActive(true);
        this.gameObject.SetActive(false);
    }
}

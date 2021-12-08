using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class Timer : MonoBehaviour
{
    private TimeManager t;
    public Text currenttimeText;
    // Start is called before the first frame update
    void Start()
    {
        t = GameObject.Find("SceneManager").GetComponent<TimeManager>();
    }

    // Update is called once per frame
    void Update()
    {
        TimeSpan time= TimeSpan.FromSeconds(t.currentTime);
        currenttimeText.text = time.ToString(@"mm\:ss");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class Timer : MonoBehaviour
{
    private GameControl t;
    public Text currenttimeText;
    [SerializeField] float Time;
    // Start is called before the first frame update
    void Start()
    {
        Time = GameObject.Find("RoomManager").GetComponent<RoomManager>().GameTime;
        // t = GameObject.Find("SceneManager").GetComponent<GameControl>();
    }

    // Update is called once per frame
    void Update()
    {
        TimeSpan time = TimeSpan.FromSeconds(Time);
        currenttimeText.text = time.ToString(@"mm\:ss");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class Timer : MonoBehaviour
{
    private GameControl t;
    public TextMeshProUGUI currenttimeText;
    [SerializeField] float Time;
    // Start is called before the first frame update
    void Start()
    {
        // t = GameObject.Find("SceneManager").GetComponent<GameControl>();
    }

    // Update is called once per frame
    void Update()
    {
        Time = GameObject.Find("RoomManager").GetComponent<RoomManager>().TotalTime;
        TimeSpan time = TimeSpan.FromSeconds(Time);
        currenttimeText.text = time.ToString(@"mm\:ss");
    }
}

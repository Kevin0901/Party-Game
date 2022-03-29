using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class strike : MonoBehaviour
{
    [SerializeField] private int max;
    [SerializeField] private float all_dis;
    [SerializeField] private int cnt;
    private float move_dis;
    public HammerEvent _event;
    private void Start()
    {
        move_dis = all_dis / max;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !_event.isEnd)
        {
            cnt++;
            this.transform.GetChild(0).localPosition += new Vector3(0, -move_dis, 0);
        }
        if (cnt >= max)
        {
            StartCoroutine(_event.EndGame(this.transform.parent.gameObject));
        }
    }
}

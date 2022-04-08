using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class strike : MonoBehaviour
{
    [SerializeField] private int max;
    [SerializeField] private float all_dis;
    [SerializeField] private int cnt;
    private float move_dis;
    [HideInInspector] public HammerEvent _event;
    private void Start()
    {
        move_dis = all_dis / max;
    }
    void Update()
    {
        if (!_event.isEnd && Input.GetKeyDown(KeyCode.Space))
        {
            cnt++;
            this.transform.GetChild(0).localPosition += new Vector3(0, -move_dis, 0);
            if (cnt >= max)
            {
                this.GetComponent<SpriteRenderer>().color = new Color(1, 0.3f, 0.47f);
                StartCoroutine(_event.EndGame(this.transform.parent.parent.gameObject));
            }
        }
    }
}

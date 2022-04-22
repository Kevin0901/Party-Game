using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class strike : MonoBehaviour
{
    [SerializeField] private int max;
    [SerializeField] private float all_dis;
    [SerializeField] private int cnt;
    public SpriteRenderer lightSword;
    private float move_dis;
    [HideInInspector] public HammerEvent _event;
    PhotonView PV;
    bool SetPos = false;
    private void Start()
    {
        PV = GetComponent<PhotonView>();
        move_dis = all_dis / max;
    }
    void Update()
    {
        if (_event != null && !SetPos)
        {
            this.transform.parent.position += new Vector3(0, -20, 0);
            SetPos = true;
        }
        if (_event != null && !_event.isEnd && Input.GetKeyDown(KeyCode.Space) && PV.IsMine)
        {
            cnt++;
            this.transform.localPosition += new Vector3(0, -move_dis, 0);
            if (cnt >= max)
            {
                lightSword.enabled = true;
                StartCoroutine(_event.EndGame(this.transform.parent.parent.gameObject));
            }
        }
    }
}

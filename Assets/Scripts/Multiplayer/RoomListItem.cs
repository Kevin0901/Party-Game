using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using TMPro;
public class RoomListItem : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    public RoomInfo info;
    bool isClick = false;
    public void SetUp(RoomInfo _info)
    {
        info = _info;
        text.text = _info.Name;
    }
    private void Update()
    {
        if(GameObject.Find("ChoosePlayer").GetComponent<CanvasGroup>().blocksRaycasts && isClick)
        {
            isClick = false;
        }
    }

    public void OnClick()
    {
        if (!isClick)
        {
            Launcher.Instance.JoinRoom(info);
            // GameObject.Find("RoomMenu").transform.Find("Create").gameObject.SetActive(false);
            //GameObject.Find("RoomMenu").transform.Find("RoomList").gameObject.SetActive(false);
            GameObject.Find("RoomMenu").GetComponent<RoomMenu>().StartCoroutine("fadeout", "ChoosePlayer");
            GameObject.Find("ChoosePlayer").GetComponent<ChoosePlayer>().inChoosePlayer = true;
            isClick = true;
        }

    }
}

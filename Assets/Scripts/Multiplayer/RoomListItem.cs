using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using TMPro;
public class RoomListItem : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_Text text;
    public RoomInfo info;

    public void SetUp(RoomInfo _info)
    {
        info = _info;
        text.text = _info.Name;
    }

    public void OnClick()
    {
        Launcher.Instance.JoinRoom(info);
        GameObject.Find("RoomMenu").transform.Find("Create").gameObject.SetActive(false);
        GameObject.Find("RoomMenu").transform.Find("RoomList").gameObject.SetActive(false);
        GameObject.Find("RoomMenu").GetComponent<RoomMenu>().StartCoroutine("fadeout","ChoosePlayer");
        GameObject.Find("ChoosePlayer").GetComponent<ChoosePlayer>().inChoosePlayer = true;
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)  //房間列表更新
    {
        if(text.text.Equals(roomList[0].Name))
        {
            Destroy(this.gameObject);
        }
    }
}

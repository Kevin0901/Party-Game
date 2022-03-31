using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using Firebase;
using Firebase.Database;
public class Login : MonoBehaviour
{
    [Header("玩家名字輸入")]
    [SerializeField] TMP_InputField Name;
    [Header("玩家密碼輸入")]
    [SerializeField] TMP_InputField Password;
    [Header("玩家密碼確認輸入")]
    [SerializeField] TMP_InputField ConfirmPassword;
    [Header("登入按鈕")]
    [SerializeField] GameObject LoginButton;
    [Header("註冊按鈕")]
    [SerializeField] GameObject StartGameButton;
    [Header("沒有註冊訊息")]
    [SerializeField] GameObject NoRegister;
    [Header("密碼錯誤訊息")]
    [SerializeField] GameObject WrongPass;
    [Header("註冊成功訊息")]
    [SerializeField] GameObject RegisterComplete;
    [Header("密碼不相配訊息")]
    [SerializeField] GameObject PassNotMatch;
    [Header("玩家名稱")]
    [SerializeField] TMP_Text UserName;
    
    DatabaseReference reference;
    void Start()
    {
        reference = FirebaseDatabase.DefaultInstance.RootReference;  //定義資料庫連接
        if (PlayerPrefs.HasKey("username"))  //如果 PlayerPrefs 裡面有玩家資料，直接預先填入
        {
            Name.text = PlayerPrefs.GetString("username");
            PhotonNetwork.NickName = PlayerPrefs.GetString("username");
        }
    }

    public void PlayerLogin() //登入
    {
        NoRegister.SetActive(false);
        WrongPass.SetActive(false);
        RegisterComplete.SetActive(false);
        PassNotMatch.SetActive(false);

        bool isRegister = false;
        bool isRightPass = false;
        StartCoroutine(GetAcc((DataSnapshot Acc) =>  //從資料庫抓取所有玩家帳號密碼
        {
            foreach (var rules in Acc.Children)  //逐筆檢視
            {
                if (Name.text.Equals(rules.Key.ToString()))  //如果帳號已在資料庫裡
                {
                    isRegister = true;  //表示已註冊
                    if (Password.text.Equals(rules.Value.ToString()))  //如果輸入的密碼與資料庫相同
                    {
                        isRightPass = true;  //表示密碼正確
                    }
                    break;
                }
            }

            if (isRegister && isRightPass)  //如果都正確
            {
                Launcher.Instance.isLogin = true;
                PhotonNetwork.NickName = Name.text;
                UserName.SetText(PhotonNetwork.NickName);

                PlayerPrefs.SetString("username", Name.text);
                MenuManger.Instance.OpenMenu("title");
            }
            else if (!isRegister)  //尚未註冊
            {
                NoRegister.SetActive(true);
            }
            else if (!isRightPass)  //密碼輸入錯誤
            {
                WrongPass.SetActive(true);
            }
        }));
    }
    public void PlayerRegister()  //註冊，寫進資料庫
    {
        NoRegister.SetActive(false);
        WrongPass.SetActive(false);
        RegisterComplete.SetActive(false);
        PassNotMatch.SetActive(false);
        if(!ConfirmPassword.gameObject.activeSelf)
        {
            ConfirmPassword.gameObject.SetActive(true);
            return;
        }
        if (!(Password.text.Equals(ConfirmPassword.text)) || Password.text.Equals(""))
        {
            PassNotMatch.SetActive(true);
            return;
        }
        reference.Child("Account").Child(Name.text).SetValueAsync(Password.text);
        RegisterComplete.SetActive(true);
    }

    IEnumerator GetAcc(System.Action<DataSnapshot> onCallbacks) //從資料庫讀取所有玩家 Account
    {
        var userData = reference.Child("Account").GetValueAsync();

        yield return new WaitUntil(predicate: () => userData.IsCompleted);

        if (userData != null)
        {
            DataSnapshot snapshot = userData.Result;
            onCallbacks.Invoke(snapshot);
        }
    }
}

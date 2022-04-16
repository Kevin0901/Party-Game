using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using Firebase;
using Firebase.Database;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class Login : MonoBehaviour
{
    [Header("動畫")]
    [SerializeField] GameObject Anime;
    [Header("登入方塊")]
    [SerializeField] GameObject LoginBlock;
    [Header("註冊方塊")]
    [SerializeField] GameObject RegisterBlock;
    [Header("[登入]玩家名字輸入")]
    [SerializeField] TMP_InputField LoginName;
    [Header("[登入]玩家密碼輸入")]
    [SerializeField] TMP_InputField LoginPassword;
    [Header("[註冊]玩家名字輸入")]
    [SerializeField] TMP_InputField RegisterName;
    [Header("[註冊]玩家密碼輸入")]
    [SerializeField] TMP_InputField RegisterPassword;
    [Header("[註冊]玩家密碼確認輸入")]
    [SerializeField] TMP_InputField ConfirmPassword;
    // [Header("登入按鈕")]
    // GameObject LoginButton;
    // [Header("註冊按鈕")]
    // GameObject StartGameButton;
    [Header("沒有註冊訊息")]
    [SerializeField] GameObject NoRegister;
    [Header("密碼錯誤訊息")]
    [SerializeField] GameObject WrongPass;
    [Header("註冊成功訊息")]
    [SerializeField] GameObject RegisterComplete;
    [Header("密碼不相配訊息")]
    [SerializeField] GameObject PassNotMatch;
    [Header("已經註冊")]
    [SerializeField] GameObject IsRegister;
    // [Header("玩家名稱")]
    // [SerializeField] TMP_Text UserName;

    public bool inLoginMenu;
    private CanvasGroup CanvasGroup;
    DatabaseReference reference;
    int loginCnt, registerCnt;
    void Start()
    {
        inLoginMenu = false;
        CanvasGroup = this.GetComponent<CanvasGroup>();
        reference = FirebaseDatabase.DefaultInstance.RootReference;  //定義資料庫連接
        if (PlayerPrefs.HasKey("username"))  //如果 PlayerPrefs 裡面有玩家資料，直接預先填入
        {
            LoginName.text = PlayerPrefs.GetString("username");
            PhotonNetwork.NickName = PlayerPrefs.GetString("username");
        }
    }
    void Update()
    {
        if (inLoginMenu) //判斷是否切換畫面
        {
            fadein();
        }
        if (CanvasGroup.blocksRaycasts && Input.GetKeyDown(KeyCode.Tab))
        {
            if (LoginBlock.activeSelf)
            {
                if (loginCnt % 2 == 0)
                {
                    LoginName.Select();
                }
                else
                {
                    LoginPassword.Select();
                }
                loginCnt++;
            }
            else if (RegisterBlock.activeSelf)
            {
                if (registerCnt % 3 == 0)
                {
                    RegisterName.Select();
                }
                else if (registerCnt % 3 == 1)
                {
                    RegisterPassword.Select();
                }
                else
                {
                    ConfirmPassword.Select();
                }
                registerCnt++;
            }
        }
    }
    private void fadein() //淡入畫面
    {
        CanvasGroup.blocksRaycasts = true;
        CanvasGroup.alpha = 1;
        Anime.SetActive(true);
        StartCoroutine(Anime_IE());
        loginCnt = 0;
        registerCnt = 0;
        inLoginMenu = false;
    }
    private IEnumerator fadeout() //淡出畫面
    {
        CanvasGroup.blocksRaycasts = false;
        GameObject.Find("TranPageAnimation").GetComponent<Animator>().SetTrigger("change");
        yield return new WaitForSeconds(0.5f);
        CanvasGroup.alpha = 0;
        LoginBlock.SetActive(false);
        RegisterBlock.SetActive(false);
        transform.Find("Quit").gameObject.SetActive(false);
    }
    IEnumerator Anime_IE()
    {
        yield return new WaitForSeconds(1.2f);
        Anime.SetActive(false);
        Open_Login();
    }
    public void Open_Login()
    {
        NoRegister.SetActive(false);
        WrongPass.SetActive(false);
        RegisterComplete.SetActive(false);
        PassNotMatch.SetActive(false);
        IsRegister.SetActive(false);

        LoginBlock.SetActive(true);
        RegisterBlock.SetActive(false);
        transform.Find("Quit").gameObject.SetActive(true);
    }

    public void Open_Register()
    {
        NoRegister.SetActive(false);
        WrongPass.SetActive(false);
        RegisterComplete.SetActive(false);
        PassNotMatch.SetActive(false);
        IsRegister.SetActive(false);

        LoginBlock.SetActive(false);
        RegisterBlock.SetActive(true);
        transform.Find("Quit").gameObject.SetActive(true);
    }
    public void Quit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    public void PlayerLogin() //登入
    {
        NoRegister.SetActive(false);
        WrongPass.SetActive(false);
        RegisterComplete.SetActive(false);
        PassNotMatch.SetActive(false);
        IsRegister.SetActive(false);

        bool isRegister = false;
        bool isRightPass = false;
        StartCoroutine(GetAcc((DataSnapshot Acc) =>  //從資料庫抓取所有玩家帳號密碼
        {
            foreach (var rules in Acc.Children)  //逐筆檢視
            {
                if (LoginName.text.Equals(rules.Key.ToString()))  //如果帳號已在資料庫裡
                {
                    isRegister = true;  //表示已註冊
                    if (LoginPassword.text.Equals(rules.Value.ToString()))  //如果輸入的密碼與資料庫相同
                    {
                        isRightPass = true;  //表示密碼正確
                    }
                    break;
                }
            }

            if (isRegister && isRightPass)  //如果都正確
            {
                Launcher.Instance.isLogin = true;
                PhotonNetwork.NickName = LoginName.text;
                //UserName.SetText(PhotonNetwork.NickName);

                PlayerPrefs.SetString("username", LoginName.text);
                PlayerPrefs.SetString("password", LoginPassword.text);
                StartCoroutine(fadeout());
                GameObject.Find("GameMenu").GetComponent<GameMenu>().inGameMenu = true;
                // MenuManger.Instance.OpenMenu("title");
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
        bool isRegister = false;
        NoRegister.SetActive(false);
        WrongPass.SetActive(false);
        RegisterComplete.SetActive(false);
        PassNotMatch.SetActive(false);
        IsRegister.SetActive(false);
        if (!(RegisterPassword.text.Equals(ConfirmPassword.text)) || RegisterPassword.text.Equals("") || RegisterName.text.Equals(""))
        {
            PassNotMatch.SetActive(true);
            return;
        }
        StartCoroutine(GetAcc((DataSnapshot Acc) =>  //從資料庫抓取所有玩家帳號密碼
        {
            foreach (var rules in Acc.Children)  //逐筆檢視
            {
                if (RegisterName.text.Equals(rules.Key.ToString()))  //如果帳號已在資料庫裡
                {
                    IsRegister.SetActive(true);
                    isRegister = true;
                }
            }
            if (!isRegister)
            {
                reference.Child("Account").Child(RegisterName.text).SetValueAsync(RegisterPassword.text);
                RegisterComplete.SetActive(true);
            }

        }));
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

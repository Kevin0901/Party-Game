using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class GameControl : MonoBehaviour
{
    public float currentTime;
    public int randomTime;
    public bool isChange;
    public TextMeshProUGUI countdownDisplay;
    private string game;
    private GameObject events;
    public GameObject redcastle, bluecastle;
    private bool winplay, isEnd;
    private float oneSec;
    private int test = 0, lastTime;
    // Start is called before the first frame update
    void Start()
    {
        currentTime = 0;
        isChange = false;
        isEnd = false;
        winplay = false;
        events = this.transform.Find("Event").gameObject;
    }
    // Update is called once per frame
    void Update()
    {
        // if (GameObject.Find("GameManager").GetComponent<PlayerManager>().blue != null && GameObject.Find("GameManager").GetComponent<PlayerManager>().red != null)
        // {
        //     redcastle = GameObject.Find("GameManager").GetComponent<PlayerManager>().red;
        //     bluecastle = GameObject.Find("GameManager").GetComponent<PlayerManager>().blue;
        // }
        currentTime += Time.deltaTime;
        oneSec += Time.deltaTime;
        if (oneSec >= 1f)
        {
            lastTime = randomTime - (int)oneSec;
            if (lastTime == 15)  //如果剩15秒的話
            {
                StartCoroutine(EventNotice());
            }
            else if (lastTime <= 5 && lastTime > 0)//剩五秒的話，倒數計時
            {
                transform.Find("CountDown").gameObject.SetActive(true);
                startArenaGame(lastTime);
            }
            else if (((int)currentTime) == randomTime && !isChange && !isEnd)//如果時間到了
            {
                StartCoroutine(setPAPA());
                isChange = true;
            }
            oneSec = 0;
        }

        if (redcastle != null && bluecastle != null)
        {
            Debug.Log("RED:" + redcastle.GetComponent<Castle>().CurHealth);
            Debug.Log("Blue:" + bluecastle.GetComponent<Castle>().CurHealth);
            if (redcastle.GetComponent<Castle>().CurHealth < 0)
            {
                if (winplay == false)
                {
                    isEnd = true;
                    events.SetActive(true);
                    events.transform.Find("Image").gameObject.SetActive(true);
                    events.transform.Find("Win").gameObject.SetActive(true);
                    StartCoroutine(bluewin());
                    Cursor.visible = true;
                }

            }
            else if (bluecastle.GetComponent<Castle>().CurHealth < 0)
            {
                if (winplay == false)
                {
                    isEnd = true;
                    events.SetActive(true);
                    events.transform.Find("Image").gameObject.SetActive(true);
                    events.transform.Find("Win").gameObject.SetActive(true);
                    StartCoroutine(redwin());
                    Cursor.visible = true;
                }
            }
        }

    }
    private IEnumerator EventNotice()//如果剩15秒的話
    {
        test += 1;
        if (test == 5)
        {
            test = 1;
        }
        if (events.activeSelf == false)
        {
            events.SetActive(true);
            events.transform.GetChild(test).gameObject.SetActive(true);
            game = events.transform.GetChild(test).gameObject.name;
            yield return new WaitForSeconds(10f);
            events.transform.GetChild(test).gameObject.SetActive(false);
        }
    }
    private void startArenaGame(int time)//剩五秒的話，倒數計時
    {
        countdownDisplay.text = time.ToString();
    }
    public void ExitGameToUI()
    {
        Destroy(GameObject.Find("GameManager"));
        SceneManager.LoadScene("UI");
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    private IEnumerator bluewin()
    {
        Animator animator = events.transform.Find("Win").GetComponent<Animator>();
        animator.SetBool("BlueWin", true);
        yield return null;
        animator.SetBool("BlueWin", false);
        winplay = true;
    }
    private IEnumerator redwin()
    {
        Animator animator = events.transform.Find("Win").GetComponent<Animator>();
        animator.SetBool("RedWin", true);
        yield return null;
        animator.SetBool("RedWin", false);
        winplay = true;
    }
    private IEnumerator setPAPA()//開始加入子物件
    {
        events.SetActive(false);
        GameObject.Find("MainBlackScreen").GetComponent<Animator>().SetBool("fadein", true);
        yield return null;
        GameObject.Find("MainBlackScreen").GetComponent<Animator>().SetBool("fadein", false);
        yield return new WaitForSeconds(1f);
        GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        for (int i = 0; i < allObjects.Length; i++)
        {
            if (allObjects[i].transform.parent == null) //加入子物件
            {
                allObjects[i].transform.SetParent(GameObject.Find("PAPA").transform);
            }
        }
        for (int i = 0; i < GameObject.Find("PAPA").transform.childCount; i++)
        {
            string name = GameObject.Find("PAPA").transform.GetChild(i).name;
            if (name != "MainGameManager2" && name != "GameManager" && name != "MainBlackScreen" && name != "GameSettingMenu" && name != "Audio Source")
            {
                GameObject.Find("PAPA").transform.GetChild(i).gameObject.SetActive(false);//把除了Manager以外的東西都關掉
            }
            if (GameObject.Find("PAPA").transform.GetChild(i).gameObject.layer == 16)
            {
                Destroy(GameObject.Find("PAPA").transform.GetChild(i).gameObject);
            }
        }
        this.transform.GetChild(0).gameObject.SetActive(false);
        SceneManager.LoadScene("FightScene", LoadSceneMode.Additive);
        GameObject.Find("GameManager").GetComponent<PlayerManager>().EventGame = game;
        yield return new WaitForSeconds(0.5f);
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("FightScene"));
        this.gameObject.SetActive(false);
    }
}

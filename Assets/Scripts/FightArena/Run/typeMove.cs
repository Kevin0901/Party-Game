using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class typeMove : MonoBehaviour
{
    [SerializeField] private List<string> strAry = new List<string> { "a", "w", "d" };//打字機器
    [SerializeField] private string nowStr;//現在的字
    [SerializeField] private float badTime = 0.75f, greatTime = 0.15f;//生成按鍵時間
    [SerializeField] private float move = 15; //移動距離
    private bool _enter, isRight;
    private int num;
    private void Start()
    {
        _enter = true;
        isRight = false;
        StartCoroutine(changeType());
    }
    private void Update()
    {
        if (Input.anyKeyDown && !(Input.GetMouseButtonDown(0)
         || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2)) && _enter)
        {
            if (Input.GetKeyDown(nowStr))
            {
                StartCoroutine(typeRight(greatTime));
            }
            else
            {
                StartCoroutine(typeWrong(badTime));
            }
        }
    }
    private void FixedUpdate()
    {
        if (isRight)
        {
            this.transform.GetComponentInParent<Rigidbody2D>().MovePosition
            (this.transform.parent.transform.position + new Vector3(move, 0) * Time.deltaTime);
        }
    }
    IEnumerator changeType()
    {
        yield return null;
        num = Random.Range(0, strAry.Count);
        nowStr = strAry[num];
        this.transform.Find("type").GetComponent<TextMeshProUGUI>().text = nowStr.ToUpper();
    }
    IEnumerator typeRight(float t)
    {
        _enter = false;
        isRight = true;
        this.transform.Find("image").GetComponent<Image>().color = Color.green;
        yield return new WaitForSeconds(t);
        num = Random.Range(0, strAry.Count);
        nowStr = strAry[num];
        this.transform.Find("type").GetComponent<TextMeshProUGUI>().text = nowStr.ToUpper();
        this.transform.Find("image").GetComponent<Image>().color = Color.white;
        _enter = true;
        isRight = false;
    }
    IEnumerator typeWrong(float t)
    {
        _enter = false;
        this.transform.Find("image").GetComponent<Image>().color = Color.red;
        yield return new WaitForSeconds(t);
        num = Random.Range(0, strAry.Count);
        nowStr = strAry[num];
        this.transform.Find("type").GetComponent<TextMeshProUGUI>().text = nowStr.ToUpper();
        this.transform.Find("image").GetComponent<Image>().color = Color.white;
        _enter = true;
    }
}

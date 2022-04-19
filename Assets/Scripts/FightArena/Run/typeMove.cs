using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Pun;
public class typeMove : MonoBehaviour
{
    [SerializeField] private List<string> strAry;//打字機器
    public List<Sprite> normalAry, rightAry, wrongAry;
    [SerializeField] private string nowStr;//現在的字
    [SerializeField] private float badTime = 0.75f, greatTime = 0.15f;//生成按鍵時間
    [SerializeField] private float move = 15; //移動距離
    private bool canEnter, isRight;
    private int num;
    PhotonView PV;
    private void Start()
    {
        PV = GetComponentInParent<PhotonView>();
        if (!PV.IsMine)
        {
            Destroy(this.gameObject);
        }
        canEnter = true;
        isRight = false;
        StartCoroutine(changeType());
    }
    private void Update()
    {
        if (Input.anyKeyDown && !(Input.GetMouseButtonDown(0)
        || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2)) && canEnter)
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
        this.transform.Find("image").GetComponent<Image>().sprite = normalAry[num];
    }
    IEnumerator typeRight(float t)
    {
        canEnter = false;
        isRight = true;
        this.transform.Find("image").GetComponent<Image>().sprite = rightAry[num];
        yield return new WaitForSeconds(t);
        isRight = false;

        num = Random.Range(0, strAry.Count);
        nowStr = strAry[num];
        this.transform.Find("image").GetComponent<Image>().sprite = normalAry[num];
        canEnter = true;
    }
    IEnumerator typeWrong(float t)
    {
        canEnter = false;
        this.transform.Find("image").GetComponent<Image>().sprite = wrongAry[num];
        yield return new WaitForSeconds(t);

        num = Random.Range(0, strAry.Count);
        nowStr = strAry[num];
        this.transform.Find("image").GetComponent<Image>().sprite = normalAry[num];
        canEnter = true;
    }
}

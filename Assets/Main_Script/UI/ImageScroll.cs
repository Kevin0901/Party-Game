using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ImageScroll : MonoBehaviour
{
    // public string imgPath;
    // public Sprite highLightPoint;
    // public Sprite darkLightPoint;
    // public Image aImg;
    // public Image bImg;
    // public Image cImg;
    // private Vector3 p1;
    // private Vector3 p2;
    // private Vector3 p3;

    // //判断是否可以点击
    // public bool ask;

    // public List<Image> points;

    // public int imgNum;

    // public float scrollCD;

    // public float scrollTime;

    // public int curIndex;
    // public int nextIndex;

    // private float t = 2.1f;

    // private void Start()
    // {
    //     p1 = aImg.transform.localPosition;
    //     p2 = bImg.transform.localPosition;
    //     p3 = cImg.transform.localPosition;
    //     points.ForEach(p => p.sprite = darkLightPoint);
    //     points[curIndex].sprite = highLightPoint;
    // }

    // // Update is called once per frame
    // void Update()
    // {
    //     t += Time.deltaTime;
    //     if (t > scrollCD)
    //     {
    //         t = 0;
    //         ScrollRight();
    //     }
    // }

    // public void ScrollRight()
    // {
    //     if (ask)
    //     {
    //         return;
    //     }
    //     if (curIndex + 1 >= imgNum)
    //     {
    //         nextIndex = 0;
    //     }
    //     else
    //     {
    //         nextIndex = curIndex + 1;
    //     }

    //     ask = true;
    //     curIndex = nextIndex;
    //     // cImg.sprite = Resources.Load<Sprite>(Path.Combine(imgPath, nextIndex.ToString()));
    //     aImg.transform.localPosition = p3;
    //     bImg.transform.DOLocalMove(p1, scrollTime);
    //     cImg.transform.DOLocalMove(p2, scrollTime).OnComplete(() => { ask = false; });
    //     Image t1 = aImg;
    //     Image t2 = bImg;
    //     Image t3 = cImg;

    //     aImg = t2;
    //     bImg = t3;
    //     cImg = t1;

    //     points.ForEach(p => p.sprite = darkLightPoint);
    //     points[curIndex].sprite = highLightPoint;
    // }

    // public void ScrollLeft()
    // {
    //     if (ask)
    //     {
    //         return;
    //     }
    //     if (curIndex - 1 < 0)
    //     {
    //         nextIndex = imgNum - 1;
    //     }
    //     else
    //     {
    //         nextIndex = curIndex - 1;
    //     }

    //     ask = true;
    //     curIndex = nextIndex;
    //     // cImg.sprite = Resources.Load<Sprite>(Path.Combine(imgPath, nextIndex.ToString()));
    //     cImg.transform.localPosition = p1;
    //     aImg.transform.DOLocalMove(p2, scrollTime);
    //     bImg.transform.DOLocalMove(p3, scrollTime).OnComplete(() => { ask = false; });

    //     Image t1 = aImg;
    //     Image t2 = bImg;
    //     Image t3 = cImg;

    //     aImg = t3;
    //     bImg = t1;
    //     cImg = t2;

    //     points.ForEach(p => p.sprite = darkLightPoint);
    //     points[curIndex].sprite = highLightPoint;
    // }

    // public void Button_left()
    // {
    //     ScrollLeft();
    //     t = 0;
    // }
    // public void Button_right()
    // {
    //     ScrollRight();
    //     t = 0;
    // }
}

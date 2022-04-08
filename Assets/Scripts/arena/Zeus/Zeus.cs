using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zeus : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject flash, warningflash;
    private Quaternion k;
    void Start()
    {
        // InvokeRepeating("spawnFlash", 2, 2f);
        InvokeRepeating("spawnFlash2", 1, 17f);
    }
    void spawnFlash()
    {
        // float x = Random.Range(-54, -18);

        for (int i = 1; i < 4; i++)
        {
            float x = 0;
            switch (i)
            {
                case 1:
                    x = Random.Range(-54, -18);
                    break;
                case 2:
                    x = Random.Range(-17, 18);
                    break;
                case 3:
                    x = Random.Range(19, 54);
                    break;
            }

            Vector3 bornpos = new Vector3(x, 54 - Mathf.Abs(x), 0);
            Vector3 endpos = new Vector3(-(bornpos.x), -(bornpos.y), 0);

            Vector2 line = bornpos - endpos;
            float rotate = Mathf.Atan2(line.y, line.x) * Mathf.Rad2Deg;
            k = Quaternion.AngleAxis(rotate, Vector3.forward);
            StartCoroutine(Warning(endpos, bornpos, k, 200));

        }
        // GameObject arr = Instantiate(flash);   //生成子弹
        // arr.transform.position = bornpos;
        // arr.transform.rotation = k;
        // arr.SendMessage("Pos", endpos);
    }

    void spawnFlash2()
    {
        StartCoroutine(flash2());
    }
    IEnumerator flash2()
    {
        float x = -40;
        float y = -70;

        for (int i = 0; i < 17; i++)
        {
            Vector3 bornpos = new Vector3(x, -(y)-50, 0);
            Vector3 endpos = new Vector3(x, y, 0);
            Vector2 line = bornpos - endpos;
            float rotate = Mathf.Atan2(line.y, line.x) * Mathf.Rad2Deg;
            k = Quaternion.AngleAxis(rotate, Vector3.forward);
            StartCoroutine(Warning(endpos, bornpos, k, 200));
            x += 5;
            yield return new WaitForSeconds(0.5f);
        }
        StartCoroutine(flash21());

    }
    IEnumerator flash21()
    {
        float x2 = 40;
        float y2 = 40;
        for (int i = 0; i < 17; i++)
        {
            Vector3 bornpos = new Vector3(x2, y2, 0);
            Vector3 endpos = new Vector3(-(x2)-30, y2, 0);
            Vector2 line = bornpos - endpos;
            float rotate = Mathf.Atan2(line.y, line.x) * Mathf.Rad2Deg;
            k = Quaternion.AngleAxis(rotate, Vector3.forward);
            StartCoroutine(Warning(endpos, bornpos, k, 200));
            y2 -= 5;
            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator Warning(Vector3 pos, Vector3 pos2, Quaternion k, int speed)
    {
        GameObject warning = Instantiate(warningflash);
        warning.transform.position = pos;
        warning.transform.rotation = k;
        yield return new WaitForSeconds(1f);
        Destroy(warning);

        GameObject arr = Instantiate(flash);   //生成子弹
        arr.transform.position = pos2;
        arr.transform.rotation = k;
        arr.SendMessage("Pos", pos);
        arr.SendMessage("Speed", speed);
    }
}

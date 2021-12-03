using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class REsetGameObject : MonoBehaviour
{
    public bool isRESpawn;
    // Start is called before the first frame update
    void Start()
    {
        isRESpawn = false;
    }

    // Update is called once per frame
    void Update()
    {
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name == "MainScene" && isRESpawn) //如果是主場景
        {
            Cursor.visible = false;
            StartCoroutine(ReSpawn());
            if (this.gameObject.transform.childCount == 0) //如果PAPA下面沒有子物件
            {
                isRESpawn = false;
            }
        }
    }

    private IEnumerator ReSpawn()
    {
        for (int i = 0; i < this.gameObject.transform.childCount; i++) //把MainGameManager從papa拉出來
        {
            GameObject c = transform.GetChild(i).gameObject;
            c.SetActive(true);
            if (c.name == "MainBlackScreen")
            {
                c.GetComponent<Animator>().SetBool("fadeout", true);
                yield return new WaitForSeconds(0.1f);
                c.GetComponent<Animator>().SetBool("fadeout", false);
                // c.transform.GetChild(1).gameObject.SetActive(false);
                c.transform.SetParent(null);
            }
            else
            {
                c.SetActive(false);
            }
        }
        for (int i = 0; i < this.gameObject.transform.childCount; i++) //把MainGameManager從papa拉出來
        {
            GameObject c = transform.GetChild(i).gameObject;
            c.SetActive(true);
            if (c.name == "MainGameManager2")
            {
                c.GetComponent<TimeManager>().randomTime += Random.Range(90, 91);
                c.GetComponent<TimeManager>().isChange = false;
            }
            if (c.layer == 9)
            {
                if(c.GetComponent<monsterMove>().currentState == MonsterState.idle)
                {
                    c.GetComponent<monsterMove>().currentState = MonsterState.walk;
                }
            }
            c.transform.SetParent(null);
        }
    }
}

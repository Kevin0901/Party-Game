using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


public class GoldenSheep : MonoBehaviour
{
    private Animator animator;
    [SerializeField]
    private string bonusteam;   //加成隊伍
    [SerializeField]
    private float speed;  //移動速度
    private bool nowtstatic = true;  //可否被牽
    private GameObject player;  //玩家
    private int playoncatheal; //玩家是否被攻擊
    private int fc = 0; //是否隨機生成走動目的點
    private int mc = 0; //是否移動到目的
    private Vector3 targetpos; //目的點
    private List<Vector3> playerposlist = new List<Vector3>(); //玩家位置
    private GameObject bonusUI;
    PhotonView PV;
    public float timer;
    private float timerMax = 210f;
    private void Start()
    {
        PV = GetComponent<PhotonView>();
        animator = this.gameObject.GetComponent<Animator>();
        timer = timerMax;
    }
    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 1f)
        {
            bonusUI = player.transform.parent.Find("EffectUI").GetChild(0).Find("ResourceUp").gameObject;
            bonusUI.SetActive(false);
            ResourceManager.Instance.Brestimes = 1;
            ResourceManager.Instance.Rrestimes = 1;
        }
        if (!PV.IsMine)
        {
            return;
        }

        if (nowtstatic == false)
        {

            if (player.GetComponent<PlayerMovement>().health.playercatchsheeponhit != 0)
            {
                if (bonusteam == "blue")
                {
                    ResourceManager.Instance.Brestimes = 1;
                }
                else if (bonusteam == "red")
                {
                    ResourceManager.Instance.Rrestimes = 1;
                }
                bonusUI.SetActive(false);
                StartCoroutine(gobackcolddown());
                nowtstatic = true;
                bonusteam = null;
                playerposlist.Clear();
            }
            else
            {
                playerposlist.Add(player.transform.position);
                if (playerposlist.Count > 30)
                {
                    playerposlist.RemoveAt(0);
                    Vector3 direction = (playerposlist[0] - transform.position);
                    Debug.Log(direction.magnitude);
                    if (direction.magnitude > 2)
                    {
                        transform.position += direction.normalized * 8 * Time.fixedDeltaTime;
                        Debug.Log(direction);
                        animator.SetFloat("moveX", Mathf.RoundToInt(direction.normalized.x));
                        animator.SetFloat("moveY", Mathf.RoundToInt(direction.normalized.y));
                    }

                }
            }
        }

        if (nowtstatic == true)
        {
            if (mc == 0)
            {
                if (fc == 0)
                {
                    fc = 1;
                    float x = Random.Range(-40, -0.75f);
                    float y = Random.Range(-11, 11);
                    targetpos = new Vector3(x, y, 0);
                }
                Vector2 direction = targetpos - transform.position;
                transform.position = Vector3.MoveTowards(transform.position, targetpos, Time.deltaTime * speed);
                animator.SetFloat("moveX", direction.x);
                animator.SetFloat("moveY", direction.y);
                if (transform.position == targetpos)
                {
                    mc = 1;
                    StartCoroutine(movecolddown());
                }


            }
        }
    }

    IEnumerator movecolddown()
    {
        yield return new WaitForSeconds(3);
        fc = 0;
        mc = 0;
    }

    IEnumerator gobackcolddown()
    {
        mc = 1;
        yield return new WaitForSeconds(10);
        mc = 0;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        //&& Input.GetKey(KeyCode.E)
        if (other.gameObject.layer == 10 && nowtstatic == true)
        {
            nowtstatic = false;
            player = other.gameObject;
            bonusteam = player.tag;
            player.GetComponent<PlayerMovement>().health.playercatchsheeponhit = 0;
            bonusUI = player.transform.parent.Find("EffectUI").GetChild(0).Find("ResourceUp").gameObject;
            bonusUI.SetActive(true);
            playoncatheal = player.GetComponent<PlayerMovement>().health.playercatchsheeponhit;
            if (bonusteam == "blue")
            {
                ResourceManager.Instance.Brestimes = 2;
            }
            else if (bonusteam == "red")
            {
                ResourceManager.Instance.Rrestimes = 2;
            }
        }
    }


}

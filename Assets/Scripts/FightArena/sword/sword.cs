using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sword : MonoBehaviour
{
    [SerializeField] private GameObject lastplayer;
    private arenaPlayer player;
    [SerializeField] private float hurtTime;
    public float gaveTime = 1;
    private float nexthurt;
    private int num;
    // Start is called before the first frame update
    void Awake()
    {
        player = this.GetComponentInParent<arenaPlayer>();
        num = player.p_index;
        lastplayer = null;
    }
    // Update is called once per frame
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == 10 && (other.gameObject != player.gameObject) && (lastplayer != other.gameObject))
        {
            other.gameObject.transform.Find("sword").gameObject.SetActive(true);
            other.gameObject.transform.Find("sword").gameObject.GetComponent<sword>().Savelastplayer(player.gameObject);
            player.changeColor();
            this.gameObject.SetActive(false);
        }
    }
    private void OnEnable()
    {
        player.speed *= 1.5f;
        player.gameObject.transform.Find("NumTitle").GetChild(num).GetComponent<SpriteRenderer>().color = new Color32(255, 28, 28, 255);
        nexthurt = Time.time;
    }
    private void OnDisable()
    {
        player.speed /= 1.5f;
    }
    private void Hurt()
    {
        player.hurt(0.5f);
    }
    private void Update()
    {
        if (Time.time - nexthurt > hurtTime)
        {
            Hurt();
            nexthurt = Time.time + hurtTime;
        }
    }
    public void Savelastplayer(GameObject p)
    {
        lastplayer = p;
        StartCoroutine(Savelastplayer());
    }
    public IEnumerator Savelastplayer()
    {
        yield return new WaitForSeconds(gaveTime);
        lastplayer = null;
    }
}

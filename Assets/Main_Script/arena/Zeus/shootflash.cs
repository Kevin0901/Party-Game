using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shootflash : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 endPos;
    public int damage = 5;
    private int speed;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, endPos, Time.deltaTime * speed);//移動
        if(transform.position == endPos)
        {
            StartCoroutine(WaitForDestory());
        }
    }
    IEnumerator WaitForDestory()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(this.gameObject);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 10)
        {
            // other.gameObject.GetComponentInChildren<health>().Hurt(damage);
        }
    }
    void Pos(Vector3 vec)
    {
        endPos = vec;
    }
    void Speed(int sp)
    {
        speed = sp;
    }
}

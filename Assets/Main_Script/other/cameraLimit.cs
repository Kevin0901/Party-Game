using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class cameraLimit : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject player;
    private Transform k;
    private Vector2 clamp;
    private float size;
    void Start()
    {
        player = this.transform.parent.gameObject;
        k = GameObject.Find("background").transform;
        clamp = k.position;
        size = this.gameObject.GetComponent<Camera>().orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        clamp.x = Mathf.Clamp(player.transform.position.x, k.position.x - 2, k.position.x + 2);
        clamp.y = Mathf.Clamp(player.transform.position.y, k.position.y - 52 + size, k.position.y + 52 - size);
        this.transform.position = new Vector3(clamp.x, clamp.y, transform.position.z);
    }
}

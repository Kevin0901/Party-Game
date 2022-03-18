using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{

    [SerializeField] private float shaketime; //搖晃時間
    [SerializeField] private float level; //幅度
    [SerializeField] private GameObject player;

    static public bool canshake = false;

    private void Update()
    {
        if (canshake)
        {
            StartCoroutine(shake(shaketime, level));
        }
    }

    public IEnumerator shake(float duration, float magnitude)
    {
        Vector3 orginpos = transform.position;
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            float Xoffset = Random.Range(-0.5f, 0.5f) * magnitude;
            float Yoffset = Random.Range(-0.5f, 0.5f) * magnitude;

            transform.position = new Vector3(player.transform.position.x + Xoffset, player.transform.position.y + Yoffset, orginpos.z);
            elapsedTime += Time.deltaTime;

            yield return null;
        }
    }
}

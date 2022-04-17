using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{

    [SerializeField] private float shaketime = 1; //搖晃時間
    [SerializeField] private float level = 2; //幅度
    [SerializeField] private GameObject player;
    public GameObject SandEffect;
    static public bool canshake = false;
    static public bool cansand = false;
    private Animator an;

    private void Start()
    {
        an = SandEffect.GetComponentInChildren<Animator>();
        SandEffect.SetActive(false);
    }

    private void Update()
    {
        if (canshake)
        {
            StartCoroutine(shake(shaketime, level));
        }
        if (cansand)
        {
            SandEffect.SetActive(true);
        }

        AnimatorStateInfo info = an.GetCurrentAnimatorStateInfo(0);
        if (info.normalizedTime >= 1 && info.IsName("Sand"))
        {
            cansand = false;
            SandEffect.SetActive(false);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnCrystal : MonoBehaviour
{
    [SerializeField] private GameObject spawnPrefabs;
    [SerializeField] private float startdelay, again;
    [Header("隨機生成座標限制")]
    [SerializeField] private float x1 = 0, x2 = 0, y1 = 0, y2 = 0;
    private Vector3 pos;
    void Start()
    {
        StartCoroutine(spawnFirst());
    }
    private void OnEnable()
    {
        StartCoroutine( spawnRandom());
    }
    IEnumerator spawnFirst()
    {
        yield return new WaitForSeconds(startdelay);
        pos = new Vector3(Random.Range(x1, x2), Random.Range(y1, y2), 0);
        Instantiate(spawnPrefabs, pos, spawnPrefabs.transform.rotation);
        StartCoroutine(spawnRandom()); //重複調用函式
    }
    IEnumerator spawnRandom()
    {
        yield return new WaitForSeconds(again);
        pos = new Vector3(Random.Range(x1, x2), Random.Range(y1, y2), 0);
        Instantiate(spawnPrefabs, pos, spawnPrefabs.transform.rotation);
        StartCoroutine(spawnRandom()); //重複調用函式
    }

}

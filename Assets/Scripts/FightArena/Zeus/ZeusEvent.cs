using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZeusEvent : MonoBehaviour
{
    [SerializeField] private GameObject UI, theLight;
    [SerializeField] private float spawnTime;
    // Start is called before the first frame update
    public void StartGame()
    {
        for (int i = 0; i < FightManager.Instance.plist.Count; i++)
        {
            GameObject.Find("HealthUI").transform.GetChild(i).gameObject.SetActive(true);
            FightManager.Instance.plist[i].GetComponent<arenaPlayer>().currentState = ArenaState.lighting;
        }
        StartCoroutine(spawnLight());
    }
    // Update is called once per frame
    void Update()
    {
        if (FightManager.Instance.plist.Count <= 1)
        {
            UI.SetActive(true);
            if (FightManager.Instance.plist[0].GetComponent<arenaPlayer>().red)
            {
                UI.transform.Find("red").gameObject.SetActive(true);
            }
            else
            {
                UI.transform.Find("blue").gameObject.SetActive(true);
            }
            this.gameObject.SetActive(false);
        }
    }
    //生成閃電
    private IEnumerator spawnLight()
    {
        yield return new WaitForSeconds(spawnTime);
        float x = Random.Range(-55f, 55f);
        float y = Random.Range(-33f, 33f);
        Instantiate(theLight, new Vector3(x, y, 0), theLight.transform.rotation);
        StartCoroutine(spawnLight());
    }
}
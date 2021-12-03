using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeReference] public GameObject hover;
    public static GameObject perfab;
    static public float a;
    public TowerBtn ClickedBtn { get; private set; }

    private void Start()
    {
        a = 0;
    }

    public void PickTower(TowerBtn towerBtn)
    {
        this.ClickedBtn = towerBtn;
        hover.SetActive(true);
        Hover.Instance.Activate(towerBtn.Sprite, towerBtn.ObjectPrefab);
        perfab = towerBtn.ObjectPrefab;
        a = 1;
    }


}

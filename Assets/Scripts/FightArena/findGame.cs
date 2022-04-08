using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class findGame : MonoBehaviour
{
    public void chooseGame(int num)
    {
        FightManager.Instance.gameNum(num);
    }
}

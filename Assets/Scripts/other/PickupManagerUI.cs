using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PickupManagerUI : MonoBehaviour
{
    private static PickupManagerUI instance;
    private Text coinText; 
    private int coins; 
    // Start is called before the first frame update
    private void Awake(){
        instance = this; 
    }
    public void AddCoin(){
        coins++;
        coinText.text = "水晶:" + coins.ToString();
    }
}

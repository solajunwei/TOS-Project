using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInterface : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowShop()
    {
        Debug.Log("ShowShop ==== ");
        UIManager.Instance.ShowPanel<BasePanel>("Perfabs/Login/Shop");
    }
}

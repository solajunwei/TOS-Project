using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveFile : BasePanel
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onBack()
    {
        UIManager.GetInstance().HidePanel("Perfabs/Login/SaveFile");
    }

    public void HideMe()
    {
        Destroy(gameObject);
    }

    // ¼òµ¥
    public void onOpenSaveEase()
    {
        UIManager.GetInstance().ShowPanel<BasePanel>("Perfabs/Login/SelectLevel");
    }

    // À§ÄÑ
    public void onOpenSaveDifficulty()
    {
        UIManager.GetInstance().ShowPanel<BasePanel>("Perfabs/Login/SelectLevel");
    }

    public void onOpenNewGame()
    {
        UIManager.GetInstance().ShowPanel<BasePanel>("Perfabs/Login/SelectLevel");
    }    
}

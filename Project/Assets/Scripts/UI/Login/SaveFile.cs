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
        UIManager.Instance.HidePanel("Perfabs/Login/SaveFile");
    }


    // ¼òµ¥
    public void onOpenSaveEase()
    {
        UIManager.Instance.ShowPanel<SelectLevel>("Perfabs/Login/SelectLevel");
    }

    // À§ÄÑ
    public void onOpenSaveDifficulty()
    {
        UIManager.Instance.ShowPanel<SelectLevel>("Perfabs/Login/SelectLevel");
    }

    public void onOpenNewGame()
    {
        UIManager.Instance.ShowPanel<SelectLevel>("Perfabs/Login/SelectLevel");
    }    
}

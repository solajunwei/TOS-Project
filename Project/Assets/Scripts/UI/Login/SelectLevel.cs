using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectLevel : BasePanel
{
    public void onBack()
    {
        UIManager.GetInstance().HidePanel("Perfabs/Login/SelectLevel");
    }

    public void HideMe()
    {
        Destroy(gameObject);
    }
    
    public void onInGame()
    {
        Debug.Log("Ω¯»Î”Œœ∑");
    }
}

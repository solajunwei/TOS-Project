using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectLevel : BasePanel
{
    public void onBack()
    {
        UIManager.Instance.HidePanel("Perfabs/Login/SelectLevel");
    }

    public void onInGame()
    {
        UIManager.Instance.ShowPanel<Pet>("Perfabs/Pet/Pet");
    }
}

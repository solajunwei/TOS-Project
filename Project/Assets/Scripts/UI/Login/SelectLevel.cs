using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectLevel : UIView
{
    public void onBack()
    {
        UIManager.Instance.GoBack();
    }

    public void onInGame()
    {
        UIManager.Instance.OpenView<Pet>("Perfabs/Pet/Pet");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelLevel : BasePanel
{
    public void onClickLand(int tag)
    {
        Debug.Log("tag === " + tag);
        UIManager.Instance.HidePanel("Perfabs/Main/Setting");
        UIManager.Instance.ShowPanel<SaveFile>("Perfabs/Login/SaveFile");
    }

    public void onClickBack()
    {
        UIManager.Instance.HidePanel("Perfabs/Login/SelLevel");
    }

    public void onClickPet()
    {
        //UIManager.Instance.ShowPanel<SaveFile>("Perfabs/Login/SaveFile");
    }

    public void onClickUp()
    {

    }
}

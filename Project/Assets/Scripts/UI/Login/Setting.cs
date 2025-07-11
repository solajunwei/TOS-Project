using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Setting : BasePanel
{
    public void onClickSetting()
    {
        Debug.Log("onClickSetting");
    }

    public void onCloseSetting()
    {
        UIManager.Instance.HidePanel("Perfabs/Main/Setting");
    }
}

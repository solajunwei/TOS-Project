using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Setting : MonoBehaviour
{
    public void onClickSetting()
    {
        Debug.Log("onClickSetting");
    }

    public void HideMe()
    {
        Destroy(gameObject);
    }
}

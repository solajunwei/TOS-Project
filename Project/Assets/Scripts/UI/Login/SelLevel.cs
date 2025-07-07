using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelLevel : MonoBehaviour
{
    public void onClickLand(int tag)
    {
        Debug.Log("tag === " + tag);
        UIManager.GetInstance().ShowPanel<BasePanel>("Perfabs/Login/SaveFile");
    }

    public void onClickBack()
    {

    }

    public void onClickPet()
    {
        
    }

    public void onClickUp()
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class choseUser : MonoBehaviour
{

    public void onClickNext()
    {
        UIManager.GetInstance().ShowPanel<BasePanel>("Perfabs/Login/selLevel");
    }
}

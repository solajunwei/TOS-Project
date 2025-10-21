using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Login : UIComponent
{
    void Start()
    {
        Debug.Log("Start");
    }

    public void onStartGame()
    {
        UIManager.Instance.ShowPanel<SelLevel>("Perfabs/Login/SelLevel");
    }

    public void onSetting()
    {
        
    }

    public void onClickTitleMap()
    {
        EventManager.Instance.EventTrigger(MyConstants.start_game);
    }

    public void onExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}

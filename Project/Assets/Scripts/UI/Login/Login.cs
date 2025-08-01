using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Login : BasePanel
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start");
    }

    // Update is called once per frame
    void Update()
    {
        
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
        Debug.Log("onExitGame");
        Application.Quit();
    }
}

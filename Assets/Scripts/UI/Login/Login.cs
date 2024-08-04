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
        Debug.Log("onStartGame");
        UIManager.GetInstance().ShowPanel<BasePanel>("Perfabs/Login/choseUser");
        
        //GameObject obj = ResManager.GetInstance().Load<GameObject>("UI/Perfabs/Login/choseUser");
        //DontDestroyOnLoad(obj);
        // ʵ����Ԥ����  
        //Instantiate(obj, transform.position, Quaternion.identity);
    }

    public void onExitGame()
    {
        Debug.Log("onExitGame");
        Application.Quit();
    }
}

using cfg;
using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class StartScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameConfig.GetInstance().getTables();
        MonoMgr.GetInstance();
        UIManager.GetInstance().ShowPanel<BasePanel>("Perfabs/Login/Login");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

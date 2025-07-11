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
        GameConfig.Instance.getTables();
        var mono = MonoMgr.Instance;
        UIManager.Instance.ShowPanel<Login>("Perfabs/Login/Login");
        UIManager.Instance.ShowPanel<Setting>("Perfabs/Main/Setting", E_UI_Layer.Top);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

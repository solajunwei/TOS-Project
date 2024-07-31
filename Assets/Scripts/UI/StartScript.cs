using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MonoMgr.GetInstance();
        UIManager.GetInstance().ShowPanel<BasePanel>("Perfabs/Login/Image");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

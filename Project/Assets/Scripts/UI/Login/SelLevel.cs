using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelLevel : MonoBehaviour
{

    public GameObject levelEasy;
    public GameObject levelMedium;
    public GameObject levelDifficulty;
    // Start is called before the first frame update
    void Start()
    {
        levelEasy.SetActive(true);
        levelDifficulty.SetActive(false);
        levelMedium.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickModel(string arg)
    {
        Debug.Log("arg ==== " + arg);

        levelEasy.SetActive(arg == "1");
        levelDifficulty.SetActive(arg == "3");
        levelMedium.SetActive(arg == "2");
    }

    public void OnClickStart()
    {
        UIManager.GetInstance().ShowPanel<BasePanel>("Perfabs/Login/GameInterface");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : BasePanel
{
    // Start is called before the first frame update
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        EventManager.Instance.AddEventListener(MyConstants.start_game, startGame);
    }

    private void OnDestroy()
    {
        EventManager.Instance.RemoveEventListener(MyConstants.start_game, startGame);
    }

    public void startGame()
    {
        Debug.Log("startGame");
        UnityEngine.SceneManagement.SceneManager.LoadScene("BattleScene");
        UIManager.Instance.SetCanvasVisible(false);
    }

}

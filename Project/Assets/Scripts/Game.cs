using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Game : UIComponent
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
        _ = RunBattleScene();
    }

    public async Task RunBattleScene()
    {
        await ResManager.Instance.LoadSceneAsync("BattleScene");
        UIManager.Instance.SetCanvasVisible(false);
    }

}

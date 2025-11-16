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
        EventManager.Instance.AddEventListener(MyConstants.backUI, backgame);
    }

    private void OnDestroy()
    {
        EventManager.Instance.RemoveEventListener(MyConstants.start_game, startGame);
        EventManager.Instance.RemoveEventListener(MyConstants.backUI, backgame);
    }



    public void startGame()
    {
        _ = RunBattleScene();
    }

    public async Task RunBattleScene()
    {
        await ResManager.Instance.LoadSceneAsync("BattleScene");
       // UIManager.Instance.SetCanvasVisible(false);
    }

    public void backgame()
    {
        _ = BackBattleScene();
    }

    public async Task BackBattleScene()
    {
        await ResManager.Instance.LoadSceneAsync("Towers");
    }
}

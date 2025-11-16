using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameResult : UIView
{
    public void onBack()
    {
        EventManager.Instance.EventTrigger(MyConstants.backUI);
        
    }
}

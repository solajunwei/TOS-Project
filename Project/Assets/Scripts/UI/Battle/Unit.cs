using cfg;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public ZProgress _scrollBar;
    protected int _HP = 100;
    protected int _HPMax = 100;
    protected petConfig _petConfig;

    protected int petConfigID = 11001;
    protected int _point = 10; // 单位的金币个数

    protected int _PetID;
    public int PetID
    {
        set { _PetID = value; }
        get { return _PetID; }
    }

    // 当前单位的等级
    protected int _Level = 0;
    public int Level
    {
        set { _Level = value; }
        get { return _Level; }
    }

    protected virtual void Start()
    {
        _scrollBar.SetProgress(1);
    }

    protected virtual void OnDestroy()
    {
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroUnit : Unit
{

    private void Start()
    {
        this._Level = 1;
    }

    /// <summary>
    /// 是否正在等待攻击
    /// </summary>
    private bool _isWaiting = false;
    public bool IsWaiting
    {
        get {  return _isWaiting; }
        set { _isWaiting = value; }
    }

    private float _waitTime = 3f;

    private void Update()
    {
        if (_isWaiting)
        {
            _waitTime -= Time.deltaTime;
            if(_waitTime < 0f )
            {
                _isWaiting = false;
                _waitTime = 3f;
            }
        }
    }
}

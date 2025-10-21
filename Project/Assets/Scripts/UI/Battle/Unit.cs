using cfg;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public ZProgress _scrollBar;
    private int _HP = 100;
    private int _HPMax = 100;
    private petConfig _petConfig;

    void Start()
    {
        _scrollBar.SetProgress(1);
    }

    /// <summary>
    /// 受到攻击
    /// </summary>
    /// <param name="attack">伤害数值</param>
    public void UnderAttack(int attack)
    {
        if (_HP <= 0)
        {
            return;
        }

        Debug.Log("Attack === " + _HP);
        _HP -= attack;
        if (_HP <= 0)
        {
            _HP = 0;
            Debug.Log("eventManager ==== ");
            EventManager.Instance.EventTrigger<GameObject>(MyConstants.Enemy_deal, gameObject);
        }
        _scrollBar.SetProgress((float)_HP / _HPMax);
    }
}

using cfg;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{
    public ZProgress _scrollBar;
    protected int _HP = 100;
    protected int _HPMax = 100;
    protected petConfig _petConfig;

    protected int petConfigID = 11001;

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

    public Button _BtnSell;
    public Button _BtnUp;


    private void Start()
    {
        _scrollBar.SetProgress(1);
        EventManager.Instance.AddEventListener<int>(MyConstants.unit_show_can_up, onUnitUp);
        EventManager.Instance.AddEventListener<int>(MyConstants.unit_sell, onUnitSell);
    }

    private void OnDestroy()
    {
        EventManager.Instance.RemoveEventListener<int>(MyConstants.unit_show_can_up, onUnitUp);
        EventManager.Instance.RemoveEventListener<int>(MyConstants.unit_sell, onUnitSell);
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

        _HP -= attack;
        if (_HP <= 0)
        {
            _HP = 0;
            EventManager.Instance.EventTrigger<GameObject>(MyConstants.Enemy_deal, gameObject);
        }
        _scrollBar.SetProgress((float)_HP / _HPMax);
    }

    // 显示进化按钮
    protected void onUnitUp(int petId)
    {
        if (petConfigID == petId && _BtnUp)
        {
            _BtnUp.gameObject.SetActive(true);
        }
    }

    // 显示出售按钮
    protected void onUnitSell(int petId)
    {
        if (petId == petConfigID && _BtnSell)
        {
            _BtnSell.gameObject.SetActive(true);
        }
    }

    public void onClickBtn()
    {
        if ( _BtnSell)
        {
            _BtnSell.gameObject.SetActive(true);
        }
    }
    
    public void onClickSell()
    {
        Debug.Log("出售");
    }

    public void onClickUp()
    {
        Debug.Log("升级");
        _Level++;
        EventManager.Instance.EventTrigger<int>(MyConstants.unit_up, petConfigID);
    }
}

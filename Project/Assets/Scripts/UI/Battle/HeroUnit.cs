using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HeroUnit : Unit
{
    public Button _BtnSell;
    public Button _BtnUp;
    public Canvas _canvas;

    private float _fDistance = 3f; // 宠物的攻击距离
    public float Distance
    {
        get{return _fDistance;}
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

    protected override void Start()
    {
        base.Start();
        this._Level = 1;

        EventManager.Instance.AddEventListener<int>(MyConstants.unit_show_can_up, onShowUnitUp);
        EventManager.Instance.AddEventListener<int>(MyConstants.unit_hide_can_up, onHideUnitUp);
        EventManager.Instance.AddEventListener<int>(MyConstants.unit_sell, onUnitSell);
        EventManager.Instance.AddEventListener(MyConstants.hide_unit_sell, onHideUnitSell);
        EventManager.Instance.AddEventListener<int>(MyConstants.unit_up, onUnitUp);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        EventManager.Instance.RemoveEventListener<int>(MyConstants.unit_show_can_up, onShowUnitUp);
        EventManager.Instance.RemoveEventListener<int>(MyConstants.unit_hide_can_up, onHideUnitUp);
        EventManager.Instance.RemoveEventListener<int>(MyConstants.unit_sell, onUnitSell);
        EventManager.Instance.RemoveEventListener(MyConstants.hide_unit_sell, onHideUnitSell);
        EventManager.Instance.RemoveEventListener<int>(MyConstants.unit_up, onUnitUp);
    }

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

        // 检测鼠标左键按下
        if (Input.GetMouseButtonDown(0))
        {
            // 判断是否有出售或升级按钮
            if (!_BtnSell.gameObject.activeSelf && !_BtnUp.gameObject.activeSelf)
            {
                return;
            }

            GraphicRaycaster uiRaycaster = _canvas.GetComponent<GraphicRaycaster>();
            EventSystem eventSystem = EventSystem.current;

            // 创建射线检测数据（位置为当前鼠标屏幕坐标）
            PointerEventData pointerData = new PointerEventData(eventSystem);
            pointerData.position = Input.mousePosition;

            // 存储所有命中的UI元素
            List<RaycastResult> hitResults = new List<RaycastResult>();
            uiRaycaster.Raycast(pointerData, hitResults);

            if(hitResults.Count == 0)
            {
                _BtnSell.gameObject.SetActive(false);
                _BtnUp.gameObject.SetActive(false);
            }
        }
    }

    public void onShowUnitUp(int petId)
    {
        if (petConfigID == petId && _BtnUp)
        {
            _BtnUp.gameObject.SetActive(true);
        }
    }

    protected void onHideUnitUp(int petId)
    {
        if (petConfigID == petId && _BtnUp)
        {
            _BtnUp.gameObject.SetActive(false);
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

    protected void onHideUnitSell()
    {
        if(_BtnSell != null)
        {
            _BtnSell.gameObject.SetActive(false);
        }
    }

    public void onClickSell()
    {
        Debug.Log("出售");
        BattleModel.Instance.removePlayerUnit(gameObject);
        Destroy(gameObject);
        EventManager.Instance.EventTrigger<int>(MyConstants.add_Point, _point);
    }


    public void onClickUp()
    {
        Debug.Log("升级");
        EventManager.Instance.EventTrigger<int>(MyConstants.unit_up, petConfigID);
    }
        
    protected void onUnitUp(int petId)
    {
        if (petId == petConfigID && _BtnSell)
        {
            _Level++;
        }
    }

#region Inspectord的按钮事件
    public void onClickBtn()
    {
        if ( _BtnSell)
        {
            _BtnSell.gameObject.SetActive(true);
        }
    }
#endregion
}

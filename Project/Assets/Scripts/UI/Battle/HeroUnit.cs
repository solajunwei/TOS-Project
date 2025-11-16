using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HeroUnit : Unit
{

    public GraphicRaycaster uiRaycaster;

    protected override void Start()
    {
        base.Start();
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

        //if (Input.GetMouseButtonDown(0))
        //{
        //    //GraphicRaycaster uiRaycaster = _BtnSell[0].canvas.GetComponent<GraphicRaycaster>();
        //    EventSystem eventSystem = EventSystem.current;

        //    // 创建射线检测数据（位置为当前鼠标屏幕坐标）
        //    PointerEventData pointerData = new PointerEventData(eventSystem);
        //    pointerData.position = Input.mousePosition;

        //    // 存储所有命中的UI元素
        //    List<RaycastResult> hitResults = new List<RaycastResult>();
        //    uiRaycaster.Raycast(pointerData, hitResults);

        //    // 遍历命中结果
        //    bool isFind = false;
        //    foreach (RaycastResult result in hitResults)
        //    {
        //        if (result.gameObject == _BtnSell.gameObject)
        //        {
        //            isFind = true;
        //            break; // 找到后退出循环
        //        }
        //    }

        //    if (!isFind)
        //    {
        //        Debug.Log("find ==== ");
        //        EventManager.Instance.EventTrigger(MyConstants.hide_unit_sell);
        //    }
        //}

    }
}

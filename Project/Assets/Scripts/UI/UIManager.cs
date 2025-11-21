using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

//UI层级枚举
public enum E_UI_Layer {
    Bot,
    Mit,
    Top
}


//UI管理器（管理面板）
//管理所有显示的面板
//提供给外部 显示和隐藏
public class UIManager : BaseManager<UIManager>
{
    public Dictionary<string, UIComponent> panelDic = new Dictionary<string, UIComponent>();

    public Dictionary<E_UI_Layer, Stack<UIView>> showPanelDic = new Dictionary<E_UI_Layer, Stack<UIView>>();

    private Stack<UIView> _viewStack = new Stack<UIView>();
    // 旧界面的定时器（键：界面实例，值：对应的定时器）
    private Dictionary<UIView, UIDelayCloseTimer> _delayTimers = new Dictionary<UIView, UIDelayCloseTimer>();

    [Tooltip("旧界面自动关闭延迟（秒）")]
    public float autoCloseDelay = 10f;

    //这是几个UI面板
    private Transform bot;
    private Transform mid;
    private Transform top;
    private Canvas _canvas;
    public Canvas UICanvas
    {
        get { return _canvas; }
    }

    public UIManager() {
        //去找Canvas（做成了预设体在Resources/UI下面）
        GameObject obj= ResManager.Instance.Load<GameObject>("UI/Perfabs/Main/Canvas");
        _canvas = obj.GetComponent<Canvas>();
        if (_canvas != null)
        {
            CanvasScaler canvasScaler = _canvas.GetComponent<CanvasScaler>();
            if(canvasScaler != null)
            {
                canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;
            }
        }

        Transform canvasTransform = obj.transform;
        //创建Canvas，让其过场景的时候不被移除
        GameObject.DontDestroyOnLoad(obj);

        //找到各层
        bot = canvasTransform.Find("bot");
        mid = canvasTransform.Find("mid");
        top = canvasTransform.Find("top");

        //加载EventSystem，有了它，按钮等组件才能响应
        obj = ResManager.Instance.Load<GameObject>("UI/Perfabs/Main/EventSystem");

        //创建Canvas，让其过场景的时候不被移除
        GameObject.DontDestroyOnLoad(obj);
    }

    public void OpenView<T>(string panelName, E_UI_Layer layer=E_UI_Layer.Mit, UnityAction<T> callback=null) where T:UIView 
    {
        ResManager.Instance.LoadAsync<GameObject>("UI/"+panelName,(obj)=> {
            //把它作为Canvas的子对象
            //并且设置它的相对位置
            //找到父对象
            Transform father = bot;
            switch (layer) {
                case E_UI_Layer.Mit:
                    father = mid;
                    break;
                case E_UI_Layer.Bot:
                    father = bot;
                    break;
                case E_UI_Layer.Top:
                    father = top;
                    break;
            }
            //设置父对象
            obj.transform.SetParent(father);

            //设置相对位置和大小
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;

            (obj.transform as RectTransform).offsetMax = Vector2.zero;
            (obj.transform as RectTransform).offsetMin = Vector2.zero;

            //得到预设体身上的脚本（继承自BasePanel）
            T panel = obj.GetComponent<T>();
            panel.UILayer = layer;

            //执行外面想要做的事情
            if (callback != null) {
                callback(panel);
            }

            Stack<UIView> stack = new Stack<UIView>();
            if (showPanelDic.ContainsKey(layer))
            {
                stack = showPanelDic[layer];
            }
            else
            {
                showPanelDic[layer] = stack;
            }

            if(stack.Count > 0)
            {
                UIView oldView = stack.Peek();
                oldView.Hide(); // 隐藏旧界面

                //  // 创建并启动定时器
                // var timer = new UIDelayCloseTimer(autoCloseDelay, () => 
                // {
                //     //CloseOldView(oldView); // 计时结束关闭旧界面
                // });

                // // 记录定时器（若旧界面已有定时器，先停止旧的）
                // if (_delayTimers.ContainsKey(oldView))
                // {
                //     _delayTimers[oldView].Stop();
                //     _delayTimers[oldView] = timer;
                // }
                // else
                // {
                //     _delayTimers.Add(oldView, timer);
                // }
                // timer.Start();
            }

            // 新界面入栈并显示
            stack.Push(panel);
            panel.Show();
        });
    }

    /// <summary>
    /// 返回上一个界面（停止旧界面的定时器并显示）
    /// </summary>
    public void GoBack(E_UI_Layer layer=E_UI_Layer.Mit)
    {
        if (showPanelDic.ContainsKey(layer))
        {
            Stack<UIView> stack = showPanelDic[layer];
            // 关闭当前界面
            UIView currentView = stack.Pop();
            
            if (stack.Count == 0)
            {
                currentView.Hide(); // 最后一个隐藏
                showPanelDic.Remove(layer);
                return;
            } 
            else
            {
                currentView.Close(); // 不是最后一个删除
            }

            // 显示上一个界面，并停止其定时器
            UIView prevView = stack.Peek();
            prevView.Show();

            if (_delayTimers.TryGetValue(prevView, out var timer))
            {
                timer.Stop(); // 停止自动关闭
                _delayTimers.Remove(prevView); // 移除定时器
            }
        }
    }

    /// <summary>
    /// 关闭旧界面（从栈和字典中移除） 栈不支持指定元素移除，所以这个以后再做TODO
    /// </summary>
    private void CloseOldView(UIView oldView)
    {
        if (_viewStack.Contains(oldView) && _viewStack.Peek() != oldView) // 确保不是当前显示的界面
        {
            oldView.Close();
            // _viewStack.Remove(oldView); // 从栈中移除
        }

        if (_delayTimers.ContainsKey(oldView))
        {
            _delayTimers.Remove(oldView); // 清理定时器
        }
    }

    public void SetCanvasVisible(bool visible)
    {
        _canvas.enabled = visible;
    }
}

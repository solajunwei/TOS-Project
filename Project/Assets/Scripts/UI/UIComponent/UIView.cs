
using System;
using UnityEngine;

/// <summary>
/// UI专用延迟关闭定时器（绑定界面，支持中断）
/// </summary>
public class UIDelayCloseTimer
{
    private float _delaySeconds; // 延迟关闭时间（秒）
    private float _currentTime;  // 当前累计时间
    private bool _isRunning;     // 是否运行中
    private Action _onComplete;  // 计时结束回调（关闭界面）

    // 构造函数：传入延迟时间和关闭回调
    public UIDelayCloseTimer(float delaySeconds, Action onComplete)
    {
        _delaySeconds = delaySeconds;
        _onComplete = onComplete;
        _currentTime = 0;
        _isRunning = false;
    }

    /// <summary>
    /// 启动定时器（需在UI管理器的Update中驱动）
    /// </summary>
    public void Start()
    {
        _isRunning = true;
        _currentTime = 0;
    }

    /// <summary>
    /// 停止定时器（用户返回旧界面时调用）
    /// </summary>
    public void Stop()
    {
        _isRunning = false;
    }

    /// <summary>
    /// 每帧更新（由UI管理器调用，传入Time.deltaTime）
    /// </summary>
    public void Update()
    {
        if (!_isRunning) return;

        _currentTime += Time.deltaTime;
        if (_currentTime >= _delaySeconds)
        {
            _isRunning = false;
            _onComplete?.Invoke(); // 触发关闭界面
        }
    }
}


/// <summary>
/// UI界面基类（每个界面需继承）
/// </summary>
public class UIView : MonoBehaviour
{
    [Tooltip("界面关闭时是否销毁（默认true）")]
    public bool destroyOnClose = true;

    // layer层
    [HideInInspector]
    public E_UI_Layer UILayer = E_UI_Layer.Bot;

    // 界面显示
    public virtual void Show()
    {
        gameObject.SetActive(true);
    }

    // 界面隐藏（不销毁）
    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }

    // 界面关闭（销毁或回收）
    public virtual void Close()
    {
        // if (destroyOnClose)
        //     Destroy(gameObject);
        // else
        //     Hide(); // 若复用则仅隐藏

        Destroy(gameObject);
    }
}

/// <summary>
/// UI管理器（管理界面栈和定时器）
/// </summary>
// public class UIManager1 : MonoBehaviour
// {
//     void Update()
//     {
//         // 每帧更新所有活跃的定时器
//         foreach (var timer in _delayTimers.Values)
//         {
//             timer.Update();
//         }
//     }
// }


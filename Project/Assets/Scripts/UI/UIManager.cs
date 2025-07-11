﻿using System.Collections;
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
    public Dictionary<string, UIComponent> panelDic 
        = new Dictionary<string, UIComponent>();

    //这是几个UI面板
    private Transform bot;
    private Transform mid;
    private Transform top;

    public Canvas canvas;
    public UIManager() {
        //去找Canvas（做成了预设体在Resources/UI下面）
        GameObject obj= ResManager.Instance.Load<GameObject>("UI/Perfabs/Main/Canvas");
        Canvas canvas = obj.GetComponent<Canvas>();
        if (canvas != null)
        {
            CanvasScaler canvasScaler = canvas.GetComponent<CanvasScaler>();
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
    //public onAv
       
    public void ShowPanel<T>(string panelName,
                        E_UI_Layer layer=E_UI_Layer.Mit,
                        UnityAction<T> callback=null) where T:UIComponent {
        //已经显示了此面板
        if (panelDic.ContainsKey(panelName))
        {
            //调用重写方法，具体内容自己添加
            panelDic[panelName].ShowMe();
            if (callback!=null)
                callback(panelDic[panelName] as T);
            return;
        }
        ResManager.Instance.LoadAsync<GameObject>("UI/"+panelName,(obj)=> {
            //把它作为Canvas的子对象
            //并且设置它的相对位置
            //找到父对象
            Transform father = bot;
            switch (layer) {
                case E_UI_Layer.Mit:
                    father = mid;
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

            //执行外面想要做的事情
            if (callback != null) {
                callback(panel);
            }

            //在字典中添加此面板
            panelDic.Add(panelName, panel);
        });
    }
    //隐藏面板
    public void HidePanel(string panelName) {
        if (panelDic.ContainsKey(panelName)) {
            //调用重写方法，具体内容自己添加
            panelDic[panelName].HideMe();
            //GameObject.Destroy(panelDic[panelName].gameObject);
            panelDic.Remove(panelName);
        }
    }
}

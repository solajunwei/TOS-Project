
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// 列表项视图基类
//public abstract class ListItemView : MonoBehaviour
public class ListItemView : MonoBehaviour
{
    public int ItemIndex { get; private set; }

    public virtual void Initialize(int index, IListItemData data)
    {
        ItemIndex = index;
        this.UpdateView(data);
    }

    protected virtual void UpdateView(IListItemData data)
    {

    }
}

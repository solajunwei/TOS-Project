
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
        UpdateView(data);
    }

    [SerializeField] private Text titleText;
    [SerializeField] private Text descriptionText;
    [SerializeField] private Image backgroundImage;

    private void Awake()
    {
    }

    public void UpdateView(IListItemData data)
    {
        Debug.Log("UpdateView === ");
    }
}

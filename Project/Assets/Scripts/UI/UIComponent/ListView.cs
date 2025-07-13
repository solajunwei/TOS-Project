using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// 列表项数据接口
public interface IListItemData
{
    int GetItemId(); // 唯一标识
}
// 示例数据类
public class ExampleItemData : IListItemData
{
    public int Id { get; set; }
    public int GetItemId() => Id;
}

// 列表视图组件
[RequireComponent(typeof(ScrollRect))]
[RequireComponent(typeof(RectTransform))]
public class ListView : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    [Header("组件引用")]
    [SerializeField] private RectTransform viewport;
    [SerializeField] private RectTransform content;
    [SerializeField] private Scrollbar verticalScrollbar;
    [SerializeField] private Scrollbar horizontalScrollbar;
    
    [Header("配置参数")]
    [SerializeField] private ListItemView itemPrefab;
    [SerializeField] private Vector2 itemSize = new Vector2(100, 50);
    [SerializeField] private Vector2 spacing = Vector2.zero;
    [SerializeField] private int bufferItems = 3; // 缓冲区大小
    [SerializeField] private bool isVertical = true;
    
    // 数据和视图管理
    private List<IListItemData> dataSource = new List<IListItemData>();
    private Dictionary<int, ListItemView> activeItems = new Dictionary<int, ListItemView>();
    private Queue<ListItemView> pooledItems = new Queue<ListItemView>();
    
    // 滚动优化
    private float contentSize;
    private float viewportSize;
    private int visibleItemCount;
    private int startIndex = 0;
    private int endIndex = 0;
    private bool isDragging = false;
    private Vector2 lastPosition;
    
    // 事件
    public Action<int, IListItemData> OnItemClicked;
    public Action<int, IListItemData> OnItemSelected;
    
    private ScrollRect scrollRect;
    private RectTransform rectTransform;
    
    private void Awake()
    {
        scrollRect = GetComponent<ScrollRect>();
        rectTransform = GetComponent<RectTransform>();
        
        // 设置ScrollRect组件
        scrollRect.content = content;
        scrollRect.vertical = isVertical;
        scrollRect.horizontal = !isVertical;
        
        if (verticalScrollbar) scrollRect.verticalScrollbar = verticalScrollbar;
        if (horizontalScrollbar) scrollRect.horizontalScrollbar = horizontalScrollbar;
        
        // 初始化内容布局
        InitContentLayout();
        
        // 注册滚动事件
        scrollRect.onValueChanged.AddListener(OnScroll);
    }
    
    private void InitContentLayout()
    {
        // 根据方向设置内容大小
        if (isVertical)
        {
            content.anchorMin = new Vector2(0, 1);
            content.anchorMax = new Vector2(1, 1);
            content.pivot = new Vector2(0.5f, 1);
        }
        else
        {
            content.anchorMin = new Vector2(0, 0);
            content.anchorMax = new Vector2(0, 1);
            content.pivot = new Vector2(0, 0.5f);
        }
        
        content.sizeDelta = Vector2.zero;
    }
    
    // 设置数据源
    public void SetDataSource(List<IListItemData> data)
    {
        ClearItems();
        dataSource = data ?? new List<IListItemData>();
        Refresh();
    }
    
    // 添加单个数据项
    public void AddItem(IListItemData item)
    {
        if (item == null) return;
        
        dataSource.Add(item);
        Refresh();
    }
    
    // 在指定位置插入数据项
    public void InsertItem(int index, IListItemData item)
    {
        if (item == null || index < 0 || index > dataSource.Count) return;
        
        dataSource.Insert(index, item);
        Refresh();
    }
    
    // 移除数据项
    public void RemoveItem(int index)
    {
        if (index < 0 || index >= dataSource.Count) return;
        
        dataSource.RemoveAt(index);
        Refresh();
    }
    
    // 清除所有数据项
    public void ClearItems()
    {
        // 回收所有活动项
        foreach (var item in activeItems.Values)
        {
            item.gameObject.SetActive(false);
            pooledItems.Enqueue(item);
        }
        
        activeItems.Clear();
        dataSource.Clear();
    }
    
    // 刷新列表
    public void Refresh()
    {
        // 计算内容大小
        float totalSize = CalculateContentSize();
        content.sizeDelta = isVertical ? 
            new Vector2(content.sizeDelta.x, totalSize) : 
            new Vector2(totalSize, content.sizeDelta.y);
        
        // 获取视口大小
        viewportSize = isVertical ? viewport.rect.height : viewport.rect.width;
        
        // 计算可见项数量
        visibleItemCount = Mathf.FloorToInt(viewportSize / (isVertical ? itemSize.y : itemSize.x));
        
        // 更新可见项
        UpdateVisibleItems();
    }
    
    private float CalculateContentSize()
    {
        if (dataSource.Count == 0) return 0;
        
        float itemDimension = isVertical ? itemSize.y : itemSize.x;
        float spacingDimension = isVertical ? spacing.y : spacing.x;
        
        return (itemDimension * dataSource.Count) + (spacingDimension * (dataSource.Count - 1));
    }
    
    private void UpdateVisibleItems()
    {
        if (dataSource.Count == 0) return;
        
        // 获取当前滚动位置
        float position = isVertical ? content.anchoredPosition.y : -content.anchoredPosition.x;
        
        // 计算起始索引
        float itemDimension = isVertical ? itemSize.y : itemSize.x;
        float spacingDimension = isVertical ? spacing.y : spacing.x;
        float totalItemHeight = itemDimension + spacingDimension;
        
        startIndex = Mathf.Max(0, Mathf.FloorToInt(position / totalItemHeight) - bufferItems);
        endIndex = Mathf.Min(dataSource.Count - 1, startIndex + visibleItemCount + (bufferItems * 2));
        
        // 回收不可见项
        List<int> keysToRemove = new List<int>();
        foreach (var pair in activeItems)
        {
            if (pair.Key < startIndex || pair.Key > endIndex)
            {
                pair.Value.gameObject.SetActive(false);
                pooledItems.Enqueue(pair.Value);
                keysToRemove.Add(pair.Key);
            }
        }
        
        foreach (int key in keysToRemove)
        {
            activeItems.Remove(key);
        }
        
        // 创建或更新可见项
        for (int i = startIndex; i <= endIndex; i++)
        {
            if (!activeItems.ContainsKey(i))
            {
                CreateOrGetItem(i);
            }
            
            // 更新项数据
            activeItems[i].Initialize(i, dataSource[i]);
            
            // 设置项位置
            Vector2 pos = CalculateItemPosition(i);
            activeItems[i].transform.localPosition = pos;
        }
    }
    
    private Vector2 CalculateItemPosition(int index)
    {
        float x = isVertical ? 0 : (index * (itemSize.x + spacing.x));
        float y = isVertical ? -(index * (itemSize.y + spacing.y)) : 0;
        return new Vector2(x, y);
    }
    
    private ListItemView CreateOrGetItem(int index)
    {
        ListItemView item;
        
        // 从对象池获取或创建新项
        if (pooledItems.Count > 0)
        {
            item = pooledItems.Dequeue();
            item.gameObject.SetActive(true);
        }
        else
        {
            item = Instantiate(itemPrefab, content);
            item.gameObject.name = $"Item_{index}";
            
            // 添加点击事件
            Button btn = item.GetComponent<Button>();
            if (btn == null) btn = item.gameObject.AddComponent<Button>();
            
            int itemIndex = index;
            btn.onClick.AddListener(() => OnItemClick(itemIndex));
            
            // 添加选择事件
            Toggle toggle = item.GetComponent<Toggle>();
            if (toggle != null)
            {
                toggle.onValueChanged.AddListener((isOn) => OnItemSelect(itemIndex, isOn));
            }
        }
        
        // 设置项大小
        RectTransform itemRect = item.GetComponent<RectTransform>();
        itemRect.sizeDelta = itemSize;
        
        activeItems[index] = item;
        return item;
    }
    
    private void OnScroll(Vector2 scrollPosition)
    {
        if (!isDragging)
        {
            UpdateVisibleItems();
        }
        else
        {
            // 拖拽时记录位置变化，用于拖拽结束时更新
            Vector2 currentPos = content.anchoredPosition;
            if (Vector2.Distance(currentPos, lastPosition) > 10f)
            {
                UpdateVisibleItems();
                lastPosition = currentPos;
            }
        }
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
        lastPosition = content.anchoredPosition;
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
        UpdateVisibleItems();
    }
    
    private void OnItemClick(int index)
    {
        if (index >= 0 && index < dataSource.Count)
        {
            OnItemClicked?.Invoke(index, dataSource[index]);
        }
    }
    
    private void OnItemSelect(int index, bool isSelected)
    {
        if (index >= 0 && index < dataSource.Count && isSelected)
        {
            OnItemSelected?.Invoke(index, dataSource[index]);
        }
    }
    
    // 滚动到指定位置
    public void ScrollToItem(int index)
    {
        if (index < 0 || index >= dataSource.Count) return;
        
        float itemDimension = isVertical ? itemSize.y : itemSize.x;
        float spacingDimension = isVertical ? spacing.y : spacing.x;
        float targetPos = index * (itemDimension + spacingDimension);
        
        if (isVertical)
        {
            content.anchoredPosition = new Vector2(content.anchoredPosition.x, targetPos);
        }
        else
        {
            content.anchoredPosition = new Vector2(-targetPos, content.anchoredPosition.y);
        }
        
        UpdateVisibleItems();
    }
}
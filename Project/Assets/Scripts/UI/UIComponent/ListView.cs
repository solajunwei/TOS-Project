using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// �б������ݽӿ�
public interface IListItemData
{
    int GetItemId(); // Ψһ��ʶ
}
// ʾ��������
public class ExampleItemData : IListItemData
{
    public int Id { get; set; }
    public int GetItemId() => Id;
}

// �б���ͼ���
[RequireComponent(typeof(ScrollRect))]
[RequireComponent(typeof(RectTransform))]
public class ListView : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    [Header("�������")]
    [SerializeField] private RectTransform viewport;
    [SerializeField] private RectTransform content;
    [SerializeField] private Scrollbar verticalScrollbar;
    [SerializeField] private Scrollbar horizontalScrollbar;
    
    [Header("���ò���")]
    [SerializeField] private ListItemView itemPrefab;
    [SerializeField] private Vector2 itemSize = new Vector2(100, 50);
    [SerializeField] private Vector2 spacing = Vector2.zero;
    [SerializeField] private int bufferItems = 3; // ��������С
    [SerializeField] private bool isVertical = true;
    
    // ���ݺ���ͼ����
    private List<IListItemData> dataSource = new List<IListItemData>();
    private Dictionary<int, ListItemView> activeItems = new Dictionary<int, ListItemView>();
    private Queue<ListItemView> pooledItems = new Queue<ListItemView>();
    
    // �����Ż�
    private float contentSize;
    private float viewportSize;
    private int visibleItemCount;
    private int startIndex = 0;
    private int endIndex = 0;
    private bool isDragging = false;
    private Vector2 lastPosition;
    
    // �¼�
    public Action<int, IListItemData> OnItemClicked;
    public Action<int, IListItemData> OnItemSelected;
    
    private ScrollRect scrollRect;
    private RectTransform rectTransform;
    
    private void Awake()
    {
        scrollRect = GetComponent<ScrollRect>();
        rectTransform = GetComponent<RectTransform>();
        
        // ����ScrollRect���
        scrollRect.content = content;
        scrollRect.vertical = isVertical;
        scrollRect.horizontal = !isVertical;
        
        if (verticalScrollbar) scrollRect.verticalScrollbar = verticalScrollbar;
        if (horizontalScrollbar) scrollRect.horizontalScrollbar = horizontalScrollbar;
        
        // ��ʼ�����ݲ���
        InitContentLayout();
        
        // ע������¼�
        scrollRect.onValueChanged.AddListener(OnScroll);
    }
    
    private void InitContentLayout()
    {
        // ���ݷ����������ݴ�С
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
    
    // ��������Դ
    public void SetDataSource(List<IListItemData> data)
    {
        ClearItems();
        dataSource = data ?? new List<IListItemData>();
        Refresh();
    }
    
    // ��ӵ���������
    public void AddItem(IListItemData item)
    {
        if (item == null) return;
        
        dataSource.Add(item);
        Refresh();
    }
    
    // ��ָ��λ�ò���������
    public void InsertItem(int index, IListItemData item)
    {
        if (item == null || index < 0 || index > dataSource.Count) return;
        
        dataSource.Insert(index, item);
        Refresh();
    }
    
    // �Ƴ�������
    public void RemoveItem(int index)
    {
        if (index < 0 || index >= dataSource.Count) return;
        
        dataSource.RemoveAt(index);
        Refresh();
    }
    
    // �������������
    public void ClearItems()
    {
        // �������л��
        foreach (var item in activeItems.Values)
        {
            item.gameObject.SetActive(false);
            pooledItems.Enqueue(item);
        }
        
        activeItems.Clear();
        dataSource.Clear();
    }
    
    // ˢ���б�
    public void Refresh()
    {
        // �������ݴ�С
        float totalSize = CalculateContentSize();
        content.sizeDelta = isVertical ? 
            new Vector2(content.sizeDelta.x, totalSize) : 
            new Vector2(totalSize, content.sizeDelta.y);
        
        // ��ȡ�ӿڴ�С
        viewportSize = isVertical ? viewport.rect.height : viewport.rect.width;
        
        // ����ɼ�������
        visibleItemCount = Mathf.FloorToInt(viewportSize / (isVertical ? itemSize.y : itemSize.x));
        
        // ���¿ɼ���
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
        
        // ��ȡ��ǰ����λ��
        float position = isVertical ? content.anchoredPosition.y : -content.anchoredPosition.x;
        
        // ������ʼ����
        float itemDimension = isVertical ? itemSize.y : itemSize.x;
        float spacingDimension = isVertical ? spacing.y : spacing.x;
        float totalItemHeight = itemDimension + spacingDimension;
        
        startIndex = Mathf.Max(0, Mathf.FloorToInt(position / totalItemHeight) - bufferItems);
        endIndex = Mathf.Min(dataSource.Count - 1, startIndex + visibleItemCount + (bufferItems * 2));
        
        // ���ղ��ɼ���
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
        
        // ��������¿ɼ���
        for (int i = startIndex; i <= endIndex; i++)
        {
            if (!activeItems.ContainsKey(i))
            {
                CreateOrGetItem(i);
            }
            
            // ����������
            activeItems[i].Initialize(i, dataSource[i]);
            
            // ������λ��
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
        
        // �Ӷ���ػ�ȡ�򴴽�����
        if (pooledItems.Count > 0)
        {
            item = pooledItems.Dequeue();
            item.gameObject.SetActive(true);
        }
        else
        {
            item = Instantiate(itemPrefab, content);
            item.gameObject.name = $"Item_{index}";
            
            // ��ӵ���¼�
            Button btn = item.GetComponent<Button>();
            if (btn == null) btn = item.gameObject.AddComponent<Button>();
            
            int itemIndex = index;
            btn.onClick.AddListener(() => OnItemClick(itemIndex));
            
            // ���ѡ���¼�
            Toggle toggle = item.GetComponent<Toggle>();
            if (toggle != null)
            {
                toggle.onValueChanged.AddListener((isOn) => OnItemSelect(itemIndex, isOn));
            }
        }
        
        // �������С
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
            // ��קʱ��¼λ�ñ仯��������ק����ʱ����
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
    
    // ������ָ��λ��
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
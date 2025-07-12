using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class ListViewExample : MonoBehaviour
{
    [SerializeField] private ListView listView;
    //[SerializeField] private InputField addInput;
    //[SerializeField] private Button addButton;
    //[SerializeField] private Button clearButton;
    
    private List<ExampleItemData> dataList = new List<ExampleItemData>();
    
    private void Start()
    {
        // 初始化数据
        InitializeData();
        
        // 设置列表数据
        listView.SetDataSource(dataList.Cast<IListItemData>().ToList());
        
        // 注册事件
        listView.OnItemClicked += OnItemClicked;
        //addButton.onClick.AddListener(AddItem);
        //clearButton.onClick.AddListener(ClearItems);
    }
    
    private void InitializeData()
    {
        for (int i = 0; i < 10; i++)
        {
            dataList.Add(new ExampleItemData
            {
                Id = i,
                Title = $"Item {i}",
                Description = $"This is description for item {i}"
            });
        }
    }
    
    private void OnItemClicked(int index, IListItemData data)
    {
        Debug.Log($"Clicked on item {index}: {((ExampleItemData)data).Title}");
        listView.ScrollToItem(index);
    }
    
    private void AddItem()
    {
        
        int newId = dataList.Count;

        string str = $"string1 {newId}";
        dataList.Add(new ExampleItemData
        {
            Id = newId,
            Title = str,
            Description = $"New item added at runtime {newId}"
        });
        
        listView.SetDataSource(dataList.Cast<IListItemData>().ToList());
    }
    
    private void ClearItems()
    {
        dataList.Clear();
        listView.ClearItems();
    }
}
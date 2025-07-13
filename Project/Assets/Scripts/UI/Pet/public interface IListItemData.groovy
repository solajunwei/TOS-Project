public interface IListItemData
{
    int GetItemId(); // 唯一标识
}

// 示例数据类
public class ExampleItemData : IListItemData
{
    public int Id { get; set; }
    public string Name{get;set}
    public int GetItemId() => Id;
}

public class PetInfo 
{
    public ExampleItemData petInfo;
    
    // 修正方法名，使用正确的接口方法
    public void show(IListItemData data)
    {
        Console.WriteLine("id = " + data.GetItemId()); // 修正方法调用和输出语句
    }
}

// 列表项视图基类
public class PetCell : ListItemView
{
    public PetCell() // 添加构造函数
    {
        PetInfo pt = new PetInfo();
        ExampleItemData pe = new ExampleItemData { Id = 123 }; // 初始化Id值
        pt.show(pe); // 在构造函数中调用show方法
    }
}
public interface IListItemData
{
    int GetItemId(); // Ψһ��ʶ
}

// ʾ��������
public class ExampleItemData : IListItemData
{
    public int Id { get; set; }
    public string Name{get;set}
    public int GetItemId() => Id;
}

public class PetInfo 
{
    public ExampleItemData petInfo;
    
    // ������������ʹ����ȷ�Ľӿڷ���
    public void show(IListItemData data)
    {
        Console.WriteLine("id = " + data.GetItemId()); // �����������ú�������
    }
}

// �б�����ͼ����
public class PetCell : ListItemView
{
    public PetCell() // ��ӹ��캯��
    {
        PetInfo pt = new PetInfo();
        ExampleItemData pe = new ExampleItemData { Id = 123 }; // ��ʼ��Idֵ
        pt.show(pe); // �ڹ��캯���е���show����
    }
}

using cfg;
using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class PetInfo : ExampleItemData
{
    public pet petInfo;
    public Pet Delegate;
}

// �б�����ͼ����
public class PetCell : ListItemView
{

    public Text _petName;
    private PetInfo _dataInfo;

    public override void Initialize(int index, IListItemData data)
    {
        //base.Initialize(index, data);
        UpdateView(data);
    }

    public void UpdateView(IListItemData data)
    {
        if(null != data)
        {
            _dataInfo = data as PetInfo;
        }
        
        _petName.text = _dataInfo.petInfo.Name;
    }

    public void onClickShowInfo()
    {
        if (null != _dataInfo)
        {
            Pet petInfo = _dataInfo.Delegate;
            if (null != petInfo)
            {
                // ��Ҫ����Ƿ���ں���
                petInfo.setPetId(_dataInfo.petInfo);
            }
            
        }
        Debug.Log("sssssssssssssssss");
    }
}

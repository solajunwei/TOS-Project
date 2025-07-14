
using cfg;
using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;


// 列表项视图基类
public class PetCell : ListItemView
{

    public Text _petName;
    private PetInfo _dataInfo;

    public override void Initialize(int index, IListItemData data)
    {
        base.Initialize(index, data);
    }

    protected override void UpdateView(IListItemData data)
    {
        if(data is PetInfo petinfo)
        {
            _dataInfo = petinfo;
            _petName.text = _dataInfo.petInfo.Name;
        }
        else
        {
            Debug.LogError($"PetCell Invalid data type. Expected PetInfo, got {data?.GetType().Name ?? "null"}");
        }
    }

    public void onClickShowInfo()
    {
        if (null != _dataInfo)
        {
            Pet petInfo = _dataInfo.Delegate;
            if (null != petInfo)
            {
                // 需要检查是否存在函数
                petInfo.setPetId(_dataInfo);
            }
            
        }
    }
}

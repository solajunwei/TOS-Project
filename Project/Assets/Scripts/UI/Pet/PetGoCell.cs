using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PetGoCell : ListItemView
{
    private PetInfo _dataInfo;

    [SerializeField]
    private Text _name;

    public override void Initialize(int index, IListItemData data)
    {
        base.Initialize(index, data);
    }

    protected override void UpdateView(IListItemData data)
    {
        if(data is PetInfo petinfo)
        {
            _dataInfo = petinfo;
            _name.text = _dataInfo.petInfo.Name;
        }
        else
        {
            Debug.LogError($" PetGoCell Invalid data type. Expected PetInfo, got {data?.GetType().Name ?? "null"}");
        }
    }
}

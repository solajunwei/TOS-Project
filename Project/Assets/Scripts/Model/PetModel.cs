using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using cfg;
using Unity.VisualScripting;
using UnityEngine;

namespace Pet11
{
    public class PetInfo
    {
        public int PetId;  // 宠物id
        public float _fDistance = 3f; // 宠物的攻击距离
        public petConfig BaseInfo;  // 宠物的基本信息
    }
}


public class PetModel : BaseManager<PetModel>
{
    private List<petConfig> _onBattlePet = new List<petConfig>();

    public void onClearPet()
    {
        _onBattlePet.Clear();
    }

    /// <summary>
    /// 获取上阵宠物的所有数据
    /// </summary>
    /// <returns></returns>
    public List<petConfig> GetOnBattlePet()
    {
        if (null == _onBattlePet)
        {
            return new List<petConfig>();
        }

        return _onBattlePet;
    }

    /// <summary>
    /// 通过 index 获取上阵宠物信息
    /// </summary>
    /// <param name="index">在上阵阵容第几个</param>
    /// <returns></returns>
    public petConfig GetOnBattlePetByIndex(int index)
    {
        if (null == _onBattlePet || index >= _onBattlePet.Count)
        {
            return null;
        }

        return _onBattlePet[index];
    }


    /// <summary>
    /// 添加上阵宠物
    /// </summary>
    /// <param name="petInfo"></param>
    public void AddOnBattlePet(petConfig petInfo)
    {
        if (null == _onBattlePet)
        {
            _onBattlePet = new List<petConfig>();
        }
        AddOnBattlePet(petInfo, _onBattlePet.Count);
    }

    /// <summary>
    /// 添加上阵宠物
    /// </summary>
    /// <param name="petInfo">上阵的宠物</param>
    /// <param name="index">宠物应该放在那个位置</param>
    public void AddOnBattlePet(petConfig petInfo, int index)
    {
        if (null == petInfo)
        {
            return;
        }
        _onBattlePet.Insert(index, petInfo);
    }



}

using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using cfg;
using Unity.VisualScripting;
using UnityEngine;


public class PetModel : BaseManager<PetModel>
{
    private List<pet> _onBattlePet = new List<pet>();

    public void onClearPet()
    {
        _onBattlePet.Clear();
    }

    /// <summary>
    /// ��ȡ����������������
    /// </summary>
    /// <returns></returns>
    public List<pet> GetOnBattlePet()
    {
        if (null == _onBattlePet)
        {
            return new List<pet>();
        }

        return _onBattlePet;
    }

    /// <summary>
    /// ͨ�� index ��ȡ���������Ϣ
    /// </summary>
    /// <param name="index">���������ݵڼ���</param>
    /// <returns></returns>
    public pet GetOnBattlePetByIndex(int index)
    {
        if (null == _onBattlePet || index >= _onBattlePet.Count)
        {
            return null;
        }

        return _onBattlePet[index];
    }


    /// <summary>
    /// ����������
    /// </summary>
    /// <param name="petInfo"></param>
    public void AddOnBattlePet(pet petInfo)
    {
        if (null == _onBattlePet)
        {
            _onBattlePet = new List<pet>();
        }
        AddOnBattlePet(petInfo, _onBattlePet.Count);
    }

    /// <summary>
    /// ����������
    /// </summary>
    /// <param name="petInfo">����ĳ���</param>
    /// <param name="index">����Ӧ�÷����Ǹ�λ��</param>
    public void AddOnBattlePet(pet petInfo, int index)
    {
        if (null == petInfo)
        {
            return;
        }
        _onBattlePet.Insert(index, petInfo);
    }



}

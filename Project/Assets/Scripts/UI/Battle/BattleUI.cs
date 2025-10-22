using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

class DrawPetData
{
    public int petId;

}

public class BattleUI : UIComponent
{

    public Image[] _iamges;
    public GameObject _prefab;

    // 轮数
    public Text _RoundText;
    // 金币
    public Text _PointText;
    // 血量
    public Text _BloodText;

    // 默认是第一轮
    private int _round = 1;

    // 血量默认20
    private int _blood = 20;

    // 金币默认为0
    private int _point = 0;

    private Dictionary<int, int> _drawPetDic = new Dictionary<int, int>();

    public void Start()
    {
        int roundCount = BattleModel.Instance.getRoundCount();
        _RoundText.text = _round + "/" + roundCount;
        _PointText.text = _point.ToString();
        _BloodText.text = _blood.ToString();

        EventManager.Instance.AddEventListener(MyConstants.jump_next_round, updateNextRound);
        EventManager.Instance.AddEventListener(MyConstants.home_attack, updateLeft);
        EventManager.Instance.AddEventListener<GameObject>(MyConstants.Enemy_deal, EnemyDeal);
    }
    public void OnDestroy()
    {
        EventManager.Instance.RemoveEventListener(MyConstants.jump_next_round, updateNextRound);
        EventManager.Instance.RemoveEventListener(MyConstants.home_attack, updateLeft);
        EventManager.Instance.RemoveEventListener<GameObject>(MyConstants.Enemy_deal, EnemyDeal);
    }

    // 改变轮数
    public void updateNextRound()
    {
        _round++;
        int roundCount = BattleModel.Instance.getRoundCount();
        _RoundText.text = _round + "/" + roundCount;
    }

    // 更改生命值
    public void updateLeft()
    {
        _blood--;
        _BloodText.text = _blood.ToString();
    }

    // 获取金币
    public void EnemyDeal(GameObject obj)
    {
        EnemyUnit enemyUnit = obj.GetComponent<EnemyUnit>();
        _point += enemyUnit.PointNum;
        _PointText.text = _point.ToString();
    }

    // 抽卡
    public void Draw()
    {

        int index = getInstantiateIndex();
        if (index == -1)
        {
            Debug.LogError("格子已满，无法抽取");
            return;
        }

        if (index < 0 && index > 10)
        {
            Debug.LogError("获取参数错误");
            return;
        }

        Image image = _iamges[index];
        GameObject obj = Instantiate(_prefab, image.transform);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;
        obj.transform.localScale = Vector3.one;

        int petId = 11001;
        PetUnit unit = obj.GetComponent<PetUnit>();
        unit.LoadAndReplaceImage(petId);
        if (checkCanMerge(petId))
        {
            return;
        }
        _drawPetDic[index] = petId;
    }

    // 获取抽取出来的宠物放在的位置
    private int getInstantiateIndex()
    {
        for (int i = 0; i < 10; i++)
        {
            if (!_drawPetDic.ContainsKey(i))
            {
                return i;
            }
        }

        return -1;
    }
    
    // 判断宠物是否可以合并
    private bool checkCanMerge(int petId)
    {
        return false;
    }

    public void OnClickSkill(int skillId)
    {

    }
}

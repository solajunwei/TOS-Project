using DG.Tweening;
using UnityEngine;

public class EnemyUnit : Unit
{
    // 自身拥有的金币
    public int PointNum
    {
        get { return _point; }
    }

    protected override void OnDestroy()
    {
        gameObject.SetActive(false);
        gameObject.transform.DOKill();
        Destroy(gameObject, 0.01f);
    }

        /// <summary>
    /// 受到攻击
    /// </summary>
    /// <param name="attack">伤害数值</param>
    public void UnderAttack(int attack)
    {
        if (_HP <= 0)
        {
            return;
        }

        _HP -= attack;
        if (_HP <= 0)
        {
            _HP = 0;
            EventManager.Instance.EventTrigger<GameObject>(MyConstants.Enemy_deal, gameObject);
            EventManager.Instance.EventTrigger<int>(MyConstants.add_Point, _point);
        }
        _scrollBar.SetProgress((float)_HP / _HPMax);
    }
}

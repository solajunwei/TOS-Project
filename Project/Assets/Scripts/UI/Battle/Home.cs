using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Home : MonoBehaviour
{
    public ZProgress _scrollBar;
    private int _HP = 100;
    private int _HPMax = 100;

    void Start()
    {
        _scrollBar.SetProgress(1);
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.CompareTag("Enemy"))
        {
            UnderAttack(10);
            EventManager.Instance.EventTrigger<GameObject>(MyConstants.Enemy_deal, other.gameObject);
        }
    }

    /// <summary>
    /// 受到攻击
    /// </summary>
    /// <param name="attack">伤害数值</param>
    public void UnderAttack(int attack)
    {
        EventManager.Instance.EventTrigger(MyConstants.home_attack);
        _HP -= attack;
        if (_HP <= 0)
        {
            _HP = 0;
            Debug.Log("死亡了");
            return;
        }
        _scrollBar.SetProgress((float)_HP / _HPMax);
    }
}

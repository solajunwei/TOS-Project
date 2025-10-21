using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleModel : BaseManager<BattleModel>
{
    /// <summary>
    /// 敌方活着的单位数量
    /// </summary>
    private List<GameObject> _EnemyList = new List<GameObject>();
    public List<GameObject> EnemyList
    {
        get { return _EnemyList; }
    }

    private List<GameObject> _HeroList = new List<GameObject>();

    /// <summary>
    /// 一场最多几个敌人
    /// </summary>
    private int _enemyNumMax = 2;
    public int EnemyNumMax
    {
        get { return _enemyNumMax; }
    }

    /// <summary>
    /// 攻击距离最远
    /// </summary>
    private float _fDistance = 3f;
    public float FDistance
    {
        get { return _fDistance; }
    }


    // 敌方单位是否已经加入最大
    public bool IsEnemyMax()
    {
        return _enemyNumMax > _EnemyList.Count;
    }

    // 添加敌方单位
    public void addEnemyList(GameObject addEnemy)
    {
        _EnemyList.Add(addEnemy);
    }

    /// <summary>
    /// 删除一个单位
    /// </summary>
    /// <returns></returns>
    public void removeEnemyList(GameObject enemy)
    {
        _EnemyList.Remove(enemy);
    }

    // 获取第一个单位
    public GameObject getFirstEnemy()
    {
        if(IsEnemyEmpty())
        {
            return null;
        }
        return _EnemyList[0];
    }

    //敌方单位是否为空
    public bool IsEnemyEmpty()
    {
        return _EnemyList.Count == 0;
    }

    // 获取攻击的最先一个
    public GameObject getFirstAttackEnemy(GameObject heroObj)
    {
        foreach(GameObject enemy in _EnemyList) 
        {
            float tmpDis = Vector3.Distance(heroObj.transform.position, enemy.transform.position);
            Debug.Log("tempDis == " + tmpDis + " enemyPosition === " + enemy.transform.position);
            if(_fDistance >= tmpDis)
            {
                return enemy;
            }
        }
        
        return null;
    }

    private void addHeroList(GameObject addHero)
    {
        _HeroList.Add(addHero);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BattleModel : BaseManager<BattleModel>
{
    // 一关有多少回合
    private List<int> _roundGroupList = new List<int>
    {
        2, 4, 6,8
    };

    // 多少个英雄单位
    private List<GameObject> _PlayerList = new List<GameObject>();
    public List<GameObject> PlayerList
    {
        get { return _PlayerList; }
    }

    /// <summary>
    /// 当前回合总共出现了多少个敌方单位
    /// </summary>
    private int _curRoundEnemyMax = 0;
    public int CurRoundEnemyMax
    {
        get { return _curRoundEnemyMax; }
        set { _curRoundEnemyMax = value; }
    }

    /// <summary>
    /// 敌方活着的单位数量
    /// </summary>
    private List<GameObject> _EnemyList = new List<GameObject>();
    public List<GameObject> EnemyList
    {
        get { return _EnemyList; }
    }


    /// <summary>
    /// 攻击距离最远
    /// </summary>
    private float _fDistance = 3f;
    public float FDistance
    {
        get { return _fDistance; }
    }


    /// <summary>
    /// 当前轮是否已经添加敌人已经达到最大值
    /// </summary>
    /// <param name="round"></param>
    /// <returns></returns>
    public bool IsEnemyMax(int round)
    {
        if (round > _roundGroupList.Count)
        {
            return false;
        }

        return _curRoundEnemyMax >= _roundGroupList[round];
    }

    // 添加敌方单位
    public void addEnemyList(GameObject addEnemy)
    {
        _curRoundEnemyMax++;
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

    /// <summary>
    /// 添加我方单位
    /// </summary>
    /// <param name="player"></param>
    public void addPlayerUnit(GameObject player)
    {
        PlayerList.Add(player);
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
        Vector3 heroPositoion = new Vector3(heroObj.transform.position.x, heroObj.transform.position.y, 0);
        foreach(GameObject enemy in _EnemyList)
        {
            Vector3 enemyPositoion = new Vector3(enemy.transform.position.x, enemy.transform.position.y, 0);
            float tmpDis = Vector3.Distance(heroPositoion, enemyPositoion);
            if(_fDistance >= tmpDis)
            {
                return enemy;
            }
        }
        
        return null;
    }

    /// <summary>
    /// 获取当前场战斗总共多少轮
    /// </summary>
    /// <returns></returns>
    public int getRoundCount()
    {
        return _roundGroupList.Count;
    }
}

using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using Battle;

public class BattleManager : MonoBehaviour
{
    public GameObject HeroUnit;
    public GameObject EnemyUnit;
    public Camera _camera;

    /// <summary>
    /// 敌方按照动画运动的总时长
    /// </summary>
    public float fAnimTime = 60f;

    private GameObject heroObj;

    private Vector3[] path;

    BattleInfo battlelInfo = null;

    private void Start()
    {
        UIManager.Instance.OpenView<BattleUI>("Perfabs/Battle/BattleBtns", E_UI_Layer.Mit);
        battlelInfo = BattleModel.Instance.CurBattleInfo;

        path = new Vector3[]
        {
            new Vector3(-9.5f, 0.4f, 10),
            new Vector3(-8.5f, 0.4f, 10),
            new Vector3(-7.5f, 0.8f, 10),
            new Vector3(-6f, 0, 10),
            new Vector3(-5f, 0.5f, 10),
            new Vector3(-3.5f, -0.2f, 10),
            new Vector3(-1.5f, 0.7f, 10),
            new Vector3(0, 0, 10),
            new Vector3(5f, 2.5f, 10),
            new Vector3(7.5f, 1.2f, 10)
        };

        EventManager.Instance.AddEventListener<GameObject>(MyConstants.Enemy_deal, EnemyDeal);
        EventManager.Instance.AddEventListener<Vector3>(MyConstants.create_unit, CreateUnit);
        EventManager.Instance.AddEventListener(MyConstants.jump_next_round, updateNextRound);
        EventManager.Instance.AddEventListener(MyConstants.gameoverLevel, gameover);
        EventManager.Instance.AddEventListener(MyConstants.start_game_run, OnStartGame);

        
    }

    private void OnDestroy()
    {
        EventManager.Instance.RemoveEventListener<GameObject>(MyConstants.Enemy_deal, EnemyDeal);
        EventManager.Instance.RemoveEventListener<Vector3>(MyConstants.create_unit, CreateUnit);
        EventManager.Instance.RemoveEventListener(MyConstants.jump_next_round, updateNextRound);
        EventManager.Instance.RemoveEventListener(MyConstants.gameoverLevel, gameover);
        EventManager.Instance.RemoveEventListener(MyConstants.start_game_run, OnStartGame);
    }

    public void gameover()
    {
        battlelInfo.setGameOver(true);
        // 展示结算
        UIManager.Instance.OpenView<GameResult>("Perfabs/Battle/GameResult", E_UI_Layer.Mit);
    }

    // 跳转到下一轮
    public void updateNextRound()
    {
        if (battlelInfo.isGameOver())
        {
            return;
        }

        // 开始创建新的敌方单位
        startCreateEnemy();
    }

    public void EnemyDeal(GameObject obj)
    {
        if (battlelInfo.isGameOver()) return;
        battlelInfo.removeEnemy(obj);
        Destroy(obj, 0.01f);
    }

    // 开始游戏
    public void OnStartGame()
    {
        CreateHero();
    }

    private void CreateHero()
    {
        GameObject UnitObj = Resources.Load<GameObject>("UI/Perfabs/Battle/HeroUnit");
        if (null == UnitObj)
        {
            Debug.LogError("Resources load Battle Unit");
            return;
        }
        heroObj = Instantiate(UnitObj, HeroUnit.gameObject.transform.position, Quaternion.identity);
        battlelInfo.addPlayer(heroObj);
        startCreateEnemy();
    }

    /// <summary>
    /// 开始创建敌方单位
    /// </summary>
    public void startCreateEnemy()
    {
        if (battlelInfo.isGameOver()) return;
        // 延迟5秒后开始创建敌方单位，每秒创建一个
        InvokeRepeating("CountDown", 5f, 2f);
    }
    
    public void CountDown()
    {
        // 当前轮敌方单位是否已经达到上线
        if (battlelInfo.isGameOver() || battlelInfo.IsRoundEnemyMax())
        {
            // 停止定时器添加单位
            CancelInvoke("CountDown");
            return;
        }

        // 创建敌方坦克
        CreateEnemy();
    }

    private void CreateEnemy()
    {
        if (battlelInfo.isGameOver()) return;
        GameObject UnitObj = Resources.Load<GameObject>("UI/Perfabs/Battle/EnemyUnit");
        if (null == UnitObj)
        {
            Debug.LogError("Resources load Battle Unit");
            return;
        }
        GameObject emenyObj = Instantiate(UnitObj, EnemyUnit.gameObject.transform.position, Quaternion.identity);
        battlelInfo.addEnemy(emenyObj);
        RunEnemyPosition(emenyObj);
    }
    

    private void RunEnemyPosition(GameObject enemy)
    {
        Transform target = enemy.transform;
        target.DOPath(path, fAnimTime, PathType.Linear);
    }

    private void Update()
    {
        if (battlelInfo.isGameOver()) return;
        // 是否存在敌方单位
        if (!battlelInfo.isEnemyEmpty())
        {
            List<GameObject> list = battlelInfo.PlayerList;
            foreach(GameObject obj in list)
            {
                GameObject otherObj = battlelInfo.getFirstAttackEnemy(obj);
                if (null != otherObj) // 确保有敌人在攻击范围内
                {
                    HeroUnit herounit = obj.GetComponent<HeroUnit>();
                    if (!herounit.IsWaiting) // 不在攻击冷却中
                    {
                        herounit.IsWaiting = true;
                        GameObject bulletObj = Resources.Load<GameObject>("UI/Perfabs/Battle/Buttle");
                        GameObject killObj = Instantiate(bulletObj, obj.gameObject.transform.position, Quaternion.identity);
                        int playerLayerIndex = LayerMask.NameToLayer("Player");
                        if (playerLayerIndex != -1)
                        {
                            killObj.layer = playerLayerIndex;
                        }
                        Bullet bullet = killObj.GetComponent<Bullet>();
                        if (bullet)
                        {
                            bullet.TargetCoordinates = otherObj;
                        }
                    }
                }
            }
        }
    }

    private void CreateUnit(Vector3 vecPos)
    {
        if (battlelInfo.isGameOver()) return;
        GameObject obj = Resources.Load<GameObject>("UI/Perfabs/Battle/HeroUnit");
        if (null == obj)
        {
            Debug.LogError("Resources load Battle Unit");
            return;
        }
        Vector3 worldPoint = _camera.ScreenToWorldPoint(vecPos);
        worldPoint.z = 10;
        GameObject UnitObj = Instantiate(obj, worldPoint, Quaternion.identity);
        battlelInfo.addPlayer(UnitObj);
    }
}

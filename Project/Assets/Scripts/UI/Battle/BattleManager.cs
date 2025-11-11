using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using DG.Tweening;
using UnityEditor;
using Unity.VisualScripting;

public class BattleManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject HeroUnit;
    public GameObject EnemyUnit;
    public Camera _camera;

    /// <summary>
    /// 敌方按照动画运动的总时长
    /// </summary>
    public float fAnimTime = 60f;

    private GameObject heroObj;
    private GameObject _moveObj;

    private Vector3[] path;

    /// <summary>
    /// 当前是第几轮
    /// </summary>
    private int _curRound = 1;

    Vector3 lastMousePosition;

    private void Start()
    {
        UIManager.Instance.ShowPanel<BattleUI>("Perfabs/Battle/BattleBtns", E_UI_Layer.Mit);
        CreateHero();
        lastMousePosition = Input.mousePosition;

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

    }

    private void OnDestroy()
    {
        EventManager.Instance.RemoveEventListener<GameObject>(MyConstants.Enemy_deal, EnemyDeal);
        EventManager.Instance.RemoveEventListener<Vector3>(MyConstants.create_unit, CreateUnit);
        EventManager.Instance.RemoveEventListener(MyConstants.jump_next_round, updateNextRound);
    }

    // 跳转到下一轮
    public void updateNextRound()
    {
        _curRound++;
        int roundCount = BattleModel.Instance.getRoundCount();

        // 如果大于当前最大一轮，则当前战斗胜利
        if (_curRound > roundCount)
        {
            // 战斗胜利
            Debug.Log("战斗胜利");
            return;
        }

        // 重置当前轮的数值
        BattleModel.Instance.CurRoundEnemyMax = 0;

        // 开始创建新的敌方单位
        startCreateEnemy();
    }

    public void EnemyDeal(GameObject obj)
    {
        foreach (GameObject objTet in BattleModel.Instance.EnemyList)
        {
            if (obj == objTet)
            {
                Destroy(objTet, 0.01f);
                BattleModel.Instance.removeEnemyList(objTet);
                break;
            }
        }

        // 如果当前轮的所有敌方单位全部死亡，切换下一轮
        if(BattleModel.Instance.IsEnemyEmpty())
        {
            EventManager.Instance.EventTrigger(MyConstants.jump_next_round);
        }
    }

    private void CreateHero()
    {
        Debug.Log("BattleManager_CreateHero");
        GameObject UnitObj = Resources.Load<GameObject>("UI/Perfabs/Battle/Unit");
        if (null == UnitObj)
        {
            Debug.LogError("Resources load Battle Unit");
            return;
        }
        heroObj = Instantiate(UnitObj, HeroUnit.gameObject.transform.position, Quaternion.identity);
        heroObj.tag = "Player";
        int playerLayerIndex = LayerMask.NameToLayer("Player");
        if (playerLayerIndex != -1)
        {
            heroObj.layer = playerLayerIndex;
        }

        Unit unit = heroObj.GetComponent<Unit>();
        ZProgress pro = unit._scrollBar;
        Destroy(unit);
        HeroUnit heroUnit = heroObj.AddComponent<HeroUnit>();
        heroUnit._scrollBar = pro;
        BattleModel.Instance.addPlayerUnit(heroObj);
        startCreateEnemy();
    }

    /// <summary>
    /// 开始创建敌方单位
    /// </summary>
    public void startCreateEnemy()
    {
        // 延迟5秒后开始创建敌方单位，每秒创建一个
        InvokeRepeating("CountDown", 5f, 2f);
    }
    
    public void CountDown()
    {
        // 当前轮敌方单位是否已经达到上线
        if (BattleModel.Instance.IsEnemyMax(_curRound))
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
        Debug.Log("BattleManager_CreateEnemy");
        GameObject UnitObj = Resources.Load<GameObject>("UI/Perfabs/Battle/Unit");
        if (null == UnitObj)
        {
            Debug.LogError("Resources load Battle Unit");
            return;
        }
        GameObject emenyObj = Instantiate(UnitObj, EnemyUnit.gameObject.transform.position, Quaternion.identity);
        Unit unit = emenyObj.GetComponent<Unit>();
        ZProgress pro = unit._scrollBar;
        Destroy(unit);
        EnemyUnit enemyUnit = emenyObj.AddComponent<EnemyUnit>();
        enemyUnit._scrollBar = pro;
        int playerLayerIndex = LayerMask.NameToLayer("Player");
        if(playerLayerIndex != -1)
        {
            emenyObj.layer = playerLayerIndex;
        }

        BattleModel.Instance.addEnemyList(emenyObj);
        RunEnemyPosition(emenyObj);
    }
    

    private void RunEnemyPosition(GameObject enemy)
    {
        Transform target = enemy.transform;
        target.DOPath(path, fAnimTime, PathType.Linear);
    }

    private void Update()
    {
        // 是否存在敌方单位
        if (!BattleModel.Instance.IsEnemyEmpty())
        {
            List<GameObject> list = BattleModel.Instance.PlayerList;
            foreach(GameObject unit in list)
            {
                GameObject otherObj = BattleModel.Instance.getFirstAttackEnemy(unit);
                if (null != otherObj) // 确保有敌人在攻击范围内
                {
                    HeroUnit herounit = unit.GetComponent<HeroUnit>();
                    if (!herounit.IsWaiting) // 不在攻击冷却中
                    {
                        herounit.IsWaiting = true;
                        GameObject bulletObj = Resources.Load<GameObject>("UI/Perfabs/Battle/Buttle");
                        GameObject killObj = Instantiate(bulletObj, unit.gameObject.transform.position, Quaternion.identity);
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
        GameObject obj = Resources.Load<GameObject>("UI/Perfabs/Battle/Unit");
        if (null == obj)
        {
            Debug.LogError("Resources load Battle Unit");
            return;
        }
        Vector3 worldPoint = _camera.ScreenToWorldPoint(vecPos);
        worldPoint.z = 10;
        GameObject UnitObj = Instantiate(obj, worldPoint, Quaternion.identity);

        UnitObj.tag = "Player";
        int playerLayerIndex = LayerMask.NameToLayer("Player");
        if (playerLayerIndex != -1)
        {
            UnitObj.layer = playerLayerIndex;
        }

        Unit unit = UnitObj.GetComponent<Unit>();
        ZProgress pro = unit._scrollBar;
        Destroy(unit);
        HeroUnit heroUnit = UnitObj.AddComponent<HeroUnit>();
        heroUnit._scrollBar = pro;



        BattleModel.Instance.addPlayerUnit(UnitObj);
    }
}

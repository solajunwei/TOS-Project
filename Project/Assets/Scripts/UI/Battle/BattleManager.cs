using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using DG.Tweening;
using UnityEditor;

public class BattleManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject HeroUnit;
    public GameObject EnemyUnit;

    /// <summary>
    /// 敌方按照动画运动的总时长
    /// </summary>
    public float fAnimTime = 60f;

    private GameObject heroObj;

    private Vector3[] path;

    private float countdownTime = 3f;
    private float countTime = 0f;

    private void Start()
    {
        UIManager.Instance.ShowPanel<Setting>("Perfabs/Battle/BattleBtns", E_UI_Layer.Mit);
        CreateHero();

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
            new Vector3(9f, 0.5f, 10),
            new Vector3(10f, 1.5f, 10)
        };

        EventManager.Instance.AddEventListener<GameObject>(MyConstants.Enemy_deal, EnemyDeal);
        EventManager.Instance.AddEventListener<GameObject>(MyConstants.hero_bullet, HeroBullet);
    }

    private void OnDestroy()
    {
        EventManager.Instance.RemoveEventListener<GameObject>(MyConstants.Enemy_deal, EnemyDeal);
        EventManager.Instance.RemoveEventListener<GameObject>(MyConstants.hero_bullet, HeroBullet);
    }

    public void EnemyDeal(GameObject obj)
    {
        foreach (GameObject objTet in BattleModel.Instance.EnemyList)
        {
            if (obj == objTet)
            {
                Destroy(objTet);
                BattleModel.Instance.removeEnemyList(objTet);
                break;
            }
        }
    }

    // 创建子弹
    public void HeroBullet(GameObject obj)
    {

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
        Unit unit = heroObj.GetComponent<Unit>();
        ZProgress pro = unit._scrollBar;
        Destroy(unit);
        HeroUnit heroUnit =heroObj.AddComponent<HeroUnit>();
        heroUnit._scrollBar = pro;
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
        enemyUnit._scrollBar= pro;

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
        // 创建敌方单位
        if (BattleModel.Instance.IsEnemyMax())
        {
            countTime += Time.deltaTime;
            if (countTime >= countdownTime)
            {
                CreateEnemy();
                countTime = 0f;
            }
        }


        // 创建子弹
        if (!BattleModel.Instance.IsEnemyEmpty())
        {
            HeroUnit herounit = heroObj.GetComponent<HeroUnit>();
            if(!herounit.IsWaiting)
            {
                herounit.IsWaiting = true;
                GameObject bulletObj = Resources.Load<GameObject>("UI/Perfabs/Battle/Buttle");
                GameObject killObj = Instantiate(bulletObj, HeroUnit.gameObject.transform.position, Quaternion.identity);
                Bullet bullet = killObj.GetComponent<Bullet>();
                if(bullet){
                    GameObject obj = BattleModel.Instance.getFirstAttackEnemy(heroObj);
                    bullet.TargetCoordinates = obj;
                }
            }
        }

        // 测试按钮
        if (Input.GetKeyDown(KeyCode.J))
        {
            GameObject bulletObj = Resources.Load<GameObject>("UI/Perfabs/Battle/Buttle");
            GameObject killObj = Instantiate(bulletObj, HeroUnit.gameObject.transform.position, Quaternion.identity);
            Bullet bullet = killObj.GetComponent<Bullet>();
            if (bullet)
            {
                GameObject obj = BattleModel.Instance.getFirstAttackEnemy(heroObj);
                bullet.TargetCoordinates = obj;
            }
        }
    }
}

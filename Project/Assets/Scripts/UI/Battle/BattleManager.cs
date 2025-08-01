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

    public GameObject UnitPrefab;
    public GameObject UnitKillPrefab;

    public float fDistance = 3f;
    //public float fAnimTime = 30f;
    public float fAnimTime = 60f;

    public float fSpeed = 1.0f;

    private float fSkillTime = 0.2f;
    private float timer = 0f;

    private GameObject heroObj;
    private GameObject emenyObj;
    private GameObject killObj;

    private Vector3[] path;

    private void Start()
    {
        heroObj = Instantiate(UnitPrefab, HeroUnit.gameObject.transform.position, Quaternion.identity);

        path = new Vector3[]
        {
            new Vector3(-9.5f, 0.4f, 0),
            new Vector3(-8.5f, 0.4f, 0),
            new Vector3(-7.5f, 0.8f, 0),
            new Vector3(-6f, 0, 0),
            new Vector3(-5f, 0.5f, 0),
            new Vector3(-3.5f, -0.2f, 0),
            new Vector3(-1.5f, 0.7f, 0),
            new Vector3(0, 0, 0),
            new Vector3(5f, 2.5f, 0),
            new Vector3(9f, 0.5f, 0),
            new Vector3(10f, 1.5f, 0)
        };

        emenyObj = Instantiate(UnitPrefab, EnemyUnit.gameObject.transform.position, Quaternion.identity);
        RunEnemyPosition(emenyObj);
    }

    private void RunEnemyPosition(GameObject enemy)
    {
        Transform target = enemy.transform;
        target.DOPath(path, fAnimTime, PathType.Linear);
    }

    private void Update()
    {
        timer += Time.deltaTime;
        float tmpDis = Vector3.Distance(heroObj.transform.position, emenyObj.transform.position);
        if (tmpDis < fDistance)
        {
            if (timer > fSkillTime)
            {
                timer = 0;
                createBullet();
            }
        }
    }

    //´´½¨×Óµ¯
    void createBullet()
    {
        killObj = Instantiate(UnitKillPrefab, HeroUnit.gameObject.transform.position, Quaternion.identity);
        Bullet bullet = killObj.GetComponent<Bullet>();
        if (bullet != null)
        {
            bullet.TargetForm = emenyObj.transform;
            bullet.createAinm();
        }
    }
}

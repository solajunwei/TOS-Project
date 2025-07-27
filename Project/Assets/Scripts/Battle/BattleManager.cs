using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using DG.Tweening;

public class BattleManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject HeroUnit;
    public GameObject EnemyUnit;
    public GameObject UnitPrefab;

    public float fDistance = 2.5f;

    private GameObject heroObj;
    private GameObject emenyObj;

    List<Vector3> vecList;
    private Vector3[] path;

    private void Start()
    {
        vecList = new List<Vector3>();
        heroObj = Instantiate(UnitPrefab, HeroUnit.gameObject.transform.position, Quaternion.identity);

        vecList.Add(new Vector3(-9.5f, 0.4f, 0));
        vecList.Add(new Vector3(-8.5f, 0.4f, 0));
        vecList.Add(new Vector3(-7.5f, 0.8f, 0));
        vecList.Add(new Vector3(-6f, 0, 0));
        vecList.Add(new Vector3(-5f, 0.5f, 0));
        vecList.Add(new Vector3(-3.5f, -0.2f, 0));
        vecList.Add(new Vector3(-1.5f, 0.7f, 0));
        vecList.Add(new Vector3(0, 0, 0));
        vecList.Add(new Vector3(5f, 2.5f, 0));
        vecList.Add(new Vector3(9f, 0.5f, 0));
        vecList.Add(new Vector3(10f, 1.5f, 0));

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
        target.DOPath(path, 30f, PathType.Linear);
    }

    private void Update()
    {
        float tmpDis = Vector3.Distance(heroObj.transform.position, emenyObj.transform.position);
        if (tmpDis < fDistance)
        {
            Debug.Log("tmpDis ==== " + tmpDis);
        }
    }

}

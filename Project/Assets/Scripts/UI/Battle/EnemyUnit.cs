using cfg;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUnit : Unit
{

    // 自身拥有的金币
    private int _PointNum = 10;
    public int PointNum
    {
        get { return _PointNum; }
    }

    protected override void OnDestroy()
    {
        gameObject.SetActive(false);
        gameObject.transform.DOKill();
        Destroy(gameObject, 0.01f);
    }
}

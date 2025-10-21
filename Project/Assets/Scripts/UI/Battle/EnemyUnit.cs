using cfg;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUnit : Unit
{
    // 攻击当前单位坦克的子弹
    private List<GameObject> _BulletList = new List<GameObject>();
    public void AddBullet(GameObject bullet)
    {
        _BulletList.Add(bullet);
    }

    public void OnDestroy()
    {

        Debug.Log("Unit_OnDestory");
        gameObject.SetActive(false);

        gameObject.transform.DOKill();
        Destroy(gameObject, 0.01f);
    }
}

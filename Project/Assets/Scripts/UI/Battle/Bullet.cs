using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : BasePanel
{
    private Transform _targetForm;
    public Transform TargetForm
    {
        get { return _targetForm; }
        set { _targetForm = value; }
    }

    private void Awake()
    {
        DOTween.SetTweensCapacity(2000, 50);
    }

    public void createAinm()
    {
        transform.DOMove(_targetForm.position, 0.2f)
            .OnComplete(() => {
                transform.DOKill();
                Destroy(gameObject);
            });
    }
}

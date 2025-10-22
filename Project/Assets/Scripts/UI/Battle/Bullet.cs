using UnityEngine;

public class Bullet : UIComponent
{
    private Rigidbody rb;

    // 速度默认是4
    private float bulletSpeed = 5f;

    // 子弹伤害默认为1
    private int bulletAttack = 50;

    /// <summary>
    /// 子弹目标的 GameObject
    /// </summary>
    private GameObject _TargetCoordinates;
    public GameObject TargetCoordinates
    {
        set{ _TargetCoordinates = value; }
    }

    public void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;

        EventManager.Instance.AddEventListener<GameObject>(MyConstants.Enemy_deal, EnemyDeal);
    }

    public void Oestroy()
    {
        EventManager.Instance.RemoveEventListener<GameObject>(MyConstants.Enemy_deal, EnemyDeal);
    }

    public void Update()
    {
        if (_TargetCoordinates)
        {
            Vector3 direction = (_TargetCoordinates.transform.position - transform.position).normalized;
            rb.velocity = direction * bulletSpeed;
        }
        
    }

    /// <summary>
    /// 目标已死亡，子弹销毁
    /// </summary>
    /// <param name="unitObject"></param>
    public void EnemyDeal(GameObject unitObject)
    {
        if(_TargetCoordinates == unitObject && this != null)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider == null)
        {
            return;
        }
        
        if(collider.CompareTag("Enemy"))
        {
            Destroy(gameObject);
            EnemyUnit enemyUnit = _TargetCoordinates.GetComponent<EnemyUnit>();
            if (enemyUnit)
            {
                enemyUnit.UnderAttack(bulletAttack);
            }
        }
    }
}

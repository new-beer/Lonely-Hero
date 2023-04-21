using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bee : Enemy
{
    [Header("移动范围")]
    public float patrolRadius;
    protected override void Awake()
    {
        base.Awake();
        patrolState = new BeePatrolState();
        chaseState = new BeeChaseState();
    }
    //覆盖父类方法，重新描写追踪敌人
    public override bool FoundPlayer()
    {
        var obj = Physics2D.OverlapCircle(transform.position, checkDistance, attackLayer);
        if (obj)
        {
            attacker = obj.transform;
        }
        return obj;
    }
    //绘制检测范围
    public override void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, checkDistance);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, patrolRadius);

    }
    //获得随机移动范围
    public override Vector3 GetNewPoint()
    {
        var targetX = Random.Range(-patrolRadius,patrolRadius);
        var targetY = Random.Range(-patrolRadius,patrolRadius);

        return spwanPoint + new Vector3(targetX, targetY);
    }
    public override void Move()
    {
        
    }
}

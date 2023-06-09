using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoarPatrolState : BaseState
{
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.normalSpeed;

    }
    public override void LogicUpdate()
    {
        //TODO:发现player切换chase状态
        if (currentEnemy.FoundPlayer())
        {
            currentEnemy.SwitchState(NPCState.Chase);
        }
        //如果检测到墙体转向
        if (!currentEnemy.physicsCheck.isGround || (currentEnemy.physicsCheck.touchLeftWall && currentEnemy.faceDir.x < 0) || (currentEnemy.physicsCheck.touchRightWall && currentEnemy.faceDir.x > 0))
        {
            currentEnemy.wait = true;
            currentEnemy.animator.SetBool("walk", false);
        }
        else
        {
            currentEnemy.animator.SetBool("walk", true);
        }
    }
    public override void PhysicsUpdate()
    {
        
    }
    public override void OnExit()
    {
        currentEnemy.animator.SetBool("walk", false);
    }

}

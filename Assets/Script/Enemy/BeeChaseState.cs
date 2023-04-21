using System.Collections;
using System.Collections.Generic;
using System.Drawing.Printing;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BeeChaseState : BaseState
{
    private Vector3 target;
    private Vector3 moveDir;
    private Attack attacker;
    private bool isAttack;
    private float attackRateCounter =0;
    public override void OnEnter(Enemy enemy)
    {
        currentEnemy = enemy;
        currentEnemy.currentSpeed = currentEnemy.chaseSpeed;
        attacker = enemy.GetComponent<Attack>();
        currentEnemy.animator.SetBool("chase", true);
    }
    public override void LogicUpdate()
    {
        if (currentEnemy.lostTimeCounter <= 0)
        {
            currentEnemy.SwitchState(NPCState.Patrol);
        }
        //��ʱ��
        attackRateCounter -= Time.deltaTime;
        //׷����Ŀ������ҵ�����
        target = new Vector3(currentEnemy.attacker.position.x, currentEnemy.attacker.position.y + 1.5f, 0);
        //�жϹ�������
        if(Mathf.Abs(target.x - currentEnemy.transform.position.x)<=attacker.attackRang&& Mathf.Abs(target.y - currentEnemy.transform.position.y) <= attacker.attackRang)
        {
            //����
            isAttack = true;
            
            if(!currentEnemy.isHurt)
                currentEnemy.rb.velocity = Vector2.zero;
            //�����ʹ���
            if(attackRateCounter <= 0)
            {
                attackRateCounter = attacker.attackRate;
                currentEnemy.animator.SetTrigger("attack");
            }
        }
        //����������Χ
        else
        {
            isAttack = false;
        }
        //��ת
        moveDir = (target - currentEnemy.transform.position).normalized;
        if (moveDir.x > 0)
        {
            currentEnemy.transform.localScale = new Vector3(-1, 1, 1);
        }
        if (moveDir.x < 0)
        {
            currentEnemy.transform.localScale = new Vector3(1, 1, 1);
        }
    }
    public override void PhysicsUpdate()
    {
        //�۷��׷��
        if (!currentEnemy.isHurt && !currentEnemy.isDie&&!isAttack)
        {
            currentEnemy.rb.velocity = moveDir * currentEnemy.currentSpeed * Time.deltaTime;
        }
    }
    public override void OnExit()
    {
        currentEnemy.animator.SetBool("chase", false);
    }

}

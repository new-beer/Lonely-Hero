using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    [Header("基本属性")]
    public float maxHealth;
    public float currentHealth;
    [Header("受伤无敌")]
    public float invulnerableDuration; //无敌时间
    [HideInInspector]public float invulnerableCounter; //计数器
    public bool invulnerable; //无敌状态
    public UnityEvent<Transform> OnTakeDamage; //创建被攻击事件
    public UnityEvent OnDie; //创建死亡事件

    private void Start()
    {
        currentHealth = maxHealth;
    }
    private void Update()
    {
        //更新无敌时间
        if (invulnerable)
        {
            invulnerableCounter -= Time.deltaTime;
            if (invulnerableCounter <= 0)
            {
                invulnerable = false;
            }
        }
    }
    //接受伤害
    public void TakeDamge(Attack attacker)
    {
        if (invulnerable)
            return;
        //剩余血量足够受到下一次伤害
        if(currentHealth - attacker.damage > 0)
        {
            currentHealth -= attacker.damage;
            TriggerInvulnerable();
            //执行受伤
            OnTakeDamage?.Invoke(attacker.transform);
        }
        else 
        {
            currentHealth = 0;
            //触发死亡
            OnDie?.Invoke();
        }
    }
    //触发无敌
    private void TriggerInvulnerable()
    {
        if (!invulnerable)
        {
            invulnerable = true;
            invulnerableCounter = invulnerableDuration;
        }
    }
    
}

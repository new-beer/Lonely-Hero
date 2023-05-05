using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    [Header("监听事件")]
    public VoidEVentSO newGameEvent;
    [Header("基本属性")]
    public float maxHealth;
    public float currentHealth;
    public float maxPower;
    public float currentPower;
    public float powerRecoverSpeed;
    [Header("受伤无敌")]
    public float invulnerableDuration; //无敌时间
    [HideInInspector]public float invulnerableCounter; //计数器
    public bool invulnerable; //无敌状态
    public UnityEvent<Character> OnHealthChange;//
    public UnityEvent<Transform> OnTakeDamage; //创建被攻击事件
    public UnityEvent OnDie; //创建死亡事件


    //新游戏开始时人物血量恢复
    private void NewGame()
    {
        currentHealth = maxHealth;
        currentPower = maxPower; ;
        OnHealthChange?.Invoke(this);

    }
    private void Start()
    {
        //新游戏开始时敌人血量变满
        currentHealth = maxHealth;
    }
    //确保在新游戏开始时人物血量满
    private void OnEnable()
    {
        newGameEvent.OnEventRaised += NewGame;
    }
    private void OnDisable()
    {
        newGameEvent.OnEventRaised -= NewGame;
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
        //能量恢复
        if (currentPower < maxPower)
        {
            currentPower += Time.deltaTime * powerRecoverSpeed;
        }
    }
    //触发水面死亡
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Water"))
        {
            //死亡,更新血量
            currentHealth = 0;
            OnHealthChange?.Invoke(this);
            OnDie?.Invoke();

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
        OnHealthChange?.Invoke(this);
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
    //将能量数据传出
    public void Onslide(int cost)
    {
        currentPower -= cost;
        OnHealthChange?.Invoke(this);
    }
    
}

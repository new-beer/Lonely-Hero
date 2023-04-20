using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    [Header("��������")]
    public float maxHealth;
    public float currentHealth;
    [Header("�����޵�")]
    public float invulnerableDuration; //�޵�ʱ��
    [HideInInspector]public float invulnerableCounter; //������
    public bool invulnerable; //�޵�״̬
    public UnityEvent<Transform> OnTakeDamage; //�����������¼�
    public UnityEvent OnDie; //���������¼�

    private void Start()
    {
        currentHealth = maxHealth;
    }
    private void Update()
    {
        //�����޵�ʱ��
        if (invulnerable)
        {
            invulnerableCounter -= Time.deltaTime;
            if (invulnerableCounter <= 0)
            {
                invulnerable = false;
            }
        }
    }
    //�����˺�
    public void TakeDamge(Attack attacker)
    {
        if (invulnerable)
            return;
        //ʣ��Ѫ���㹻�ܵ���һ���˺�
        if(currentHealth - attacker.damage > 0)
        {
            currentHealth -= attacker.damage;
            TriggerInvulnerable();
            //ִ������
            OnTakeDamage?.Invoke(attacker.transform);
        }
        else 
        {
            currentHealth = 0;
            //��������
            OnDie?.Invoke();
        }
    }
    //�����޵�
    private void TriggerInvulnerable()
    {
        if (!invulnerable)
        {
            invulnerable = true;
            invulnerableCounter = invulnerableDuration;
        }
    }
    
}

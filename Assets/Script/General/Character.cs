using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Events;

public class Character : MonoBehaviour
{
    [Header("�����¼�")]
    public VoidEVentSO newGameEvent;
    [Header("��������")]
    public float maxHealth;
    public float currentHealth;
    public float maxPower;
    public float currentPower;
    public float powerRecoverSpeed;
    [Header("�����޵�")]
    public float invulnerableDuration; //�޵�ʱ��
    [HideInInspector]public float invulnerableCounter; //������
    public bool invulnerable; //�޵�״̬
    public UnityEvent<Character> OnHealthChange;//
    public UnityEvent<Transform> OnTakeDamage; //�����������¼�
    public UnityEvent OnDie; //���������¼�


    //����Ϸ��ʼʱ����Ѫ���ָ�
    private void NewGame()
    {
        currentHealth = maxHealth;
        currentPower = maxPower; ;
        OnHealthChange?.Invoke(this);

    }
    private void Start()
    {
        //����Ϸ��ʼʱ����Ѫ������
        currentHealth = maxHealth;
    }
    //ȷ��������Ϸ��ʼʱ����Ѫ����
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
        //�����޵�ʱ��
        if (invulnerable)
        {
            invulnerableCounter -= Time.deltaTime;
            if (invulnerableCounter <= 0)
            {
                invulnerable = false;
            }
        }
        //�����ָ�
        if (currentPower < maxPower)
        {
            currentPower += Time.deltaTime * powerRecoverSpeed;
        }
    }
    //����ˮ������
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Water"))
        {
            //����,����Ѫ��
            currentHealth = 0;
            OnHealthChange?.Invoke(this);
            OnDie?.Invoke();

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
        OnHealthChange?.Invoke(this);
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
    //���������ݴ���
    public void Onslide(int cost)
    {
        currentPower -= cost;
        OnHealthChange?.Invoke(this);
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Rigidbody2D rb;
    [HideInInspector]public Animator animator;
    [HideInInspector]public PhysicsCheck physicsCheck;
    [Header("��������")]
    public float normalSpeed;
    public float chaseSpeed; //��ײ�ٶ�
    [HideInInspector]public float currentSpeed;
    public float hurtForce;
    public Vector3 faceDir;
    [HideInInspector] public Transform attacker;
    [Header("��ʱ��")]
    public float waitTime;
    public float waitTimeCounter;
    public bool wait;
    [Header("״̬")]
    public bool isHurt;
    public bool isDie;
    private BaseState currentState;//��ǰ״̬
    protected BaseState patrolState; //Ѳ��״̬
    protected BaseState chaseState; //׷��״̬

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        physicsCheck = GetComponent<PhysicsCheck>();
        currentSpeed = normalSpeed;
        waitTimeCounter = waitTime;
    }
    //���屻�����
    private void OnEnable()
    {
        currentState = patrolState;
        currentState.OnEnter(this);
    }
    private void Update()
    {
        //���˵ĳ���
        faceDir = new Vector3(-transform.localScale.x, 0, 0);
        
        //ִ���߼�UpDate
        currentState.LogicUpdate();
        //����ǽʱͣ��
        TimeCounter();

    }
    private void FixedUpdate()
    {
        if (!isHurt && !isDie&&!wait)
            Move();
        //�����߼��ж�
        currentState.PhysicsUpdate();
    }
    //������ʧ����
    private void OnDisable()
    {
        currentState.OnExit();
    }
    public virtual void Move()
    {
        rb.velocity = new Vector2(currentSpeed * faceDir.x * Time.deltaTime, rb.velocity.y);
    }
    //���˵ļ�ʱϵͳ
    public void TimeCounter()
    {
        if (wait)
        {
            waitTimeCounter -= Time.deltaTime;
            if (waitTimeCounter <= 0)
            {
                wait = false;
                waitTimeCounter = waitTime;
                transform.localScale = new Vector3(faceDir.x, 1, 1);
            }
        }
    }
    //�������˶���
    public void OnTakeDamage(Transform attackTrans)
    {
        attacker = attackTrans;
        //ת��
        if (attackTrans.position.x - transform.position.x > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        if (attackTrans.position.x - transform.position.x < 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        //���˱�����
        isHurt = true;
        animator.SetTrigger("hurt");
        Vector2 dir = new Vector2(transform.position.x - attackTrans.position.x, 0).normalized;

        StartCoroutine(OnHurt(dir));
    }
    //Я�̷�������˳��ִ�б�������
    private IEnumerator OnHurt(Vector2 dir)
    {
        rb.AddForce(dir * hurtForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.45f);
        isHurt = false;
    }
    //����
    public void OnDie()
    {
        gameObject.layer = 2;
        animator.SetBool("dead", true);
        isDie = true;
    }
    //���������ٶ���
    public void DestroyAnimation()
    {
        Destroy(this.gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected Animator animator;
    PhysicsCheck physicsCheck;
    [Header("基本参数")]
    public float normalSpeed;
    public float chaseSpeed; //冲撞速度
    public float currentSpeed;
    public float hurtForce;
    public Vector3 faceDir;
    public Transform attacker;
    [Header("计时器")]
    public float waitTime;
    public float waitTimeCounter;
    public bool wait;
    [Header("状态")]
    public bool isHurt;
    public bool isDie;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        physicsCheck = GetComponent<PhysicsCheck>();
        currentSpeed = normalSpeed;
        waitTimeCounter = waitTime;
    }
    private void Update()
    {
        //敌人的朝向
        faceDir = new Vector3(-transform.localScale.x, 0, 0);
        //如果检测到墙体转向
        if((physicsCheck.touchLeftWall&&faceDir.x<0) || (physicsCheck.touchRightWall&&faceDir.x>0))
        {
            wait = true;
            animator.SetBool("walk", false);
        }
        //在碰墙时停留
        TimeCounter();

    }
    private void FixedUpdate()
    {
        Move();
    }
    public virtual void Move()
    {
        if(!isHurt&&!isDie)
            rb.velocity = new Vector2(currentSpeed * faceDir.x * Time.deltaTime, rb.velocity.y);
    }
    //敌人的计时系统
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
    //敌人受伤动画
    public void OnTakeDamage(Transform attackTrans)
    {
        attacker = attackTrans;
        //转身
        if (attackTrans.position.x - transform.position.x > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        if (attackTrans.position.x - transform.position.x < 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        //受伤被击退
        isHurt = true;
        animator.SetTrigger("hurt");
        Vector2 dir = new Vector2(transform.position.x - attackTrans.position.x, 0).normalized;

        StartCoroutine(OnHurt(dir));
    }
    //携程方法，按顺序执行被被击退
    private IEnumerator OnHurt(Vector2 dir)
    {
        rb.AddForce(dir * hurtForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.45f);
        isHurt = false;
    }
    //死亡
    public void OnDie()
    {
        gameObject.layer = 2;
        animator.SetBool("dead", true);
        isDie = true;
    }
    //死亡后销毁动画
    public void DestroyAnimation()
    {
        Destroy(this.gameObject);
    }
}

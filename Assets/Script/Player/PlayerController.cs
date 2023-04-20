using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    public PlayerInputController inputControl;
    public Rigidbody2D rb;
    public CapsuleCollider2D capsuleCollider2D;
    public Vector2 inputDirection; //输入方向
    public PlayerAnimation playerAnimation;
    private PhysicsCheck physicsCheck;
    private SpriteRenderer spriteRenderer;
    [Header("基本参数")]
    public float speed;
    public float jumpForce;
    public float hurtForce;
    private float runSpeed;
    private float walkSpeed => runSpeed/2.5f;
    private Vector2 originalOffset;
    private Vector2 originalSize;
    [Header("状态")]
    public bool isCrouch;
    public bool isHurt;
    public bool isDie;
    public bool isAttack;
    [Header("物理材质")]
    public PhysicsMaterial2D normal;
    public PhysicsMaterial2D wall;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        inputControl = new PlayerInputController();
        spriteRenderer = GetComponent<SpriteRenderer>();
        physicsCheck = GetComponent<PhysicsCheck>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        playerAnimation = GetComponent<PlayerAnimation>();
        originalOffset = capsuleCollider2D.offset;
        originalSize = capsuleCollider2D.size;
        //检测jump键被按压
        inputControl.Player.Jump.started += Jump;
        #region 强制走路
        //检测Walk键一直被按压
        runSpeed = speed;
        inputControl.Player.WalkBotton.performed += ctx =>
        {
            if (physicsCheck.isGround)
            {
                speed = walkSpeed;
            }
        };
        inputControl.Player.WalkBotton.canceled += ctx =>
        {
            if (physicsCheck.isGround)
            {
                speed = runSpeed;
            }
        };
        #endregion
        //检测Attack键被按压
        inputControl.Player.Attack.started += PlayerAttack;

    }
    private void OnEnable()
    {
        inputControl.Enable();
    }
    private void OnDisable()
    {
        inputControl.Disable();
    }
    private void Update()
    {
        //检测每帧键盘手柄的输入
        inputDirection = inputControl.Player.Move.ReadValue<Vector2>();
        //调用checkstate
        CheckState();
    }
    private void FixedUpdate()
    {
        if(!isHurt && !isAttack)
            Move();
    }
    //private void OnTriggerStay2D(Collider2D other)
    //{
    //    Debug.Log(other.name);
    //}
    //人物位置移动
    public void Move()
    {
        if(!isCrouch)
            rb.velocity = new Vector2(inputDirection.x*speed*Time.deltaTime,rb.velocity.y);
        //人物翻转
        //if (inputDirection.x > 0)
        //{
        //    spriteRenderer.flipX = false;
        //}
        //else if (inputDirection.x < 0)
        //{
        //    spriteRenderer.flipX = true;
        //}
        int faceDir = (int)transform.localScale.x;
        if (inputDirection.x > 0)
        {
            faceDir = 1;
        }
        if (inputDirection.x < 0)
        {
            faceDir = -1;
        }
        transform.localScale = new Vector3(faceDir, 1, 1);
        //下蹲
        isCrouch = inputDirection.y < -0.5f && physicsCheck.isGround;
        if (isCrouch)
        {
            //修该碰状体大小和位移
            capsuleCollider2D.offset = new Vector2(-0.05f, 0.85f);
            capsuleCollider2D.size = new Vector2(0.7f, 1.7f);
        }
        else
        {
            //还原之前碰撞体参数
            capsuleCollider2D.offset = originalOffset;
            capsuleCollider2D.size = originalSize;
        }
    }
    //跳跃
    public void Jump(InputAction.CallbackContext obj)
    {
        if(physicsCheck.isGround)
            rb.AddForce(transform.up * jumpForce,ForceMode2D.Impulse);
    }
    //攻击方法
    private void PlayerAttack(InputAction.CallbackContext obj)
    {
        if (!physicsCheck.isGround)
            return;
        playerAnimation.PlayAttack();
        isAttack = true;
    }
    #region UnityEvent
    //受到伤害时人物的移动
    public void GetHurt(Transform attacker)
    {
        isHurt = true;
        //受伤停止移动
        rb.velocity = Vector2.zero;
        //受伤被反向击退方向
        Vector2 dir = new Vector2((transform.position.x - attacker.position.x), 0).normalized;
        rb.AddForce(dir * hurtForce, ForceMode2D.Impulse);
    }
    //玩家死亡后设置
    public void PlayerDie()
    {
        isDie = true;
        inputControl.Player.Disable();
    }
    #endregion
    //避免玩家死亡后被敌人攻击

    private void CheckState()
    {
        //碰撞体材质取决于是否跳跃
        capsuleCollider2D.sharedMaterial = physicsCheck.isGround ? normal : wall;
        if (isDie)
        {
            gameObject.layer = LayerMask.NameToLayer("Enemy");
        }
        else
        {
            gameObject.layer = LayerMask.NameToLayer("Player");
        }
    }
}

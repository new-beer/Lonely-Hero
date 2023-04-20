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
    public Vector2 inputDirection; //���뷽��
    public PlayerAnimation playerAnimation;
    private PhysicsCheck physicsCheck;
    private SpriteRenderer spriteRenderer;
    [Header("��������")]
    public float speed;
    public float jumpForce;
    public float hurtForce;
    private float runSpeed;
    private float walkSpeed => runSpeed/2.5f;
    private Vector2 originalOffset;
    private Vector2 originalSize;
    [Header("״̬")]
    public bool isCrouch;
    public bool isHurt;
    public bool isDie;
    public bool isAttack;
    [Header("�������")]
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
        //���jump������ѹ
        inputControl.Player.Jump.started += Jump;
        #region ǿ����·
        //���Walk��һֱ����ѹ
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
        //���Attack������ѹ
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
        //���ÿ֡�����ֱ�������
        inputDirection = inputControl.Player.Move.ReadValue<Vector2>();
        //����checkstate
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
    //����λ���ƶ�
    public void Move()
    {
        if(!isCrouch)
            rb.velocity = new Vector2(inputDirection.x*speed*Time.deltaTime,rb.velocity.y);
        //���﷭ת
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
        //�¶�
        isCrouch = inputDirection.y < -0.5f && physicsCheck.isGround;
        if (isCrouch)
        {
            //�޸���״���С��λ��
            capsuleCollider2D.offset = new Vector2(-0.05f, 0.85f);
            capsuleCollider2D.size = new Vector2(0.7f, 1.7f);
        }
        else
        {
            //��ԭ֮ǰ��ײ�����
            capsuleCollider2D.offset = originalOffset;
            capsuleCollider2D.size = originalSize;
        }
    }
    //��Ծ
    public void Jump(InputAction.CallbackContext obj)
    {
        if(physicsCheck.isGround)
            rb.AddForce(transform.up * jumpForce,ForceMode2D.Impulse);
    }
    //��������
    private void PlayerAttack(InputAction.CallbackContext obj)
    {
        if (!physicsCheck.isGround)
            return;
        playerAnimation.PlayAttack();
        isAttack = true;
    }
    #region UnityEvent
    //�ܵ��˺�ʱ������ƶ�
    public void GetHurt(Transform attacker)
    {
        isHurt = true;
        //����ֹͣ�ƶ�
        rb.velocity = Vector2.zero;
        //���˱�������˷���
        Vector2 dir = new Vector2((transform.position.x - attacker.position.x), 0).normalized;
        rb.AddForce(dir * hurtForce, ForceMode2D.Impulse);
    }
    //�������������
    public void PlayerDie()
    {
        isDie = true;
        inputControl.Player.Disable();
    }
    #endregion
    //������������󱻵��˹���

    private void CheckState()
    {
        //��ײ�����ȡ�����Ƿ���Ծ
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

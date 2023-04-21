using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{
    private CapsuleCollider2D capsuleCollider2D;
    private PlayerController playerController;
    private Rigidbody2D rb;
    [Header("������")]
    public bool manual; //�ֶ�
    public bool isPlayer; //�ж��Ƿ�����ҵ����
    public float checkRaduis; //��ⷶΧ
    public Vector2 bottomOffset; //����һ���ױ�ƫ����
    public Vector2 leftOffset;
    public Vector2 rightOffset;
    public LayerMask groundLayer; //���ͼ��
    [Header("״̬")]
    public bool isGround;
    public bool onWall;
    public bool touchLeftWall;
    public bool touchRightWall;
    private void Awake()
    {
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        if (!manual)
        {
            rightOffset = new Vector2((capsuleCollider2D.bounds.size.x) / 2, capsuleCollider2D.bounds.size.y / 2);
            leftOffset = new Vector2(-rightOffset.x, rightOffset.y);
        }
        if (isPlayer)
        {
            playerController = GetComponent<PlayerController>();
        }
    }
    private void Update()
    {
        Check();
    }
    private void Check()
    {
        //������
        if (onWall)
        {
            isGround = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(bottomOffset.x * transform.localScale.x, bottomOffset.y), checkRaduis, groundLayer);
        }
        else
        {
            isGround = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(bottomOffset.x * transform.localScale.x, 0), checkRaduis, groundLayer);
        }
            
        //���ǽ��
        touchLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(leftOffset.x,leftOffset.y), checkRaduis, groundLayer);
        touchRightWall = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(rightOffset.x,rightOffset.y), checkRaduis, groundLayer);
        //��ǽ����
        if(isPlayer)
            onWall = (touchLeftWall && playerController.inputDirection.x <0f || touchRightWall && playerController.inputDirection.x>0f) && rb.velocity.y<0f;
    }
    private void OnDrawGizmosSelected()
    {
        //���Ƶײ��ⷶΧ
        Gizmos.DrawWireSphere((Vector2)transform.position + new Vector2(bottomOffset.x*transform.localScale.x,bottomOffset.y), checkRaduis);
        //����ǽ���ⷶΧ
        Gizmos.DrawWireSphere((Vector2)transform.position + new Vector2(leftOffset.x,leftOffset.y), checkRaduis);
        Gizmos.DrawWireSphere((Vector2)transform.position + new Vector2(rightOffset.x,rightOffset.y), checkRaduis);
    }
}

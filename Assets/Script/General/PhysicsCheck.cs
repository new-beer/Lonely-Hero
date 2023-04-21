using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{
    private CapsuleCollider2D capsuleCollider2D;
    private PlayerController playerController;
    private Rigidbody2D rb;
    [Header("检测参数")]
    public bool manual; //手动
    public bool isPlayer; //判断是否是玩家的组件
    public float checkRaduis; //检测范围
    public Vector2 bottomOffset; //定义一个底边偏移量
    public Vector2 leftOffset;
    public Vector2 rightOffset;
    public LayerMask groundLayer; //检测图层
    [Header("状态")]
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
        //检测地面
        if (onWall)
        {
            isGround = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(bottomOffset.x * transform.localScale.x, bottomOffset.y), checkRaduis, groundLayer);
        }
        else
        {
            isGround = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(bottomOffset.x * transform.localScale.x, 0), checkRaduis, groundLayer);
        }
            
        //检测墙面
        touchLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(leftOffset.x,leftOffset.y), checkRaduis, groundLayer);
        touchRightWall = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(rightOffset.x,rightOffset.y), checkRaduis, groundLayer);
        //在墙面上
        if(isPlayer)
            onWall = (touchLeftWall && playerController.inputDirection.x <0f || touchRightWall && playerController.inputDirection.x>0f) && rb.velocity.y<0f;
    }
    private void OnDrawGizmosSelected()
    {
        //绘制底层检测范围
        Gizmos.DrawWireSphere((Vector2)transform.position + new Vector2(bottomOffset.x*transform.localScale.x,bottomOffset.y), checkRaduis);
        //绘制墙体检测范围
        Gizmos.DrawWireSphere((Vector2)transform.position + new Vector2(leftOffset.x,leftOffset.y), checkRaduis);
        Gizmos.DrawWireSphere((Vector2)transform.position + new Vector2(rightOffset.x,rightOffset.y), checkRaduis);
    }
}

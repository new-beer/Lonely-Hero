using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{
    private CapsuleCollider2D capsuleCollider2D;
    [Header("¼ì²â²ÎÊý")]
    public bool manual; //ÊÖ¶¯
    public float checkRaduis; //¼ì²â·¶Î§
    public Vector2 bottomOffset; //¶¨ÒåÒ»¸öµ×±ßÆ«ÒÆÁ¿
    public Vector2 leftOffset;
    public Vector2 rightOffset;
    public LayerMask groundLayer; //¼ì²âÍ¼²ã
    [Header("×´Ì¬")]
    public bool isGround;
    public bool touchLeftWall;
    public bool touchRightWall;
    private void Awake()
    {
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        if (!manual)
        {
            rightOffset = new Vector2((capsuleCollider2D.bounds.size.x) / 2, capsuleCollider2D.bounds.size.y / 2);
            leftOffset = new Vector2(-rightOffset.x, rightOffset.y);
        }
    }
    private void Update()
    {
        Check();
    }
    private void Check()
    {
        //¼ì²âµØÃæ
        isGround = Physics2D.OverlapCircle((Vector2)transform.position+new Vector2(bottomOffset.x*transform.localScale.x,bottomOffset.y), checkRaduis,groundLayer);
        //¼ì²âÇ½Ãæ
        touchLeftWall = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(leftOffset.x,leftOffset.y), checkRaduis, groundLayer);
        touchRightWall = Physics2D.OverlapCircle((Vector2)transform.position + new Vector2(rightOffset.x,rightOffset.y), checkRaduis, groundLayer);
    }
    private void OnDrawGizmosSelected()
    {
        //»æÖÆµ×²ã¼ì²â·¶Î§
        Gizmos.DrawWireSphere((Vector2)transform.position + new Vector2(bottomOffset.x*transform.localScale.x,bottomOffset.y), checkRaduis);
        //»æÖÆÇ½Ìå¼ì²â·¶Î§
        Gizmos.DrawWireSphere((Vector2)transform.position + new Vector2(leftOffset.x,leftOffset.y), checkRaduis);
        Gizmos.DrawWireSphere((Vector2)transform.position + new Vector2(rightOffset.x,rightOffset.y), checkRaduis);
    }
}

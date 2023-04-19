using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public int damage;
    public float attackRang;  //¹¥»÷·¶Î§
    public float attackRate;  //¹¥»÷ËÙÂÊ
    private void OnTriggerStay2D(Collider2D other)
    {
        other.GetComponent<Character>()?.TakeDamge(this);
    }
}

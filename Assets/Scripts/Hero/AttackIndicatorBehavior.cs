using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackIndicatorBehavior : MonoBehaviour
{  
    private float attackTimer = 0f;
    public int normalAttackHarm = 25;//普通攻击伤害
    void Start()
    {
        attackTimer = 0f;
    }

    void Update()
    {
        attackTimer += Time.deltaTime;
        if(attackTimer >= 0.6f)
        {
            gameObject.SetActive(false);
            attackTimer = 0f;
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Debug.Log("Attacking enemy");
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Debug.Log("Attacking enemy");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackIndicatorBehavior : MonoBehaviour
{  
    protected float attackTimer = 0f;
    void Start()
    {
        attackTimer = 0f;
        gameObject.GetComponent<HeroAttackHurt>().hurt=1;
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
            gameObject.SetActive(false);
        }
    }
}

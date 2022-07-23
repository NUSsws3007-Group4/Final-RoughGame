using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackIndicatorBehavior : MonoBehaviour
{  
    protected float attackTimer = 0f;
    protected float normalAttackColdDown = 0.6f;
    void Start()
    {
        attackTimer = 0f;
        GameObject.Find("hero").GetComponent<HeroAttackHurt>().hurt=25;//Tag:normalAttack
    }

    void Update()
    {
        attackTimer += Time.deltaTime;
        if(attackTimer >= normalAttackColdDown)
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackIndicatorBehavior : MonoBehaviour
{  
    protected const float OriginCD = 0.6f;//初始设定CD
    protected float attackTimer = 0f;
    protected float normalAttackColdDown;
    void Start()
    {
        attackTimer = 0f;       
        gameObject.transform.parent.GetComponent<HeroAttackHurt>().hurt = 25;
        normalAttackColdDown = OriginCD * gameObject.transform.parent.GetComponent<HeroAttackHurt>().coldDownCoef;
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
        if(collision.gameObject.layer == LayerMask.NameToLayer("Enemy")|| collision.gameObject.layer == LayerMask.NameToLayer("Boss"))
        {
            GameObject.Find("hero").GetComponent<AudioManager>().PlayEnemy("EnemyHurt");
            Debug.Log("Attacking enemy");
            gameObject.SetActive(false);
        }
    }
}

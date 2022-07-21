using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseCombactEnemyBehavior : MonoBehaviour
{
   protected float attackTimer = 0f;
   GameObject parent;
    void Start()
    {
        attackTimer = 0f;
    }

    void Update()
    {
        attackTimer += Time.deltaTime;
        if(attackTimer >= 0.25f)
        {
            gameObject.SetActive(false);
            attackTimer = 0f;
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if(collision.gameObject.layer == 8)
        {
            Debug.Log("Attacking hero");
            gameObject.SetActive(false);
        }
    }
}

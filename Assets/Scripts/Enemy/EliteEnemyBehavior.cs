using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EliteEnemyBehavior : EnemyBehavior
{
    // Start is called before the first frame update
    protected override void Start()
    {
        anim = GetComponent<Animator>();
        enemyRenderer = GetComponent<SpriteRenderer>();
        mRigidbody = gameObject.GetComponent<Rigidbody2D>();
        targetHero = GameObject.Find("hero");
        detectDistance = 2f;
        initialpos = transform.localPosition;
        initialright = transform.right;
    }
    override protected void patrolBehavior()
    {
        transform.GetChild(1).GetComponent<Renderer>().enabled = false;
        distance = Vector3.Distance(pos, targetpos);
        if (distance <= detectDistance)
        {
            targetDirection = (targetpos - pos).normalized;
            info = Physics2D.Raycast(transform.localPosition, targetDirection, chasedistance, 1 << 6 | 1 << 8);
            if (info.collider != null && info.collider.gameObject.layer == 8)
            {
                patrol = false;
                transform.GetChild(0).GetComponent<Renderer>().enabled = true;
            }

        }
    }

    protected override void chaseBehavior()
    {
        anim.SetBool("Walking", true);
        base.chaseBehavior();
    }
}

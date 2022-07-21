using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyEliteEnemy : EnemyBehavior
{
    private Vector3 stayPosition;
    private float arrowShotTimer = 0f;
    // Start is called before the first frame update
    protected override void Start()
    {
        anim = GetComponent<Animator>();
        enemyRenderer = GetComponent<SpriteRenderer>();
        mRigidbody = gameObject.GetComponent<Rigidbody2D>();
        targetHero = GameObject.Find("hero");
        detectDistance = 10f;
        initialpos = transform.localPosition;
        initialright = transform.right;
    }
    override protected void patrolBehavior()
    {
        anim.SetBool("FlyEliteIdle", true);

        transform.GetChild(1).GetComponent<Renderer>().enabled = false;
        distance = Vector3.Distance(pos, targetpos);
        if (distance <= detectDistance)
        {
            targetDirection = (targetpos - pos).normalized;
            info = Physics2D.Raycast(transform.localPosition, targetDirection, chasedistance, 1 << 6 | 1 << 8);
            if (info.collider != null && info.collider.gameObject.layer == 8)
            {
                patrol = false;
                stayPosition = transform.localPosition;
                transform.GetChild(0).GetComponent<Renderer>().enabled = true;
            }

        }
    }

    protected override void chaseBehavior()
    {
        transform.localPosition = stayPosition;
        arrowShotTimer += Time.deltaTime;
        if(arrowShotTimer >= 1.0f)
        {
            arrowShotTimer = 0f;
            attackBehavior();
        }
        
    }

    protected override void attackBehavior()
    {
        anim.SetBool("FLyEliteAttack", true);
        GameObject tempArrow = Instantiate(Resources.Load("Prefabs/Arrow") as GameObject);
        targetpos = targetHero.transform.localPosition;
        tempArrow.transform.localPosition = transform.localPosition;
        tempArrow.transform.right = (targetpos - transform.localPosition).normalized;
    }
}

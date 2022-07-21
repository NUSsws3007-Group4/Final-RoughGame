using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyEliteEnemy : EnemyBehavior
{
    //private Vector3 stayPosition;
    private float arrowShotTimer = 0f;
    // Start is called before the first frame update
    protected override void Start()
    {
        detectDistance = 10f;
        detectAngle = 90f;

        anim = GetComponent<Animator>();
        enemyRenderer = GetComponent<SpriteRenderer>();
        mRigidbody = gameObject.GetComponent<Rigidbody2D>();
        targetHero = GameObject.Find("hero");
        detectDistance = 4f;
        initialpos = transform.localPosition;
        initialright = transform.right;
    }
    override protected void patrolBehavior()
    {
        anim.SetBool("FlyEliteIdle", true);
        
        base.patrolBehavior();

    }

    protected override void chaseBehavior()
    {
        mRigidbody.velocity = new Vector3(0, 0, 0);

        arrowShotTimer += Time.deltaTime;
        
        distance = Vector3.Distance(pos, targetpos);
        
        if(arrowShotTimer >= 1.0f)
        {
            arrowShotTimer = 0f;
            attackBehavior();
        }

        if(distance >= 5f)
        {
            arrowShotTimer = 0f;
            transform.GetChild(0).GetComponent<Renderer>().enabled = false;
            patrol = true;
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

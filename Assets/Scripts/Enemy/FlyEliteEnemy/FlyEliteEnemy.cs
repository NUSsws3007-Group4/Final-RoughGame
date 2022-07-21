using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyEliteEnemy : EnemyBehavior
{
    private const float pi = 3.1415926f;
    private int barrageAttackCount = 0;
    private float arrowShotTimer = 0f;
    private GameObject guardKey, rock;
    // Start is called before the first frame update
    protected override void Start()
    {
        detectDistance = 10f;
        detectAngle = 90f;

        anim = GetComponent<Animator>();
        enemyRenderer = GetComponent<SpriteRenderer>();
        mRigidbody = gameObject.GetComponent<Rigidbody2D>();
        targetHero = GameObject.Find("hero");
        guardKey = GameObject.Find("KeyArea");
        rock = GameObject.Find("Rock");
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
        // rock.GetComponent<RockBehavior>().
        mRigidbody.velocity = new Vector3(0, 0, 0);

        arrowShotTimer += Time.deltaTime;

        distance = Vector3.Distance(pos, targetpos);

        if (arrowShotTimer >= 1.0f)
        {
            arrowShotTimer = 0f;
            attackBehavior();
        }

        if (distance >= 5f)
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

        barrageAttackCount++;
        if (barrageAttackCount >= 3)
        {
            barrageAttackCount = 0;
            BarrageArrowAttack();
        }

    }

    protected override void friendlyBehavior()
    {
        base.friendlyBehavior();
        if (distance < detectDistance)
        {
            // rock.GetComponent<RockBehavior>().
        }

    }
    private void BarrageArrowAttack()
    {
        Vector3 tvec, direction;
        //Debug.Log("Barrage!");
        for (float i = -30f; i <= 30f; i += 20f)
        {
            GameObject barrageAttack = Instantiate(Resources.Load("Prefabs/Arrow") as GameObject);
            barrageAttack.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            barrageAttack.transform.localPosition = transform.localPosition;

            tvec = targetHero.transform.localPosition - transform.localPosition;
            direction.x = tvec.x * Mathf.Cos(i / 180f * pi) + tvec.y * Mathf.Sin(i / 180f * pi);
            direction.y = -tvec.x * Mathf.Sin(i / 180f * pi) + tvec.y * Mathf.Cos(i / 180f * pi);
            direction.z = 0;
            barrageAttack.transform.up = direction.normalized;
        }
    }

    protected override void Death()
    {
        Destroy(guardKey.gameObject);
        base.Death();
    }
    public void AllowPass()
    {
        Destroy(guardKey.gameObject);
    }

}

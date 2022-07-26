using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyEliteEnemy : EnemyBehavior
{
    private const float pi = 3.1415926f;
    private int barrageAttackCount = 0;
    private float arrowShotTimer = 0f;
    private GameObject guardPortal, puzzleBase;
    // Start is called before the first frame update
    protected override void Start()
    {
        detectDistance = 15f;
        detectAngle = 90f;
        chaseDistance = 20f;
        mLifeLeft = 200;

        anim = GetComponent<Animator>();
        enemyRenderer = GetComponent<SpriteRenderer>();
        mRigidbody = gameObject.GetComponent<Rigidbody2D>();
        targetHero = GameObject.Find("hero");
        guardPortal = GameObject.Find("KeyArea");
        puzzleBase = GameObject.Find("puzzlebase");
        detectDistance = 4f;
        initialpos = transform.localPosition;
        initialright = transform.right;
    }
    override protected void patrolBehavior()
    {
        anim.SetBool("FlyEliteIdle", true);
        transform.GetChild(2).GetComponent<Renderer>().enabled = false;
        puzzleBase.GetComponent<PuzzleControl>().setPuzzleOpen(false);
        base.patrolBehavior();
    }

    protected override void chaseBehavior()
    {

        mRigidbody.velocity = new Vector3(0, 0, 0);

        arrowShotTimer += Time.deltaTime;

        distance = Vector3.Distance(pos, targetpos);

        if (arrowShotTimer >= 1.0f)
        {
            arrowShotTimer = 0f;
            attackBehavior();
        }

        if (distance >= chaseDistance)
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
            puzzleBase.GetComponent<PuzzleControl>().setPuzzleOpen(true);
            transform.GetChild(2).GetComponent<Renderer>().enabled = true;
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

    protected override void attackedBehavior()
    {
        transform.GetChild(3).gameObject.SetActive(true);
        attackedTimer -= Time.deltaTime;
        mRigidbody.AddForce(0.1f * attackedTimer * transform.right);
        if (attackedTimer <= 0)
        {
            attackedTimer = 0.5f;
            anim.SetBool("Attacked", false);
            transform.GetChild(3).gameObject.SetActive(false);
            if (mLifeLeft <= 0)
                Death();
        }
    }

    protected override void Death()
    {
        GameObject Jp;
        if (Jp = GameObject.Find("jumppad"))
        {
            Jp.SetActive(true);
        }
        Destroy(guardPortal.gameObject);
        base.Death();
    }
    public void AllowPass(GameObject Jp = null)
    {
        if(Jp)
        {
            Jp.SetActive(true);
        }
        Destroy(guardPortal.gameObject);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    protected float waitTimer = 0, attackTimer = 1.5f, attackedTimer = 0.5f,
                    detectDistance = 8f, chasedistance = 12f, detectAngle = 45f,
                    distance, angle, dot;
    protected int mLifeLeft = 4, mFriendshipRequired = 20;
    protected bool patrol = true;
    protected SpriteRenderer enemyRenderer;
    protected Rigidbody2D mRigidbody;
    public GameObject targetHero;
    protected Vector3 targetpos, initialpos, initialright, pos, targetDirection, vel;
    protected RaycastHit2D info;
    protected Animator anim;


    void Start()
    {
        anim = GetComponent<Animator>();
        enemyRenderer = GetComponent<SpriteRenderer>();
        mRigidbody = gameObject.GetComponent<Rigidbody2D>();
        targetHero = GameObject.Find("hero");

        initialpos = transform.localPosition;
        initialright = transform.right;
        vel = mRigidbody.velocity;
        vel.x = 0f;
        mRigidbody.velocity = transform.right * 4 + vel;

    }

    // Update is called once per frame
    void Update()
    {
        if (mLifeLeft <= 0)
            Death();
        pos = transform.localPosition;
        targetpos = targetHero.transform.localPosition;
        targetDirection = targetpos - pos;
        dot = Vector3.Dot(transform.right, targetDirection.normalized);

        if (patrol)
        {
            if (targetHero.GetComponent<HeroBehavior>().getFriendship() >= mFriendshipRequired)
                friendlyBehavior();
            else
                patrolBehavior();
        }
        else
        {
            if (targetHero.GetComponent<HeroBehavior>().IsRespawned())
                Respawn();
            info = Physics2D.Raycast(transform.localPosition, targetDirection, chasedistance, 1 << 6 | 1 << 8);
            if (dot < -0.2f)
                transform.right = -transform.right;
            if (dot < -0.2f || dot > 0.2f)
            {
                vel = mRigidbody.velocity;
                vel.x = 0f;
                mRigidbody.velocity = transform.right * 4 + vel;
            }
            else
            {
                mRigidbody.velocity = new Vector3(0, 0, 0);
            }

            attackTimer += Time.deltaTime;
            if (attackTimer >= 2.0f)
            {
                attackBehavior();
                attackTimer = 0f;
            }

            if (info.collider == null || (info.collider != null && info.collider.gameObject.layer != 8))
            {
                waitTimer += Time.deltaTime;
                if (waitTimer >= 2f)
                {
                    waitTimer = 0;
                    Respawn();
                }
            }
            else
                waitTimer = 0;

        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (patrol && collision.gameObject.layer == 17)
            transform.right = -transform.right;
        vel = mRigidbody.velocity;
        vel.x = 0f;
        mRigidbody.velocity = transform.right * 4 + vel;

        if (collision.gameObject.layer == 19)
        {
            mRigidbody.velocity = new Vector3(0, 0, 0);
            anim.SetTrigger("Attacked");
            mLifeLeft -= collision.gameObject.GetComponent<HeroAttackHurt>().hurt;
            Debug.Log(mLifeLeft);
        }
    }

    void Death()
    {
        //TODO: 爆金币
        AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);
        if (info.normalizedTime >= 1 && info.IsName("attacked"))
            Destroy(transform.gameObject);
    }
    protected void Respawn()
    {
        patrol = true;
        mLifeLeft = 100;
        transform.localPosition = initialpos;
        transform.right = initialright;
        vel = mRigidbody.velocity;
        vel.x = 0f;
        mRigidbody.velocity = transform.right * 4 + vel;
        attackTimer = 1.5f;
        transform.GetChild(0).GetComponent<Renderer>().enabled = false;
    }
    protected virtual void patrolBehavior()
    {
        transform.GetChild(1).GetComponent<Renderer>().enabled = false;
        vel = mRigidbody.velocity;
        vel.x = 0f;
        mRigidbody.velocity = transform.right * 4 + vel;
        distance = Vector3.Distance(pos, targetpos);
        if (distance <= detectDistance)
        {
            angle = Vector3.Angle(targetpos - pos, transform.right);
            if (angle < detectAngle)
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
    }
    protected virtual void friendlyBehavior()
    {
        if (dot < -0.2f)
            transform.right = -transform.right;
        mRigidbody.velocity = new Vector3(0, 0, 0);
        transform.GetChild(1).GetComponent<Renderer>().enabled = true;
    }
    protected virtual void attackBehavior()
    {
        GameObject remoteAttack = Instantiate(Resources.Load("Prefabs/BulletScreen") as GameObject);
        targetpos = targetHero.transform.localPosition;
        remoteAttack.transform.localPosition = transform.localPosition;
        remoteAttack.transform.up = (targetpos - transform.localPosition).normalized;
    }

}
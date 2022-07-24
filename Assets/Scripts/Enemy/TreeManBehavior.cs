using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeManBehavior : MonoBehaviour
{
    private float waitTimer = 0, attackTimer = 1.0f, attackedTimer = 0.5f,
                    detectDistance = 8f, chaseDistance = 12f, detectAngle = 45f,
                    distance, angle, dot;
    private int mLifeLeft = 200, mFriendshipRequired = 20, mFriendshipStatus = 0, multiplication = 2, friendshipAddValue = 10;
    private bool patrol = true, frienshipAdded = false;
    private SpriteRenderer enemyRenderer;
    private Rigidbody2D mRigidbody;
    public GameObject targetHero;
    private Vector3 targetpos, initialpos, initialright, pos, targetDirection, vel;
    private RaycastHit2D info;
    private Animator anim;
    private bool isactive=true;

    public int nowATK = 10;
    private float atkTimer = 0f;

    private void Start()
    {
        anim = GetComponent<Animator>();
        enemyRenderer = GetComponent<SpriteRenderer>();
        mRigidbody = gameObject.GetComponent<Rigidbody2D>();
        targetHero = GameObject.Find("hero");
        initialpos = transform.localPosition;
        initialright = transform.right;
    }
    private void Update()
    {
        if(isactive)
        {
            // if (anim.GetBool("Attacked"))
            //     attackedBehavior();
            //else
            //{
                if (mFriendshipStatus == 0 && targetHero.GetComponent<HeroBehavior>().getFriendship() >= mFriendshipRequired)
                    mFriendshipStatus = 2;
                if (mFriendshipStatus == 2 && targetHero.GetComponent<HeroBehavior>().getFriendship() < mFriendshipRequired)
                    mFriendshipStatus = 0;

                pos = transform.localPosition;
                targetpos = targetHero.transform.localPosition;
                targetDirection = targetpos - pos;
                dot = Vector3.Dot(transform.right, targetDirection.normalized);

                if (patrol)
                    if (mFriendshipStatus >= 1)
                        friendlyBehavior();
                    else
                        patrolBehavior();
                else
                    chaseBehavior();
            //}
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!isactive) return;

        if (collision.gameObject.layer == 19)
        {
            //anim.SetBool("Attacked", true);
            // mRigidbody.velocity = new Vector3(0, 0, 0);
            // mRigidbody.AddForce(-100 * transform.right);
            mLifeLeft -= collision.gameObject.transform.parent.GetComponent<HeroAttackHurt>().hurt *
            collision.gameObject.transform.parent.GetComponent<HeroAttackHurt>().powerUpCoef;//计算受伤
            switch (mFriendshipStatus)
            {
                case 2:
                    for (int i = 0; i < multiplication - 1; ++i)
                        mLifeLeft -= collision.gameObject.transform.parent.GetComponent<HeroAttackHurt>().hurt;
                    mFriendshipStatus = 1;
                    targetHero.gameObject.GetComponent<HeroBehavior>().downFriendship(10);
                    if (frienshipAdded)
                        targetHero.gameObject.GetComponent<HeroBehavior>().downFriendship(friendshipAddValue);
                    break;
                case 1:
                    mFriendshipStatus = -1;
                    targetHero.gameObject.GetComponent<HeroBehavior>().downFriendship(mFriendshipRequired);
                    for (int i = 0; i < multiplication + 1; ++i)
                        attackBehavior();
                    break;
            }
            Debug.Log("Life:" + mLifeLeft);
        }
    }

    private void Death()
    {
        Destroy(transform.gameObject);
    }

    private void Respawn()
    {
        Debug.Log("Tree Respawn");
        patrol = true;
        nowATK = 10;
        mLifeLeft = 200;
        transform.localPosition = initialpos;
        transform.right = initialright;
        enemyRenderer.flipX = false;
        //mRigidbody.velocity = new Vector3(0,0,0);
        attackTimer = 0.5f;
        transform.GetChild(0).GetComponent<Renderer>().enabled = false;
    }

    private void patrolBehavior()
    {
        transform.GetChild(1).GetComponent<Renderer>().enabled = false;
        distance = Vector3.Distance(pos, targetpos);
        if (distance <= detectDistance)
        {
            angle = Vector3.Angle(targetpos - pos, transform.right);
            if (angle < detectAngle)
            {
                targetDirection = (targetpos - pos).normalized;
                info = Physics2D.Raycast(transform.localPosition, targetDirection, chaseDistance, 1 << 6 | 1 << 8);
                if (info.collider != null && info.collider.gameObject.layer == 8)
                {
                    patrol = false;
                    transform.GetChild(0).GetComponent<Renderer>().enabled = true;
                }
            }
        }
    }
    private void friendlyBehavior()
    {
        distance = Vector3.Distance(pos, targetpos);
        if (dot < -0.2f)
            transform.right = -transform.right;

        if (!frienshipAdded && distance < detectDistance)
        {
            Debug.Log("Friendship added:" + friendshipAddValue + gameObject.name);
            targetHero.gameObject.GetComponent<HeroBehavior>().upFriendship(friendshipAddValue);
            frienshipAdded = true;
            transform.GetChild(1).GetComponent<Renderer>().enabled = true;
        }
        attackedTimer = 0.5f;
        mRigidbody.velocity = new Vector3(0, 0, 0);
    }
    private void chaseBehavior()
    {
        if (targetHero.GetComponent<HeroBehavior>().IsRespawned())
            Respawn();
        else
        { 
            info = Physics2D.Raycast(transform.localPosition, targetDirection, chaseDistance, 1 << 6 | 1 << 8);
            mRigidbody.velocity = new Vector3(0, 0, 0);
            
            attackTimer += Time.deltaTime;
            atkTimer += Time.deltaTime;

            if(atkTimer >= 1.0f && nowATK <= 30)
            {
                nowATK += 5;
                atkTimer = 0f;
            }

            if (attackTimer >= 1.0f)
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
    private void attackBehavior()
    {
        GameObject remoteAttack = Instantiate(Resources.Load("Prefabs/TreeAttack") as GameObject);
        targetpos = targetHero.transform.localPosition;
        if(targetHero.transform.position.x < transform.position.x)
        {
            enemyRenderer.flipX = true;
        }
        else
        {
            enemyRenderer.flipX = false;
        }

        remoteAttack.transform.localPosition = transform.localPosition;
        remoteAttack.transform.right = (targetpos - transform.localPosition).normalized;
        remoteAttack.gameObject.GetComponent<TreeAttack>().branchDmg = nowATK;
        Debug.Log("EnemyAttacking");
    }

    private void attackedBehavior()
    {
        attackedTimer -= Time.deltaTime;
        //mRigidbody.AddForce(0.1f * attackedTimer * transform.right);
        if (attackedTimer <= 0)
        {
            attackedTimer = 0.5f;
            //anim.SetBool("Attacked", false);
            if (mLifeLeft <= 0)
                Death();
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class EnemyBehavior : MonoBehaviour
{
    protected float waitTimer = 0, attackTimer = 1.5f, attackedTimer = 0.5f,
                    detectDistance = 8f, chaseDistance = 12f, detectAngle = 45f,
                    distance, angle, dot;
    protected int mLifeLeft = 100, mFriendshipRequired = 20, mFriendshipStatus = 0, multiplication = 2, friendshipAddValue = 10;
    protected bool patrol = true, frienshipAdded = false;
    protected SpriteRenderer enemyRenderer;
    protected Rigidbody2D mRigidbody;
    public GameObject targetHero;
    protected Vector3 targetpos, initialpos, initialright, pos, targetDirection, vel;
    protected RaycastHit2D info;
    protected Animator anim;
    protected bool isactive = true;

    protected GameObject muim;
    protected GameObject dropitem;
    protected bool dropped = false;
    
    protected DialogueRunner dialogueRunner;

    protected virtual void Start()
    {
        anim = GetComponent<Animator>();
        enemyRenderer = GetComponent<SpriteRenderer>();
        mRigidbody = gameObject.GetComponent<Rigidbody2D>();
        targetHero = GameObject.Find("hero");

        initialpos = transform.localPosition;
        initialright = transform.right;
        vel = mRigidbody.velocity;
        vel.x = 0f;
        mRigidbody.velocity = transform.right * 2 + vel;
        dialogueRunner = GameObject.Find("Dialogue System").GetComponent<DialogueRunner>();

    }
    protected virtual void Awake()
    {
        //isactive=true;
    }
    // Update is called once per frame
    protected virtual void Update()
    {
        if(mLifeLeft<=0)
            Death();
        if (isactive)
        {
            if (anim.GetBool("Attacked"))
                attackedBehavior();
            else
            {
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
            }
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isactive) return;
        if (patrol && collision.gameObject.layer == 17)
            transform.right = -transform.right;
        vel = mRigidbody.velocity;
        vel.x = 0f;
        mRigidbody.velocity = transform.right * 2 + vel; // Turn back when patrolling

        if (collision.gameObject.layer == 19)
        {
            anim.SetBool("Attacked", true);
            GameObject.Find("hero").GetComponent<AudioManager>().PlayEnemy("EnemyHurt");
            mRigidbody.velocity = new Vector3(0, 0, 0);
            mRigidbody.AddForce(-100 * transform.right);
            mLifeLeft -= collision.gameObject.transform.parent.GetComponent<HeroAttackHurt>().hurt *
            collision.gameObject.transform.parent.GetComponent<HeroAttackHurt>().powerUpCoef;//计算受伤
            switch (mFriendshipStatus)
            {
                case 2:
                    mLifeLeft -= collision.gameObject.transform.parent.GetComponent<HeroAttackHurt>().hurt *
                            collision.gameObject.transform.parent.GetComponent<HeroAttackHurt>().powerUpCoef *
                            (multiplication - 1);
                    mFriendshipStatus = 1;
                    targetHero.gameObject.GetComponent<HeroBehavior>().downFriendship(10);
                    if (frienshipAdded)
                        targetHero.gameObject.GetComponent<HeroBehavior>().downFriendship(friendshipAddValue);
                    dialogueRunner.Stop();
                    dialogueRunner.StartDialogue("FriendlyAttacked");
                    break;
                case 1:
                    mFriendshipStatus = -1;
                    patrol=false;
                    targetHero.gameObject.GetComponent<HeroBehavior>().downFriendship(mFriendshipRequired);
                    transform.GetChild(0).GetComponent<Renderer>().enabled = true;
                    transform.GetChild(1).GetComponent<Renderer>().enabled = false;
                    attackBehavior();
                    if (++targetHero.GetComponent<EndingJudgement>().friendAttacked >= 5)
                    {
                        targetHero.GetComponent<EndingJudgement>().attackFriends = true;
                        targetHero.GetComponent<HeroBehavior>().setFriendship(-6666);
                    }
                    break;
            }
            Debug.Log("Life:" + mLifeLeft);
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("RemoteAttack"))
        {
            anim.SetBool("Attacked", true);
            GameObject.Find("hero").GetComponent<AudioManager>().PlayEnemy("EnemyHurt");
            mRigidbody.velocity = new Vector3(0, 0, 0);
            mRigidbody.AddForce(-100 * transform.right);
            mLifeLeft -= targetHero.gameObject.GetComponent<HeroAttackHurt>().hurt *
                         targetHero.gameObject.GetComponent<HeroAttackHurt>().powerUpCoef;//计算受伤
            switch (mFriendshipStatus)
            {
                case 2:
                    mLifeLeft -= collision.gameObject.transform.parent.GetComponent<HeroAttackHurt>().hurt *
                            collision.gameObject.transform.parent.GetComponent<HeroAttackHurt>().powerUpCoef *
                            (multiplication - 1);
                    mFriendshipStatus = 1;
                    targetHero.gameObject.GetComponent<HeroBehavior>().downFriendship(10);
                    if (frienshipAdded)
                        targetHero.gameObject.GetComponent<HeroBehavior>().downFriendship(friendshipAddValue);
                    dialogueRunner.Stop();
                    dialogueRunner.StartDialogue("FriendlyAttacked");
                    break;
                case 1:
                    mFriendshipStatus = -1;
                    targetHero.gameObject.GetComponent<HeroBehavior>().downFriendship(mFriendshipRequired);
                    transform.GetChild(0).GetComponent<Renderer>().enabled = true;
                    transform.GetChild(1).GetComponent<Renderer>().enabled = false;
                    attackBehavior();
                    if (++targetHero.GetComponent<EndingJudgement>().friendAttacked >= 5)
                    {
                        targetHero.GetComponent<EndingJudgement>().attackFriends = true;
                        targetHero.GetComponent<HeroBehavior>().setFriendship(-6666);
                    }
                    break;
            }
            Debug.Log("Life:" + mLifeLeft);
        }
    }

    protected virtual void Death()
    {
        Destroy(transform.gameObject);
        if (dropped) return;
        dropped = true;
        float rn = Random.Range(0f, 5f);
        if (rn < 1f)
        {
            dropitem = Instantiate(Resources.Load("Prefabs/dropitems/bloodflask") as GameObject);
        }
        else if (rn < 2f)
        {
            dropitem = Instantiate(Resources.Load("Prefabs/dropitems/energyflask") as GameObject);
        }
        else if (rn < 3f)
        {
            dropitem = Instantiate(Resources.Load("Prefabs/dropitems/diamond") as GameObject);
        }
        else
        {
            muim = GameObject.Find("UImanager");
            int mon = (int)(Random.Range(20, 60));
            muim.GetComponent<coincontrol>().earn(mon);
        }
        if (rn < 3)
        {
            Vector3 newpos = transform.position;
            newpos.y += 1;
            dropitem.transform.position = newpos;
        }
    }

    protected virtual void Respawn()
    {
        patrol = true;
        mLifeLeft = 100;//基础生命值以史莱姆为例子
        transform.localPosition = initialpos;
        transform.right = initialright;
        vel = mRigidbody.velocity;
        vel.x = 0f;
        mRigidbody.velocity = transform.right * 2 + vel;
        attackTimer = 0.5f;
        transform.GetChild(0).GetComponent<Renderer>().enabled = false;
    }

    protected virtual void patrolBehavior()
    {
        transform.GetChild(1).GetComponent<Renderer>().enabled = false;
        vel = mRigidbody.velocity;
        vel.x = 0f;
        mRigidbody.velocity = transform.right * 2 + vel;
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

    protected virtual void friendlyBehavior()
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
            if (!dropped)
            {
                dropped = true;
                muim = GameObject.Find("UImanager");
                int mon = (int)(Random.Range(30, 60));
                muim.GetComponent<coincontrol>().earn(mon);
            }
        }
        attackedTimer = 0.5f;
        mRigidbody.velocity = new Vector3(0, 0, 0);
    }

    protected virtual void chaseBehavior()
    {
        if (targetHero.GetComponent<HeroBehavior>().IsRespawned())
            Respawn();
        else
        {
            info = Physics2D.Raycast(transform.localPosition, targetDirection, chaseDistance, 1 << 6 | 1 << 8);
            if (dot < -0.2f)
                transform.right = -transform.right;
            if (dot < -0.2f || dot > 0.2f)
            {
                vel = mRigidbody.velocity;
                vel.x = Mathf.Min(Mathf.Abs(Vector3.Dot(transform.right, targetDirection)) * 2, transform.right.x * 4);
                mRigidbody.velocity = vel;
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
    protected virtual void attackBehavior()
    {
        GameObject remoteAttack = Instantiate(Resources.Load("Prefabs/BulletScreen") as GameObject);
        targetpos = targetHero.transform.localPosition;
        remoteAttack.transform.localPosition = transform.localPosition;
        remoteAttack.transform.up = (targetpos - transform.localPosition).normalized;
        Debug.Log("EnemyAttacking");
    }

    protected virtual void attackedBehavior()
    {
        attackedTimer -= Time.deltaTime;
        mRigidbody.AddForce(0.1f * attackedTimer * transform.right);
        if (attackedTimer <= 0)
        {
            attackedTimer = 0.5f;
            anim.SetBool("Attacked", false);
            if (mLifeLeft <= 0)
                Death();
        }
    }

    protected virtual void CloseCombact()
    {
        transform.GetChild(2).gameObject.SetActive(true);
    }

}
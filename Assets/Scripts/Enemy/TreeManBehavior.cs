using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class TreeManBehavior : MonoBehaviour
{
    private float waitTimer = 0, attackTimer = 1.0f, attackedTimer = 0.5f,
                    detectDistance = 8f, chaseDistance = 12f, detectAngle = 70f,
                    distance, angle, dot;
    private int mLifeLeft = 200, mFriendshipRequired = 20, mFriendshipStatus = 0, multiplication = 2, friendshipAddValue = 10;
    private bool patrol = true, frienshipAdded = false;
    private SpriteRenderer enemyRenderer;
    private Rigidbody2D mRigidbody;
    public GameObject targetHero;
    private Vector3 targetpos, initialpos, initialright, pos, targetDirection, vel;
    private RaycastHit2D info;
    private Animator anim;
    private bool isactive = true;
    private bool dropped = false;
    private GameObject dropitem;
    private GameObject muim;
    private DialogueRunner dialogueRunner;

    public int nowATK = 10;
    private float atkTimer = 0f;

    private void Start()
    {
        anim = GetComponent<Animator>();
        enemyRenderer = GetComponent<SpriteRenderer>();
        mRigidbody = gameObject.GetComponent<Rigidbody2D>();
        targetHero = GameObject.Find("hero");
        initialpos = transform.position;
        initialright = transform.right;
        dialogueRunner = GameObject.Find("Dialogue System").GetComponent<DialogueRunner>();
    }
    private void Update()
    {
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

                pos = transform.position;
                targetpos = targetHero.transform.position;
                targetDirection = targetpos - pos;
                dot = Vector3.Dot(transform.right, targetDirection.normalized);

                if (dot > 0.2f)
                {
                    transform.right = -transform.right;
                }

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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isactive) return;

        if (collision.gameObject.layer == LayerMask.NameToLayer("PlayerBullet"))
        {
            anim.SetBool("Attacked", true);

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
                    patrol = false;
                    targetHero.gameObject.GetComponent<HeroBehavior>().downFriendship(mFriendshipRequired);
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

    private void Death()
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

    private void Respawn()
    {
        Debug.Log("Tree Respawn");
        patrol = true;
        nowATK = 10;
        mLifeLeft = 200;
        transform.position = initialpos;
        transform.right = initialright;
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
            //angle = Vector3.Angle(targetpos - pos, transform.right);
            //if (angle < detectAngle)
            //{
            targetDirection = (targetpos - pos).normalized;
            info = Physics2D.Raycast(transform.position, targetDirection, chaseDistance, 1 << 6 | 1 << 8);
            if (info.collider != null && info.collider.gameObject.layer == 8)
            {
                patrol = false;
                transform.GetChild(0).GetComponent<Renderer>().enabled = true;
            }
            //}
        }
    }
    private void friendlyBehavior()
    {
        distance = Vector3.Distance(pos, targetpos);

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
    private void chaseBehavior()
    {
        if (targetHero.GetComponent<HeroBehavior>().IsRespawned())
            Respawn();
        else
        {
            info = Physics2D.Raycast(transform.position, targetDirection, chaseDistance, 1 << 6 | 1 << 8);
            mRigidbody.velocity = new Vector3(0, 0, 0);

            attackTimer += Time.deltaTime;
            atkTimer += Time.deltaTime;

            if (atkTimer >= 1.0f && nowATK <= 30)
            {
                nowATK += 5;
                atkTimer = 0f;
            }

            if (attackTimer >= 1.0f)
            {
                anim.SetTrigger("Attacking");
                Invoke("attackBehavior", 0.4f);
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
        targetpos = targetHero.transform.position;
        remoteAttack.transform.position = transform.position;
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
            anim.SetBool("Attacked", false);
            if (mLifeLeft <= 0)
                Death();
        }
    }

}

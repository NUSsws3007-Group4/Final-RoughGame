using UnityEngine;
using System.Threading;
public class EliteEnemyBehavior : EnemyBehavior
{
    private float remoteAttackTimer = 0f;
    private bool edgeTouched = false, flaskunlocked = false;
    private float cx1, cx2, cy1, cy2;
    private Vector3 spawnPoint;
    private GameObject guardPortal;
    private bagmanager bgm;
    // Start is called before the first frame update
    protected override void Start()
    {
        anim = GetComponent<Animator>();
        enemyRenderer = GetComponent<SpriteRenderer>();
        mRigidbody = gameObject.GetComponent<Rigidbody2D>();
        targetHero = GameObject.Find("hero");
        guardPortal = GameObject.Find("portal");
        detectDistance = 5f;
        chaseDistance = 20f;
        mFriendshipRequired = 70;
        friendshipAddValue = 0;
        mLifeLeft = 250;
        initialpos = transform.localPosition;
        initialright = transform.right;
        bgm = GameObject.Find("Canvas").GetComponent<bagmanager>();
        guardPortal.SetActive(false);
    }
    protected override void attackedBehavior()
    {
        transform.GetChild(3).gameObject.SetActive(true);
        transform.GetChild(4).gameObject.SetActive(true);
        mRigidbody.velocity = new Vector3(0, 0, 0);
        if (!edgeTouched)
            mRigidbody.AddForce(-100 * transform.right);

        attackedTimer -= Time.deltaTime;
        if (attackedTimer <= 0)
        {
            attackedTimer = 0.5f;
            anim.SetBool("Attacked", false);
            transform.GetChild(3).gameObject.SetActive(false);
            transform.GetChild(4).gameObject.SetActive(false);
            if (mLifeLeft <= 0)
                Death();
        }
    }
    protected override void attackBehavior()
    {
        Bounds b1 = transform.GetChild(2).GetComponent<BoxCollider2D>().bounds;
        Bounds b2 = targetHero.GetComponent<BoxCollider2D>().bounds;
        cx1 = Mathf.Max(b1.min.x, b2.min.x);
        cy1 = Mathf.Max(b1.min.y, b2.min.y);
        cx2 = Mathf.Min(b1.max.x, b2.max.x);
        cy2 = Mathf.Min(b1.max.y, b2.max.y);
        if (!((cx1 > cx2) || (cy1 > cy2)))
        {
            Invoke("CloseCombact", 0.2f);
            anim.SetTrigger("Attacking");
            attackTimer = 0f;

        }
        else if (remoteAttackTimer >= 6.0f)
        {
            Invoke("CloseCombact", 0.2f);
            anim.SetTrigger("Attacking");
            remoteAttackTimer = 0f;
            attackTimer = 0f;
            spawnPoint = transform.localPosition;
            spawnPoint.y += 0.6f;
            spawnPoint.x += 0.4f * transform.right.x;
            Invoke("GenerateRemote", 0.2f);


        }
    }
    protected override void patrolBehavior()
    {
        transform.GetChild(1).GetComponent<Renderer>().enabled = false;
        Debug.Log("patrolliing...");
        transform.GetChild(0).GetComponent<Renderer>().enabled = false;
        distance = Vector3.Distance(pos, targetpos);
        if (distance <= detectDistance)
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

    protected override void chaseBehavior()
    {

        if (targetHero.GetComponent<HeroBehavior>().IsRespawned())
            Invoke("Respawn", 0.2f);
        else
        {
            distance = Vector3.Distance(pos, targetpos);
            info = Physics2D.Raycast(transform.localPosition, targetDirection, chaseDistance, 1 << 6 | 1 << 8);
            if (dot < -0.2f)
            {
                transform.right = -transform.right;
                edgeTouched = false;
            }
            if (!edgeTouched && distance > 1f)
            {
                anim.SetBool("Walking", true);
                vel = mRigidbody.velocity;
                vel.x = Mathf.Min(Mathf.Abs(Vector3.Dot(transform.right, targetDirection)) * 2, transform.right.x * 4);
                mRigidbody.velocity = vel;
            }
            else
            {
                anim.SetBool("Walking", false);
                mRigidbody.velocity = new Vector3(0, 0, 0);
            }
            attackTimer += Time.deltaTime;
            remoteAttackTimer += Time.deltaTime;
            if (attackTimer >= 2.0f)
                attackBehavior();

            if (info.collider == null || (info.collider != null && info.collider.gameObject.layer != 8))
            {
                waitTimer += Time.deltaTime;
                if (waitTimer >= 15f)
                {
                    waitTimer = 0;
                    patrol = true;
                    anim.SetBool("Walking", false);
                }
            }
            else
                waitTimer = 0;
        }
    }
    protected override void friendlyBehavior()
    {
        base.friendlyBehavior();
        if (distance < detectDistance)
        {
            guardPortal.SetActive(true);
            if (!flaskunlocked)
            {
                bgm.friendshipflask.locked = false;
                bgm.pickupitem(bgm.friendshipflask);
                flaskunlocked = true;
            }
        }

    }
    protected override void Death()
    {
        guardPortal.SetActive(true);
        bgm.friendshipflask.locked = false;
        bgm.pickupitem(bgm.friendshipflask);
        base.Death();
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 17)
            edgeTouched = true;

        if (collision.gameObject.layer == 19)
        {
            anim.SetBool("Attacked", true);
            if (!edgeTouched)
            {
                mRigidbody.velocity = new Vector3(0, 0, 0);
                mRigidbody.AddForce(-100 * transform.right);
            }
            mLifeLeft -= collision.transform.parent.gameObject.GetComponent<HeroAttackHurt>().hurt *
                         collision.transform.parent.gameObject.GetComponent<HeroAttackHurt>().powerUpCoef;
            switch (mFriendshipStatus)
            {
                case 2:
                    mLifeLeft -= collision.transform.parent.gameObject.GetComponent<HeroAttackHurt>().hurt *
                                collision.transform.parent.gameObject.GetComponent<HeroAttackHurt>().powerUpCoef *
                                (multiplication - 1);
                    mFriendshipStatus = 1;
                    targetHero.gameObject.GetComponent<HeroBehavior>().downFriendship(10);
                    if (frienshipAdded)
                        targetHero.gameObject.GetComponent<HeroBehavior>().downFriendship(friendshipAddValue);
                    break;
                case 1:
                    mFriendshipStatus = -1;
                    targetHero.gameObject.GetComponent<HeroBehavior>().downFriendship(mFriendshipRequired);
                    break;
            }
            Debug.Log("Life:" + mLifeLeft);
        }
    }

    protected override void Respawn()
    {
        Debug.Log("Elite Respawn" + initialpos);
        edgeTouched = false;
        patrol = true;
        mLifeLeft = 250;
        mRigidbody.velocity = new Vector3(0, 0, 0);
        transform.localPosition = initialpos;
        transform.right = initialright;
        attackTimer = 0.5f;
        transform.GetChild(0).GetComponent<Renderer>().enabled = false;
        anim.SetBool("Walking", false);
    }

    private void GenerateRemote()
    {
        for (int i = 0; i < 3; ++i)
        {
            GameObject remoteAttack = Instantiate(Resources.Load("Prefabs/Elite1Remote") as GameObject);
            remoteAttack.transform.localPosition = spawnPoint;
            remoteAttack.transform.right = transform.right;
            spawnPoint.y -= 0.6f;
            spawnPoint.x -= 0.2f * transform.right.x;
        }
    }
}

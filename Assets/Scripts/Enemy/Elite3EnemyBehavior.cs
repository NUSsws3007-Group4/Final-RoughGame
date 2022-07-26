using UnityEngine;
using Yarn.Unity;
public class Elite3EnemyBehavior : EnemyBehavior
{
    private float remoteAttackTimer = 0f;
    private bool edgeTouched = false;
    private float cx1, cx2, cy1, cy2, dialogueTimer = 10f;
    private int blinkCount = 0;
    private Vector3 spawnPoint;
    private bagmanager bgm;
    private GameObject guardDoor;
    private bool temp = false, dialogueTriggered = false;
    // Start is called before the first frame update
    protected override void Start()
    {
        anim = GetComponent<Animator>();
        enemyRenderer = GetComponent<SpriteRenderer>();
        mRigidbody = gameObject.GetComponent<Rigidbody2D>();
        targetHero = GameObject.Find("hero");
        guardDoor = GameObject.Find("Door");
        detectDistance = 5f;
        chaseDistance = 20f;
        mFriendshipRequired = 70;
        friendshipAddValue = 0;
        mLifeLeft = 500;
        initialpos = transform.localPosition;
        initialright = transform.right;
        bgm = GameObject.Find("Canvas").GetComponent<bagmanager>();
        guardDoor.SetActive(true);
        dialogueRunner = GameObject.Find("Dialogue System").GetComponent<DialogueRunner>();
    }

    protected override void Update()
    {
        base.Update();
        if (temp)
        {
            dialogueTimer += Time.deltaTime;
            if (dialogueTimer >= 2f)
            {
                dialogueTriggered = true;
            }
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
            anim.SetTrigger("AttackingNear");
            attackTimer = 0f;

        }
        else if (remoteAttackTimer >= 6.0f)
        {
            anim.SetTrigger("AttackingRemote");
            remoteAttackTimer = 0f;
            attackTimer = 0f;
            spawnPoint = transform.localPosition;
            spawnPoint.y += 0.3f;
            spawnPoint.x += 0.2f * transform.right.x;
            Invoke("GenerateRemote", 0.9f);
        }
    }
    protected override void attackedBehavior()
    {
        attackedTimer += Time.deltaTime;
        if (attackedTimer >= 0.2f)
        {
            Color c = gameObject.GetComponent<SpriteRenderer>().color;
            if (c.a == 1)
                c.a = 0.6f;
            else c.a = 1f;
            gameObject.GetComponent<SpriteRenderer>().color = c;
            attackedTimer = 0;
            blinkCount++;
        }
        if (blinkCount == 4)
        {
            anim.SetBool("Attacked", false);
            blinkCount = 0;
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
            if (!edgeTouched && distance > 2f)
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
            if (dialogueTimer >= 5f)
            {
                dialogueRunner.Stop();
                if (!dialogueTriggered)
                {
                    dialogueRunner.StartDialogue("Elite3Friendly");
                    temp = true;
                }
                else
                    dialogueRunner.StartDialogue("Elite3Friendly2");
                dialogueTimer = 0f;
            }
        }
    }
    protected override void Death()
    {
        dialogueRunner.Stop();
        bgm.pickupitem(bgm.scroll3);
        if (dialogueTriggered)
        {
            dialogueRunner.StartDialogue("Elite3Defeated2");
        }
        else
        {
            muim = GameObject.Find("UImanager");
            int mon = (int)(Random.Range(500, 1000));
            muim.GetComponent<coincontrol>().earn(mon);
            dialogueRunner.StartDialogue("Elite3Defeated");
        }
        targetHero.GetComponent<EndingJudgement>().f3 = false;
        Destroy(guardDoor.gameObject);
        Destroy(transform.gameObject);

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
                    dialogueRunner.Stop();
                    dialogueRunner.StartDialogue("FriendlyAttacked");
                    break;
                case 1:
                    mFriendshipStatus = -1;
                    targetHero.gameObject.GetComponent<HeroBehavior>().downFriendship(mFriendshipRequired);
                    transform.GetChild(0).GetComponent<Renderer>().enabled = true;
                    transform.GetChild(1).GetComponent<Renderer>().enabled = false;
                    patrol = false;
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
            if (!edgeTouched)
            {
                mRigidbody.velocity = new Vector3(0, 0, 0);
                mRigidbody.AddForce(-100 * transform.right);
            }
            mLifeLeft -= targetHero.gameObject.GetComponent<HeroAttackHurt>().hurt *
                         targetHero.gameObject.GetComponent<HeroAttackHurt>().powerUpCoef;
            switch (mFriendshipStatus)
            {
                case 2:
                    mLifeLeft -= targetHero.gameObject.GetComponent<HeroAttackHurt>().hurt *
                                 targetHero.gameObject.GetComponent<HeroAttackHurt>().powerUpCoef *
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
                    transform.GetChild(0).GetComponent<Renderer>().enabled = true;
                    transform.GetChild(1).GetComponent<Renderer>().enabled = false;
                    if (++targetHero.GetComponent<EndingJudgement>().friendAttacked >= 5)
                    {
                        targetHero.GetComponent<EndingJudgement>().attackFriends = true;
                        targetHero.GetComponent<HeroBehavior>().setFriendship(-6666);
                    }
                    break;
            }
        }
    }

    protected override void Respawn()
    {
        Debug.Log("Elite Respawn" + initialpos);
        edgeTouched = false;
        patrol = true;
        mLifeLeft = 500;
        mRigidbody.velocity = new Vector3(0, 0, 0);
        transform.localPosition = initialpos;
        transform.right = initialright;
        attackTimer = 0.5f;
        transform.GetChild(0).GetComponent<Renderer>().enabled = false;
        anim.SetBool("Walking", false);
    }

    private void GenerateRemote()
    {
        for (int i = 0; i < 2; ++i)
        {
            GameObject remoteAttack = Instantiate(Resources.Load("Prefabs/Elite3Remote") as GameObject);
            remoteAttack.transform.localPosition = spawnPoint;
            remoteAttack.transform.right = transform.right;
            spawnPoint.y -= 1.0f;
        }
        spawnPoint.y += 1.5f;
        spawnPoint.x += 0.3f * transform.right.x;
        Invoke("GenerateCentralRemote", 0.15f);
    }

    private void GenerateCentralRemote()
    {
        GameObject remoteAttack = Instantiate(Resources.Load("Prefabs/Elite3Remote") as GameObject);
        remoteAttack.transform.localPosition = spawnPoint;
        remoteAttack.transform.right = transform.right;
    }

    public void AllowPass()
    {
        targetHero.GetComponent<EndingJudgement>().f3 = true;
        bgm.pickupitem(bgm.scroll3);
        dialogueRunner.Stop();
        dialogueRunner.StartDialogue("Elite3AllowPass");
        muim = GameObject.Find("UImanager");
        muim.GetComponent<coincontrol>().earn(1000);
        Destroy(guardDoor.gameObject);
    }
}

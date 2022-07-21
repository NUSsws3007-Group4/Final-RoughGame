using UnityEngine;

public class EliteEnemyBehavior : EnemyBehavior
{
    private float remoteAttackTimer = 0f;
    private bool edgeTouched = false;
    // Start is called before the first frame update
    override protected void Start()
    {
        anim = GetComponent<Animator>();
        enemyRenderer = GetComponent<SpriteRenderer>();
        mRigidbody = gameObject.GetComponent<Rigidbody2D>();
        targetHero = GameObject.Find("hero");
        detectDistance = 2f;
        initialpos = transform.localPosition;
        initialright = transform.right;
    }

    override protected void patrolBehavior()
    {
        Debug.Log("patrolliing...");
        transform.GetChild(1).GetComponent<Renderer>().enabled = false;
        distance = Vector3.Distance(pos, targetpos);
        if (distance <= detectDistance)
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

    override protected void chaseBehavior()
    {

        if (targetHero.GetComponent<HeroBehavior>().IsRespawned())
            Respawn();
        else
        {
            info = Physics2D.Raycast(transform.localPosition, targetDirection, chasedistance, 1 << 6 | 1 << 8);
            if (dot < -0.2f)
            {
                transform.right = -transform.right;
                edgeTouched = false;
            }
            if (!edgeTouched && (dot < -0.2f || dot > 0.2f))
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
                    patrol = true;
                    anim.SetBool("Walking", false);
                }
            }
            else
                waitTimer = 0;
        }
    }
    override protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 17)
            edgeTouched = true;

        if (collision.gameObject.layer == 19)
        {
            anim.SetBool("Attacked", true);
            mRigidbody.velocity = new Vector3(0, 0, 0);
            mRigidbody.AddForce(-100 * transform.right);
            mLifeLeft -= collision.gameObject.GetComponent<HeroAttackHurt>().hurt;
            switch (mFriendshipStatus)
            {
                case 2:
                    for (int i = 0; i < multiplication - 1; ++i)
                        mLifeLeft -= collision.gameObject.GetComponent<HeroAttackHurt>().hurt;
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

    override protected void Respawn()
    {
        Debug.Log("Elite Respawn" + initialpos);
        edgeTouched = false;
        patrol = true;
        mLifeLeft = 4;
        mRigidbody.velocity = new Vector3(0, 0, 0);
        transform.localPosition = initialpos;
        transform.right = initialright;
        attackTimer = 0.5f;
        transform.GetChild(0).GetComponent<Renderer>().enabled = false;
        anim.SetBool("Walking", false);
    }
}

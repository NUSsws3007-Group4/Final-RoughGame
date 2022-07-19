using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    protected float timeCount = 0;
    protected float AttackTime = 1.5f;
    protected float enemySpeed = 4f;
    protected int mLifeLeft = 100;
    protected bool patrol = true;
    protected float detectDistance = 8f, chasedistance = 12f, detectAngle = 45f, distance, angle;
    protected float dot;
    protected SpriteRenderer enemyRenderer;
    public GameObject targethero;
    protected Vector3 targetpos, initialpos, initialright, pos;


    void Start()
    {
        enemyRenderer = GetComponent<SpriteRenderer>();
        initialpos = transform.localPosition;
        initialright = transform.right;
    }

    // Update is called once per frame
    void Update()
    {
        if (mLifeLeft <= 0)
            Death();
        transform.localPosition += enemySpeed * transform.right * Time.smoothDeltaTime;
        pos = transform.localPosition;
        targetpos = targethero.transform.localPosition;
        distance = Vector3.Distance(pos, targetpos);
        if (distance <= detectDistance)
        {
            angle = Vector3.Angle(targetpos - pos, transform.right);
            if (angle < detectAngle)
            {
                patrol = false;
                transform.GetChild(0).GetComponent<Renderer>().enabled = true;
            }
        }

        if (!patrol)
        {
            dot = Vector3.Dot(transform.right, (targetpos - pos).normalized);
            if (-0.1f <= dot && dot <= 0.1f)
                enemySpeed = 0f;
            else
            {
                enemySpeed = 4f;
                if (dot < -0.1f)
                {
                    transform.right = -transform.right;
                }
            }
            AttackTime += Time.deltaTime;

            if (AttackTime >= 2.0f)
            {
                eject();
                AttackTime = 0f;
            }

            if (targethero.GetComponent<HeroMovement>().IsRespawned() || distance > chasedistance)
            {
                enemySpeed = 4f;
                patrol = true;
                mLifeLeft = 100;
                transform.localPosition = initialpos;
                transform.right = initialright;
                AttackTime = 1.5f;
                transform.GetChild(0).GetComponent<Renderer>().enabled = false;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (patrol && collision.gameObject.layer == 17)
            transform.right = -transform.right;
    }

    void Death()
    {
        //TODO: 爆金币
        Destroy(transform.gameObject);
    }
    protected virtual void eject()
    {
        GameObject remoteAttack = Instantiate(Resources.Load("Prefabs/BulletScreen") as GameObject);
        targetpos = targethero.transform.localPosition;
        remoteAttack.transform.localPosition = transform.localPosition;
        remoteAttack.transform.up = (targetpos - transform.localPosition).normalized;
    }

}

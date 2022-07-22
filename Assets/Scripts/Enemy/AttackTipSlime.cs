using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTipSlime : SlimeBehavior
{
    public GameObject s;
    public GameObject attackimage;
    public GameObject friendlyimage;

    void Awake()
    {
        attackimage.SetActive(false);
    }

    override protected void Start()
    {
        isactive=false;
        base.Start();
        mRigidbody.velocity = new Vector3(0, 0, 0);
        s.SetActive(false);
    }
    protected override void patrolBehavior()
    {
        patrol = false;
        transform.GetChild(0).GetComponent<Renderer>().enabled = true;
    }
    protected override void chaseBehavior()
    {
        base.chaseBehavior();
        mRigidbody.velocity = vel / 10;
    }
    protected override void Death()
    {
        s.SetActive(true);
        friendlyimage.SetActive(true);
        s.GetComponent<FriendshipTipBehavior>().timer=5f;
        base.Death();
    }

    override protected void Update()
    {
        base.Update();
        if(targetHero.GetComponent<tutorial>().state>2)
        {
            isactive=true;
            attackimage.SetActive(true);
        }
        if(mLifeLeft<4)
        {
            attackimage.SetActive(false);
        }
    }
}
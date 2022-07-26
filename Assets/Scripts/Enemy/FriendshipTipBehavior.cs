using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendshipTipBehavior : SlimeBehavior
{
    public GameObject friendlyimage;
    public float timer;

    override protected void Start()
    {
        //friendlyimage.SetActive(false);
        timer = 5f;
        base.Start();
        mRigidbody.velocity = new Vector3(0, 0, 0);
        mFriendshipRequired = 0;
        friendshipAddValue = 60;
    }
    protected override void patrolBehavior()
    {
        Debug.Log("Slime patrolling");
    }
    protected override void friendlyBehavior()
    {
        base.friendlyBehavior();
        if (frienshipAdded)
        {
            Invoke("Death1", 15);
        }
    }
    protected override void Update()
    {
        base.Update();
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = 0;
                friendlyimage.SetActive(false);
            }
        }
    }
    protected override void Death()
    {
        targetHero.GetComponent<EndingJudgement>().friendshipTipSlimeKilled = true;
        base.Death();
    }
    protected void Death1(){
        Destroy(transform.gameObject);
    }
}

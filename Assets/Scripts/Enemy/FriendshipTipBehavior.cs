using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendshipTipBehavior : SlimeBehavior
{
    public GameObject s1, s2;
    override protected void Start()
    {
        base.Start();
        mRigidbody.velocity = new Vector3(0, 0, 0);
        mFriendshipRequired = 0;
        friendshipAddValue = 50;
        s1.SetActive(false);
        s2.SetActive(false);
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
            mFriendshipRequired = 20;
            s1.SetActive(true);
            s2.SetActive(true);
            Invoke("Death", 15);
        }
    }

}

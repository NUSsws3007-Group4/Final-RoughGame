using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendshipTipBehavior : SlimeBehavior
{
    override protected void Start()
    {
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
            Invoke("Death", 15);
        }
    }

}

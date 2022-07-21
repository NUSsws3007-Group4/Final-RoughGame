using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTipSlime : SlimeBehavior
{
    public GameObject s;
    override protected void Start()
    {
        base.Start();
        mRigidbody.velocity = new Vector3(0, 0, 0);
        
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
}

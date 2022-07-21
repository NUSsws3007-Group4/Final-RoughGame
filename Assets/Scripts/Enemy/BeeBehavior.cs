using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeBehavior : EnemyBehavior
{
    private float beeSpeed = 15f;
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (patrol && collision.gameObject.layer == 17)
            transform.right = -transform.right;

        if(collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            mRigidbody.velocity = new Vector3(0, 0, 0);
            anim.SetBool("Explode", true);
            Invoke("Suicide", 0.4f);
        }
        if(collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            mRigidbody.velocity = new Vector3(0, 0, 0);
            anim.SetBool("Explode", true);
            Invoke("Suicide", 0.4f);
        }
    }

    protected override void attackBehavior()
    {
        targetpos = targetHero.transform.localPosition;
        targetpos.z = 0f;
        transform.right = targetpos - transform.localPosition;

        transform.localPosition += transform.right * Time.smoothDeltaTime * beeSpeed;
    }

    private void Suicide()
    {
        Destroy(transform.gameObject);
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeBehavior : MonoBehaviour
{

    private float distance, detectDistance = 8f, chaseDistance = 12f, detectAngle = 90f, angle;
    private bool patrol = true;
    private Vector3  originRight, targetPos, targetDirection;
    private SpriteRenderer enemyRenderer;
    private Rigidbody2D mRigidbody;
    private RaycastHit2D info;
    public GameObject targetHero;
    private Animator anim;
    private float beeSpeed = 8f;
    private float patrolSpeed = 4f;
    private void Start() 
    {
        anim = GetComponent<Animator>();
        enemyRenderer = GetComponent<SpriteRenderer>();
        mRigidbody = gameObject.GetComponent<Rigidbody2D>();
        targetHero = GameObject.Find("hero");
        originRight = transform.right;

    }

    private void Update()
    {
        targetPos = targetHero.transform.position;

        if(patrol)
        {
            transform.right = originRight;
            patrolBehavior();
        }
        else
        {
            chaseAndAttack();
        }
    }
    
    private void patrolBehavior()
    {
        Vector3 pos = transform.position;
        pos += transform.right * patrolSpeed * Time.smoothDeltaTime;
        transform.position = pos;

        distance = Vector3.Distance(transform.position, targetHero.transform.position);
        if (distance <= detectDistance)
        {
            angle = Vector3.Angle(targetPos - pos, transform.right);
            if (angle < detectAngle)
            {
                targetDirection = (targetPos - pos).normalized;
                info = Physics2D.Raycast(transform.localPosition, targetDirection, chaseDistance, 1 << 6 | 1 << 8);
                if (info.collider != null && info.collider.gameObject.layer == 8)
                {
                    patrol = false;
                    transform.GetChild(0).GetComponent<Renderer>().enabled = true;
                }
            }
        }
    }

    private void chaseAndAttack()
    {
        Vector3 rushVector = targetPos - transform.position;
        rushVector.z = 0f;
        transform.right = rushVector;

        transform.position += transform.right * Time.smoothDeltaTime * beeSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
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

    private void Suicide()
    {
        Destroy(transform.gameObject);
    }
    
}

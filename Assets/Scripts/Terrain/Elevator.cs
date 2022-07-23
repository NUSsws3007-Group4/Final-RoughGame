using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public bool MoveUpDown;
    public static float Speed = 3f;
    public GameObject mPlayer;
    private Vector3 offSet, originPos;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Rigidbody2D>().velocity = MoveUpDown ? Vector2.up * Speed : Vector2.left * Speed;
        mPlayer = GameObject.Find("hero");
    }

    // Update is called once per frame
    void Update()
    {
        if(!MoveUpDown)//是左右移动
        {
            Vector3 pos = transform.position;
            offSet = pos - originPos;
            mPlayer.transform.position += offSet;

            originPos = pos;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("WayPoint"))
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = -gameObject.GetComponent<Rigidbody2D>().velocity;
        }
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            originPos = transform.position;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            HeroBehavior hero;
            if(hero = collision.gameObject.GetComponent<HeroBehavior>())
            {
                Debug.Log("change offset");
                hero.setOffsetSpeed(gameObject.GetComponent<Rigidbody2D>().velocity.x);
            }
        }
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            HeroBehavior hero;
            if (hero = collision.gameObject.GetComponent<HeroBehavior>())
            {
                hero.setOffsetSpeed(gameObject.GetComponent<Rigidbody2D>().velocity.x);
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            HeroBehavior hero;
            if (hero = collision.gameObject.GetComponent<HeroBehavior>())
            {
                hero.setOffsetSpeed(0);
            }
        }
    }
}

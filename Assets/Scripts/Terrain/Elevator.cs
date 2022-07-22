using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingItem : MonoBehaviour
{
    public bool MoveUpDown, stayCollision;
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
        if(!MoveUpDown && stayCollision)//是左右移动 且 玩家与平台在一起
        {
            Vector3 pos = transform.position;
            offSet = pos - originPos;
            mPlayer.transform.position += offSet;

            originPos = pos;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("WayPoint"))
        {
            gameObject.GetComponent<Rigidbody2D>().velocity = -gameObject.GetComponent<Rigidbody2D>().velocity;
        }
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            originPos = transform.position;
            stayCollision = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            stayCollision = false;
        }
    }
}

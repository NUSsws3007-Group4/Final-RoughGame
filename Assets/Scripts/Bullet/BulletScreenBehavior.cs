using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScreenBehavior : MonoBehaviour
{
    private float attackSpeed = 8f;
    private GameManager mb;
    private Vector3 mineup;

    private void rot()
    {
        gameObject.transform.Rotate(gameObject.transform.forward * 800f * Time.deltaTime);
    }

    void Start()
    {
        mb = GameObject.Find("GameManager").GetComponent<GameManager>();
        mineup = transform.up;
    }

    // Update is called once per frame
    void Update()
    {   //自动飞行
        rot();
        Vector3 p = transform.localPosition;
        p += attackSpeed * mineup * Time.smoothDeltaTime;
        transform.localPosition = p;
        //出界消除
        CameraSupport s = Camera.main.GetComponent<CameraSupport>();
        if (s != null)
        {
            Bounds myBound = GetComponent<Renderer>().bounds;
            CameraSupport.WorldBoundStatus status = s.CollideWorldBound(myBound);

            if (status == CameraSupport.WorldBoundStatus.Outside)
            {
                Debug.Log("Touching the world edge: " + status);
                Destroy(transform.gameObject);
            }
        }

        if (!mb.boundscontainspoint(mb.camerabounds, transform.localPosition))
        {
            Destroy(transform.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer!=10)
        {
            Destroy(transform.gameObject);
        }
    }
}

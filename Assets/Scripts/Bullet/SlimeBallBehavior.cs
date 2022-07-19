using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBallBehavior : MonoBehaviour
{
    private float attackSpeed = 8f;
    private Animator anim;
    private GameManager mb;
    public GameObject muim;
    private Vector3 mineup;

    private void rot()
    {
        gameObject.transform.Rotate(gameObject.transform.forward * 800f * Time.deltaTime);
    }

    void Start()
    {
        Debug.Log("Ejected");
        mb = GameObject.Find("GameManager").GetComponent<GameManager>();
        mineup = transform.up;
        anim = GetComponent<Animator>();
    }


    // Update is called once per frame
    void Update()
    {   //自动飞行
        AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);
        if (info.normalizedTime >= 1 && info.IsName("Explosion"))
            Destroy(transform.gameObject);
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
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != 10 && collision.gameObject.layer != 17)
        {
            anim.SetBool("ToDestroy", true);
            attackSpeed = 0f;
            Debug.Log(collision.gameObject.layer);
        }
    }
    public void Destroy() { Destroy(transform.gameObject); }
}
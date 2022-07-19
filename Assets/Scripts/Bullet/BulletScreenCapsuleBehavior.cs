using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScreenCapsuleBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    const float pi = 3.1416f;
    private float speed;
    private float explsiondis;
    private Vector3 heropos;
    private Vector3 mineup;
    private Vector3 tvec, direction;
    private GameManager mb;

    private void rot()
    {
        gameObject.transform.Rotate(gameObject.transform.forward * 800f * Time.deltaTime);
    }

    void Start()
    {
        speed = 10f;
        explsiondis = 50f;
        mb = GameObject.Find("GameManager").GetComponent<GameManager>();
        mineup = transform.up;
    }

    // Update is called once per frame
    void Update()
    {
        rot();
        transform.localPosition += mineup * speed * Time.deltaTime;
        heropos = mb.getheropos();
        if ((heropos - transform.localPosition).sqrMagnitude <= explsiondis)
        {
            GameObject remoteAttack;
            for (float i = -30f; i <= 30f; i += 15f)
            {
                remoteAttack = Instantiate(Resources.Load("Prefabs/BulletScreen") as GameObject);
                remoteAttack.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                remoteAttack.transform.localPosition = transform.localPosition;

                tvec = heropos - transform.localPosition;
                direction.x = tvec.x * Mathf.Cos(i / 180f * pi) + tvec.y * Mathf.Sin(i / 180f * pi);
                direction.y = -tvec.x * Mathf.Sin(i / 180f * pi) + tvec.y * Mathf.Cos(i / 180f * pi);
                direction.z = 0f;
                remoteAttack.transform.up = direction.normalized;
            }
            Destroy(transform.gameObject);
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

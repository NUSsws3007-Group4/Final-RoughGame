using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBulletBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    private float speed;
    private float explsiondis;
    private Vector3 heropos;
    private Vector3 mineup;
    private GameManager mb;

    private void rot()
    {
        gameObject.transform.Rotate(gameObject.transform.forward * 800f * Time.deltaTime);
    }

    void Start()
    {
        speed = 10f;
        explsiondis = 30f;
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
            GameObject newremoteAttack = Instantiate(Resources.Load("Prefabs/CruiseBullet") as GameObject);
            newremoteAttack.transform.localPosition = transform.localPosition;
            newremoteAttack.transform.up = Vector3.up;

            newremoteAttack = Instantiate(Resources.Load("Prefabs/CruiseBullet") as GameObject);
            newremoteAttack.transform.localPosition = transform.localPosition;
            newremoteAttack.transform.up = Vector3.down;

            newremoteAttack = Instantiate(Resources.Load("Prefabs/CruiseBullet") as GameObject);
            newremoteAttack.transform.localPosition = transform.localPosition;
            newremoteAttack.transform.up = Vector3.left;

            newremoteAttack = Instantiate(Resources.Load("Prefabs/CruiseBullet") as GameObject);
            newremoteAttack.transform.localPosition = transform.localPosition;
            newremoteAttack.transform.up = Vector3.right;

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

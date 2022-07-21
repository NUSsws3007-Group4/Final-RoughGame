using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elite1BulletBehavior : MonoBehaviour
{
    private float attackSpeed = 8f;
    void Start()
    {

    }


    // Update is called once per frame
    void Update()
    {
        Vector3 p = transform.localPosition;
        p += attackSpeed * transform.right * Time.smoothDeltaTime;
        transform.localPosition = p;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != 10 && collision.gameObject.layer != 17 && collision.gameObject.layer != 13)
        {
            Destroy(transform.gameObject);
            Debug.Log(collision);
        }
    }
}

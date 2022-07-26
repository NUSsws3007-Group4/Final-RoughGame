using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elite3Bullet : MonoBehaviour
{
    private Vector3 targetPos;
    private GameObject targetHero;
    private float distance, timer = 0f;
    // Start is called before the first frame update
    void Start()
    {
        targetHero = GameObject.Find("hero");
        distance = Vector3.Distance(transform.position, targetHero.transform.position);
        if (distance <= 2f)
            timer += Time.deltaTime;
        else
            timer = 0;
        if(timer>=2f)
            Destroy(transform.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        targetPos = targetHero.transform.position;
        Vector3 rushVector = targetPos - transform.position;
        rushVector.z = 0f;
        transform.right = rushVector;
        transform.position += transform.right * Time.smoothDeltaTime * 3f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != 10 && collision.gameObject.layer != 17 && collision.gameObject.layer != 13)
        {
            Destroy(transform.gameObject);
        }
    }

}

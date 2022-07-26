using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunTriggerBehavior : MonoBehaviour
{
    private GameObject targetElite;
    // Start is called before the first frame update
    void Start()
    {
        targetElite = GameObject.Find("Elite3");
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == 8)
        {
            targetElite.GetComponent<Elite3EnemyBehavior>().AllowPass();
            Destroy(transform.gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBasicBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("destory");
        if (collision.gameObject.layer == 8)
        {
            Destroy(transform.gameObject);
        }
    }
}

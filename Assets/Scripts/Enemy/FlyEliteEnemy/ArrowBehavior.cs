using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowBehavior : MonoBehaviour
{
    private float arrowSpeed = 15f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.localPosition;
        pos += transform.right * Time.smoothDeltaTime * arrowSpeed;
        transform.localPosition = pos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer != LayerMask.NameToLayer("Enemy"))
        {
            Destroy(transform.gameObject);
        }
    }
}

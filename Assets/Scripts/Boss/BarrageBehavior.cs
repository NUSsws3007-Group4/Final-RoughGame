using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrageBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    private float barrageSpeed = 5f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.localPosition;
        pos += barrageSpeed * transform.up * Time.smoothDeltaTime;
        transform.localPosition = pos;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Boss"))
        {
            Destroy(transform.gameObject);
        }
    }
}

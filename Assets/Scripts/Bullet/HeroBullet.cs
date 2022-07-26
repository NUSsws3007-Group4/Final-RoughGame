using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroBullet : MonoBehaviour
{
    private float heroBulletSpeed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += heroBulletSpeed * transform.right * Time.smoothDeltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            Destroy(transform.gameObject);
        }
    }
}

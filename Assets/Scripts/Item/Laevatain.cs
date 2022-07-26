using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laevatain : MonoBehaviour
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
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            collision.gameObject.GetComponent<HeroBehavior>().withFlame = true;
            Destroy(transform.gameObject);
        }
    }
}

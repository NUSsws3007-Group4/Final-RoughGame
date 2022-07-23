using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class reduceCDscroll : MonoBehaviour
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
        Debug.Log("CD reduce");
        if (collision.gameObject.layer == 8)
        {
            GameObject.Find("hero").GetComponent<HeroAttackHurt>().ReduceColdDown();
            Destroy(transform.gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jiaocheng : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer==LayerMask.NameToLayer("Player"))
        {
            foreach(Transform i in transform)
            {
                i.gameObject.SetActive(true);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            foreach (Transform i in transform)
            {
                i.gameObject.SetActive(false);
            }
        }
    }
}

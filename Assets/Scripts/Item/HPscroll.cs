using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPscroll : MonoBehaviour
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
        Debug.Log("HP increase");
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            GameObject.Find("UImanager").GetComponent<bloodbarcontrol>().changemaxblood(100);
            Destroy(transform.gameObject);
        }
    }
}

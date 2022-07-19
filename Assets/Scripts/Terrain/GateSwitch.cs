using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateSwitch : MonoBehaviour
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
        if (collision.gameObject.layer == 8 && Input.GetKey(KeyCode.E))
        {
            foreach (Transform i in transform)
            {
                i.gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8 && Input.GetKey(KeyCode.E))
        {
            foreach (Transform i in transform)
            {
                i.gameObject.SetActive(false);
            }
        }
    }
}

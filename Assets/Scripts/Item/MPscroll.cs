using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MPscroll : MonoBehaviour
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
        Debug.Log("MP increase");
        if (collision.gameObject.layer == 8)
        {
            GameObject.Find("UImanager").GetComponent<energybarcontrol>().changemaxenergy(1);
            Destroy(transform.gameObject);
        }
    }
}

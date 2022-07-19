using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakBoard : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 19)
        {
            gameObject.SetActive(false);
            foreach (Transform i in transform)
            {
                i.gameObject.SetActive(false);
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 19)
        {
            gameObject.SetActive(false);
            foreach (Transform i in transform)
            {
                i.gameObject.SetActive(false);
            }
        }
    }
}

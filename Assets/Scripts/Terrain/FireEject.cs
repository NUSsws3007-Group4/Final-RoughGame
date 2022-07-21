using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireEject : MonoBehaviour
{
    private float Timer = 0;
    private bool isActive = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Timer += Time.deltaTime;
        if(isActive)
        {
            if(Timer>=3)
            {
                Timer = 0;
                foreach (Transform i in transform)
                {
                    i.gameObject.SetActive(false);
                }
            }
        }
    }
}

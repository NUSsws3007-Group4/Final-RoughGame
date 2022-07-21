using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireEject : MonoBehaviour
{
    private float Timer = 0;
    private bool isActive = false;
    public float StopTime = 2f;
    public float StartTime = 3f;
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
            if (Timer >= StartTime)
            {
                Debug.Log("Fire off");
                Timer = 0;
                isActive = false;
                foreach (Transform i in transform)
                {
                    i.gameObject.SetActive(false);
                }
            }
        }
        else
        {
            if (Timer >= StopTime)
            {
                Debug.Log("Fire on");
                Timer = 0;
                isActive = true;
                foreach (Transform i in transform)
                {
                    i.gameObject.SetActive(true);
                }
            }
        }
    }
}

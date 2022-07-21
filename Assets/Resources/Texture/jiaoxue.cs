using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jiaoxue : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Renderer>().enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
            gameObject.GetComponent<Renderer>().enabled = !gameObject.GetComponent<Renderer>().enabled;
    }

}


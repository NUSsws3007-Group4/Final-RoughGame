using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jiaoxue : MonoBehaviour
{

    public bagmanager bgm;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Renderer>().enabled = false;
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            Time.timeScale = 1 - Time.timeScale;
            gameObject.GetComponent<Renderer>().enabled = !gameObject.GetComponent<Renderer>().enabled;
        }
    }

}


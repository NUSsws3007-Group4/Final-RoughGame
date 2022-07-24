using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JiaochengDiasppear : MonoBehaviour
{
    public GameObject par;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(par)
        {
            if(!par.active)
            {
                gameObject.SetActive(false);
            }
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}

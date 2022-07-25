using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class runoutsign : MonoBehaviour
{
    // Start is called before the first frame update
    public float timer;
    private Color myc;
    public Text myself;

    void Start()
    {
        //timer=0f;
    }

    public void activate()
    {
        timer=2f;
        myc=new Color(1f,0f,0f,1f);
        myself.color=myc;
        gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if(timer>0f)
        {
            timer-=Time.unscaledDeltaTime;
            if(timer<1f)
            {
                myc.a-=Time.unscaledDeltaTime;
                myself.color=myc;
            }
        }
        if(timer<0f)
        {
            timer=0f;
            gameObject.SetActive(false);
        }
    }
}

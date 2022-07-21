using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class merchantbehavior : MonoBehaviour
{
    private bool flag=true;
    public GameObject dialoguebox;
    private float timer;
    public cameramanagerbehavior cm;
    public GameObject hero;

    void Start()
    {
        timer=0f;
        flag=true;
        dialoguebox.SetActive(false);
    }

    void Update()
    {
        if(timer>0) timer-=Time.smoothDeltaTime;
        if(timer<0)
        {
            dialoguebox.SetActive(false);
            cm.setmaintargerget(hero);
            timer=0;
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        //Destroy(collider.gameObject);
        if(collider.gameObject.tag=="Player")
        {
            if(flag)
            {
                flag=false;
                dialoguebox.SetActive(true);
                cm.setmaintargerget(gameObject);
                //dialoguetxt.text="Need something or have something to sell?Maybe I can do you a favour.";
                timer=3f;
            }
        }
    }
}

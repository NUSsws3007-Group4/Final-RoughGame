using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class herointeract : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject muim;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collider)
    {
        if(collider.gameObject.tag=="cherry")
        {
            Destroy(collider.gameObject);
            muim.GetComponent<bloodbarcontrol>().increasevolume(1f);
        }
        
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.layer==14)
        {
            muim.GetComponent<bloodbarcontrol>().decreasevolume(1f);
            gameObject.GetComponent<HeroMovement>().hurt();
        }
        //gameObject.GetComponent<HeroMovement>().setspeed(-50*((float)gameObject.GetComponent<HeroMovement>().getMoveDirection()));

    }
}

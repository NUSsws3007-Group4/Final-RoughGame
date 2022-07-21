using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class herointeract : MonoBehaviour
{
    HeroBehavior mHeroBehavior = null;
    // Start is called before the first frame update
    public bloodbarcontrol mUIManager = null;
    public bagmanager mybagmanager;
    public storemanager mystoremanager;
    public itemstruct bloodflask,energyflask,friendshipflask,emptyflask,diamond;

    private float mHurtTimer = 0;
    void Start()
    {
        mHeroBehavior = gameObject.GetComponent<HeroBehavior>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.B))
        {
            if(mybagmanager.bagisopen)
            {
                mybagmanager.closemybag();
            }
            else
            {
                mystoremanager.closemystore();
                mybagmanager.openmybag();
            }
        }
        if(Input.GetKeyDown(KeyCode.T))
        {
            if(mystoremanager.storeisopen)
            {
                mystoremanager.closemystore();
            }
            else
            {
                mybagmanager.closemybag();
                mystoremanager.openmystore();
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Item"))
        {
            switch(collider.gameObject.tag)
            {
                case "cherry":
                {
                    //Destroy(collider.gameObject);
                    mybagmanager.pickupitem(bloodflask);
                    break;
                }
                case "energyflask":
                {
                    mybagmanager.pickupitem(energyflask);
                    break;
                }
                case "diamond":
                {
                    mybagmanager.pickupitem(diamond);
                    break;
                }
                case "Dash":
                {
                    mHeroBehavior.setDashSkill(true);
                    break;
                }
                case "DoubleJump":
                {
                    mHeroBehavior.setJumpSkill(1);
                    break;
                }
            }
        }

    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == 14 || collider.gameObject.layer == 13)
        {
            mHurtTimer = 0;
            mUIManager.decreasevolume(1f);
            gameObject.GetComponent<HeroBehavior>().hurt();
        }
        //gameObject.GetComponent<HeroMovement>().setspeed(-50*((float)gameObject.GetComponent<HeroMovement>().getMoveDirection()));
    }
    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject.layer == 14 || collider.gameObject.layer == 13)
        {
            mHurtTimer += Time.deltaTime;
            if (mHurtTimer >= 1f)
            {
                mHurtTimer = 0;
                mUIManager.decreasevolume(1f);
                gameObject.GetComponent<HeroBehavior>().hurt();
            }
            
        }
        //gameObject.GetComponent<HeroMovement>().setspeed(-50*((float)gameObject.GetComponent<HeroMovement>().getMoveDirection()));
    }

}

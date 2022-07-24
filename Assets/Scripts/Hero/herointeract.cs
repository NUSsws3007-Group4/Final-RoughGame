using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class herointeract : MonoBehaviour
{
    public bool isResistance = false;
    private float resistanceTimer = 0f;

    HeroBehavior mHeroBehavior = null;
    // Start is called before the first frame update
    public bloodbarcontrol mUIManager = null;
    public bagmanager mybagmanager;
    public storemanager mystoremanager;
    public itemstruct bloodflask, energyflask, friendshipflask, emptyflask, diamond,powerrune,cooldownrune,robustrune,energyrune,key,ragepotion,soulpotion,defencepotion;

    private float mHurtTimer = 0;
    void Start()
    {
        mHeroBehavior = gameObject.GetComponent<HeroBehavior>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isResistance)
        {
            resistanceTimer += Time.deltaTime;
            if(resistanceTimer >= 10f)//无敌状态持续10s
            {
                resistanceTimer = 0f;
                isResistance = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            if (mybagmanager.bagisopen)
            {
                mybagmanager.closemybag();
            }
            else
            {
                mystoremanager.closemystore();
                mybagmanager.openmybag();
            }
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (mystoremanager.storeisopen)
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
        if (collider.gameObject.layer == LayerMask.NameToLayer("Item") || collider.gameObject.layer == LayerMask.NameToLayer("PowerUp"))
        {
            switch (collider.gameObject.tag)
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
                case "atkup":
                {
                    mybagmanager.pickupitem(powerrune);
                    break;
                }
                case "CDreduce":
                {
                    mybagmanager.pickupitem(cooldownrune);
                    break;
                }
                case "HPincrease":
                {
                    mybagmanager.pickupitem(robustrune);
                    break;
                }
                case "MPincrease":
                {
                    mybagmanager.pickupitem(energyrune);
                    break;
                }
                case "key":
                {
                    mybagmanager.pickupitem(key);
                    break;
                }
                case "ragepotion":
                {
                    mybagmanager.pickupitem(ragepotion);
                    break;
                }
                case "defencepotion":
                {
                    mybagmanager.pickupitem(defencepotion);
                    break;
                }
                case "soulpotion":
                {
                    mybagmanager.pickupitem(soulpotion);
                    break;
                }
            }
        }

    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.layer == 14 && !isResistance)
        {
            mHurtTimer = 0;
            mUIManager.decreasevolume(25);
            gameObject.GetComponent<HeroBehavior>().hurt();
        }

        if(collider.gameObject.layer == LayerMask.NameToLayer("Bullet") && !isResistance)
        {
            if(collider.gameObject.tag == "SlimeBall")//史莱姆
            {
                mUIManager.decreasevolume(10);
                gameObject.GetComponent<HeroBehavior>().hurt();
            }
            else if(collider.gameObject.tag == "Arrow")//飞行精英怪
            {
                mUIManager.decreasevolume(30);
                gameObject.GetComponent<HeroBehavior>().hurt();
            }
            else if(collider.gameObject.tag == "Bee")//自杀蜜蜂
            {
                mUIManager.decreasevolume(50);
                gameObject.GetComponent<HeroBehavior>().hurt();
            }
            else if(collider.gameObject.tag == "GroundEliteRemote")//地面精英怪远程攻击
            {
                mUIManager.decreasevolume(20);
                gameObject.GetComponent<HeroBehavior>().hurt();
            }
            else if(collider.gameObject.tag == "GroundEliteCombat")//地面精英怪近战攻击
            {
                mUIManager.decreasevolume(50);
                gameObject.GetComponent<HeroBehavior>().hurt();
            }
            else if(collider.gameObject.tag == "TreeAttack")//树人攻击
            {
                int _branchDamage = collider.gameObject.GetComponent<TreeAttack>().branchDmg;
                mUIManager.decreasevolume(_branchDamage);
                gameObject.GetComponent<HeroBehavior>().hurt();
            }
        }
    }
    void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject.layer == 14 && !isResistance)
        {
            mHurtTimer += Time.deltaTime;
            if (mHurtTimer >= 1f)
            {
                mHurtTimer = 0;
                mUIManager.decreasevolume(25);
                gameObject.GetComponent<HeroBehavior>().hurt();
            }

        }
    }

}

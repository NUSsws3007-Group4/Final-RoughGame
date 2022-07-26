using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class herointeract : MonoBehaviour
{
    public bool isResistance = false;
    private float resistanceTimer = 0f;

    HeroBehavior mHeroBehavior = null;
    // Start is called before the first frame update
    public bloodbarcontrol mUIManager = null;
    public bagmanager mybagmanager;
    public storemanager mystoremanager;
    public GameObject muim;
    //public itemstruct bloodflask, energyflask, friendshipflask, emptyflask, diamond,powerrune,cooldownrune,robustrune,energyrune,key,ragepotion,soulpotion,defencepotion;

    private float mHurtTimer = 0;
    public Text runoutH,runoutE,fullblood,fullenergy;

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
        if (Input.GetKeyDown(KeyCode.E))
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
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            if(muim.GetComponent<bloodbarcontrol>().getvolume()>=muim.GetComponent<bloodbarcontrol>().maxblood)
            {
                fullblood.GetComponent<runoutsign>().activate();
            }
            else if(mybagmanager.bloodflask.itemnum<=0)
            {
                runoutH.GetComponent<runoutsign>().activate();
            }
            else
            {
                mybagmanager.quickuseitem(mybagmanager.bloodflask);
            }
        }
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            if(muim.GetComponent<energybarcontrol>().getvolume()>=muim.GetComponent<energybarcontrol>().maxenergy)
            {
                fullenergy.GetComponent<runoutsign>().activate();
            }
            else if(mybagmanager.energyflask.itemnum<=0)
            {
                runoutH.GetComponent<runoutsign>().activate();
            }
            else
            {
                mybagmanager.quickuseitem(mybagmanager.energyflask);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Item") || collider.gameObject.layer == LayerMask.NameToLayer("PowerUp"))
        {
            if(collider.gameObject.layer == LayerMask.NameToLayer("Item"))
            {
                gameObject.GetComponent<AudioManager>().PlayEffect("GetItem");
            }
            else
            {
                gameObject.GetComponent<AudioManager>().PlayEffect("GainSkill");
            }
            switch (collider.gameObject.tag)
            {
                case "cherry":
                    {
                        //Destroy(collider.gameObject);
                        mybagmanager.pickupitem(mybagmanager.bloodflask);
                        break;
                    }
                case "energyflask":
                    {
                        mybagmanager.pickupitem(mybagmanager.energyflask);
                        break;
                    }
                case "friendshipflask":
                    {
                        mybagmanager.pickupitem(mybagmanager.friendshipflask);
                        break;
                    }
                case "diamond":
                    {
                        mybagmanager.pickupitem(mybagmanager.diamond);
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
                        mybagmanager.pickupitem(mybagmanager.powerrune);
                        break;
                    }
                case "CDreduce":
                    {
                        mybagmanager.pickupitem(mybagmanager.cooldownrune);
                        break;
                    }
                case "HPincrease":
                    {
                        mybagmanager.pickupitem(mybagmanager.robustrune);
                        break;
                    }
                case "MPincrease":
                    {
                        mybagmanager.pickupitem(mybagmanager.energyrune);
                        break;
                    }
                case "Key":
                    {
                        mybagmanager.pickupitem(mybagmanager.key);
                        break;
                    }
                case "ragepotion":
                    {
                        mybagmanager.pickupitem(mybagmanager.ragepotion);
                        break;
                    }
                case "defencepotion":
                    {
                        mybagmanager.pickupitem(mybagmanager.defencepotion);
                        break;
                    }
                case "soulpotion":
                    {
                        mybagmanager.pickupitem(mybagmanager.soulpotion);
                        break;
                    }
                case "scroll1":
                    {
                        mybagmanager.pickupitem(mybagmanager.scroll1);
                        break;
                    }
                case "scroll2":
                    {
                        mybagmanager.pickupitem(mybagmanager.scroll2);
                        break;
                    }
                case "scroll3":
                    {
                        mybagmanager.pickupitem(mybagmanager.scroll3);
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

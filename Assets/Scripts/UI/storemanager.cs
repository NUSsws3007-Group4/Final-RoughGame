using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class storemanager : MonoBehaviour
{
    public itemstruct bloodflask,energyflask,diamond,friendshipflask;

    public Text iteminformationshowed;
    public GameObject muim;
    public bagmanager bg;
    public GameObject mystoreUI;
    private int currentcoin;
    public bool storeisopen;
    public GameObject slotgrid; //itemlistUI

    public Image plate;
    public Image listplate;
    public Image title;
    public bool canopenstore=false;

    private float clock;

    public void setforeststyle()
    {
        plate.color=new Color(0f,0.5f,0f,1f);
        listplate.color=new Color(1f,0.785f,0.392f,1f);
        title.color=new Color(0.48f,0.24f,0f,1f);
    }

    public void setgrassstyle()
    {
        plate.color=new Color(0.4f,1f,0f,1f);
        listplate.color=new Color(1f,1f,0.6f,1f);
        title.color=new Color(1f,0.6f,0f,1f);
    }

    public void seticestyle()
    {
        plate.color=new Color(0f,1f,1f,1f);
        listplate.color=new Color(0.9f,1f,1f,1f);
        title.color=new Color(0f,0f,1f,1f);
    }

    private string inttostring(int k)
    {
        string m="";
        while(k>0)
        {
            m=(char)(k%10+48)+m;
            k/=10;
        }
        return m;
    }

    public void openmystore()
    {
        if(canopenstore)
        {
            mystoreUI.SetActive(true);
            storeisopen=true;
            Time.timeScale = 0;
        }
    }

    public void closemystore()
    {
        mystoreUI.SetActive(false);
        storeisopen=false;
        Time.timeScale = 1;
    }

    public void refreshstore() //refresh everything in store
    {
        foreach(Transform child in slotgrid.transform)
        {
            child.gameObject.GetComponent<storeitembehavior>().refreshstorenum();
            child.gameObject.GetComponent<storeitembehavior>().judgelocked();
        }
    }

    public void updateinfo(string info)
    {
        iteminformationshowed.text=info;
    }
    
    public void sell(itemstruct item)
    {
        bool p=muim.GetComponent<coincontrol>().pay(item.itemprice);
        bg.pickupitem(item);
        item.itemnuminstore--;
    }

    public void setitemnuminstore(itemstruct item,int num)  //can't refresh!
    {
        item.itemnuminstore=num;
    }

    public void setitemstateinstore(itemstruct item,bool state) //can't refresh! true->locked
    {
        item.locked=state;
    }

    void Awake() //set store info;
    {
        bloodflask.itemnuminstore=3;
        bloodflask.locked=false;
        energyflask.itemnuminstore=3;
        energyflask.locked=false;
        friendshipflask.itemnuminstore=3;
        friendshipflask.locked=true;
    }

    void Start()
    {
        seticestyle();
        closemystore();
        canopenstore=false;
        clock=0;
    }

    void Update()
    {

    }
}
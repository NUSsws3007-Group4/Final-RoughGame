using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class bagmanager : MonoBehaviour
{
    //all items
    public itemstruct bloodflask,energyflask,diamond,friendshipflask,emptyflask;

    public GameObject mybagUI=null; //UI
    public GameObject slotgrid; //itemlistUI
    public GameObject muim;
    public Text iteminformationshowed;
    public itemstruct nowselecteditem;
    public Button discardbutton;
    public Button usebutton;
    public Text usebuttontxt;
    public bag myitemlist; //real bag store
    public bool bagisopen;

    public void openmybag()
    {
        mybagUI.SetActive(true);
        usebutton.interactable=false;
        discardbutton.interactable=false;
        usebuttontxt.text="Use";
        iteminformationshowed.text="";
        bagisopen=true;
    }

    public void closemybag()
    {
        mybagUI.SetActive(false);
        bagisopen=false;
    }

    private string inttostring(int k)
    {
        string m="";
        if(k!=0 && k!=1)
        {
            while(k>0)
            {
                m=(char)(k%10+48)+m;
                k/=10;
            }
        }
        return m;
    }

    private void createnewitem(itemstruct item)
    {
        if(!myitemlist.itemlist.Contains(item)) myitemlist.itemlist.Add(item);
        GameObject newItem=Instantiate(Resources.Load("Prefabs/slot") as GameObject,slotgrid.transform);
        Slotproperty newItempro=newItem.GetComponent<Slotproperty>();
        newItem.transform.SetParent(slotgrid.transform,false);
        newItempro.slotitem=item;
        newItempro.slotimage.sprite=item.itemimage;
        newItempro.slotnum.text=inttostring(item.itemnum);
    }

    public void refreshitem(itemstruct item)
    {
        bool flag=false;
        foreach(Transform child in slotgrid.transform)
        {
            Slotproperty childItempro=child.gameObject.GetComponent<Slotproperty>();
            if(childItempro.slotitem==item)
            {
                if(item.itemnum==0)
                {
                    Destroy(child.gameObject);
                    myitemlist.itemlist.Remove(nowselecteditem);
                    nowselecteditem=null;
                    discardbutton.interactable=false;
                    usebutton.interactable=false;
                    iteminformationshowed.text="";
                }
                else
                {
                    childItempro.slotnum.text=inttostring(item.itemnum);
                }
                flag=true;
                break;
            }
        }
        if(!flag && item.itemnum>0)
        {
            createnewitem(item);
            refreshitem(item);
        }
    }

    public void judgecanuse()
    {
        switch(nowselecteditem.itemname)
        {
            case "bloodflask":
            {
                if(muim.GetComponent<bloodbarcontrol>().getvolume()<muim.GetComponent<bloodbarcontrol>().maxblood)
                {
                    usebutton.interactable=true;
                }
                else
                {
                    usebutton.interactable=false;
                }
                break;
            }
            case "energyflask":
            {
                if(muim.GetComponent<energybarcontrol>().getvolume()<muim.GetComponent<energybarcontrol>().maxenergy)
                {
                    usebutton.interactable=true;
                }
                else
                {
                    usebutton.interactable=false;
                }
                break;
            }
            case "friendshipflask": //not done yet
            {
                usebutton.interactable=true;
                break;
            }
            case "diamond":
            {
                usebutton.interactable=true;
                break;
            }
            case "emptyflask":
            {
                if(true) //be close enough to a merchant
                {
                    usebutton.interactable=true;
                }
                else
                {
                    usebutton.interactable=false;
                }
                break;
            }
        }
    }

    public void discarditem()
    {
        if(nowselecteditem==null) return;
        foreach(Transform child in slotgrid.transform)
        {
            Slotproperty childItempro=child.gameObject.GetComponent<Slotproperty>();
            if(childItempro.slotitem==nowselecteditem)
            {
                nowselecteditem.itemnum--;
                if(nowselecteditem.itemnum>0)
                {
                    childItempro.slotnum.text=inttostring(nowselecteditem.itemnum);
                }
                else
                {
                    Destroy(child.gameObject);
                    myitemlist.itemlist.Remove(nowselecteditem);
                    nowselecteditem=null;
                    discardbutton.interactable=false;
                    usebutton.interactable=false;
                    iteminformationshowed.text="";
                }
                break;
            }
        }
    }

    public void useitem()
    {
        switch(nowselecteditem.itemname)
        {
            case "bloodflask":
            {
                muim.GetComponent<bloodbarcontrol>().increasevolume(1f);
                pickupitem(emptyflask);
                break;
            }
            case "energyflask":
            {
                muim.GetComponent<energybarcontrol>().increasevolume(1f);
                pickupitem(emptyflask);
                break;
            }
            case "friendshipflask":
            {
                //friendly
                pickupitem(emptyflask);
                break;
            }
            case "diamond":
            {
                int earning=(int)(Random.Range(80,120));
                muim.GetComponent<coincontrol>().earn(earning);
                break;
            }
            case "emptyflask":
            {
                muim.GetComponent<coincontrol>().earn(5);
                break;
            }
        }
        judgecanuse();  
        discarditem();
    }

    public void pickupitem(itemstruct newitem)
    {
        if(!myitemlist.itemlist.Contains(newitem))
        {
            newitem.itemnum=1;
            createnewitem(newitem);
        }
        else
        {
            newitem.itemnum++;
            refreshitem(newitem);
        }
    }

    public void updateinfo(string info)
    {
        iteminformationshowed.text=info;
    }

    void refreshall()
    {
        refreshitem(bloodflask);
        refreshitem(energyflask);
        refreshitem(friendshipflask);
        refreshitem(emptyflask);
        refreshitem(diamond);
    }

    void Awake()
    {
        myitemlist.itemlist.Clear();

        bloodflask.itemnum=1;
        energyflask.itemnum=1;
        friendshipflask.itemnum=1;
        emptyflask.itemnum=1;
        diamond.itemnum=1;
        refreshall();
    }

    void Start()
    {
        usebutton.interactable=false;
        discardbutton.interactable=false;
        iteminformationshowed.text="";
        closemybag();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

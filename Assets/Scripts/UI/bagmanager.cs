using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class bagmanager : MonoBehaviour
{
    //all items
    public itemstruct bloodflask, energyflask, diamond, friendshipflask, emptyflask,ragerune,cooldownrune,robustrune,energyrune;

    public GameObject mybagUI = null; //UI
    public GameObject slotgrid; //itemlistUI
    public GameObject muim;
    public GameObject hero;
    public Text iteminformationshowed;
    public Text itemtypename;
    public itemstruct nowselecteditem;
    public Button discardbutton;
    public Button usebutton;
    public Image useimage;
    public Image sellimage;
    public Image emptysig;
    public bag myitemlist; //real bag store
    public bool bagisopen;
    public bool cansell;

    public void openmybag()
    {
        Time.timeScale = 0;
        mybagUI.SetActive(true);
        usebutton.interactable = false;
        discardbutton.interactable = false;

        useimage.gameObject.SetActive(true);
        sellimage.gameObject.SetActive(false);

        updateinfo("","");
        bagisopen = true;
    }

    public void closemybag()
    {
        mybagUI.SetActive(false);
        bagisopen = false;
        Time.timeScale = 1;
    }

    private string inttostring(int k)
    {
        string m = "";
        if (k != 0 && k != 1)
        {
            while (k > 0)
            {
                m = (char)(k % 10 + 48) + m;
                k /= 10;
            }
        }
        return m;
    }

    private void judgeempty()
    {
        if (myitemlist.itemlist.Count==0) emptysig.gameObject.SetActive(true); else emptysig.gameObject.SetActive(false);
    }

    private void createnewitem(itemstruct item)
    {
        if (!myitemlist.itemlist.Contains(item)) myitemlist.itemlist.Add(item);
        GameObject newItem = Instantiate(Resources.Load("Prefabs/slot") as GameObject, slotgrid.transform);
        Slotproperty newItempro = newItem.GetComponent<Slotproperty>();
        newItem.transform.SetParent(slotgrid.transform, false);
        newItempro.slotitem = item;
        newItempro.slotimage.sprite = item.itemimage;
        newItempro.slotnum.text = inttostring(item.itemnum);
        emptysig.gameObject.SetActive(false);
    }

    public void refreshitem(itemstruct item)
    {
        bool flag = false;
        foreach (Transform child in slotgrid.transform)
        {
            Slotproperty childItempro = child.gameObject.GetComponent<Slotproperty>();
            if (childItempro.slotitem == item)
            {
                if (item.itemnum == 0)
                {
                    Destroy(child.gameObject);
                    myitemlist.itemlist.Remove(nowselecteditem);
                    nowselecteditem = null;
                    discardbutton.interactable = false;
                    usebutton.interactable = false;
                    iteminformationshowed.text = "";
                }
                else
                {
                    childItempro.slotnum.text = inttostring(item.itemnum);
                }
                flag = true;
                break;
            }
        }
        if (!flag && item.itemnum > 0)
        {
            createnewitem(item);
            refreshitem(item);
        }
        judgeempty();
    }

    public void judgebutton()
    {
        if (nowselecteditem == null) return;
        switch (nowselecteditem.itemtype)
        {
            case "[Potion]":
            {
                switch (nowselecteditem.itemname)
                {
                    case "bloodflask":
                    {
                        if (muim.GetComponent<bloodbarcontrol>().getvolume() < muim.GetComponent<bloodbarcontrol>().maxblood)
                        {
                            usebutton.interactable = true;
                        }
                        else
                        {
                            usebutton.interactable = false;
                        }
                        break;
                    }
                    case "energyflask":
                    {
                        if (muim.GetComponent<energybarcontrol>().getvolume() < muim.GetComponent<energybarcontrol>().maxenergy)
                        {
                            usebutton.interactable = true;
                        }
                        else
                        {
                            usebutton.interactable = false;
                        }
                        break;
                    }
                    case "friendshipflask":
                    {
                        if (hero.GetComponent<HeroBehavior>().getFriendship() >= 0 && hero.GetComponent<HeroBehavior>().getFriendship() < 100)
                        {
                            usebutton.interactable = true;
                        }
                        else
                        {
                            usebutton.interactable = false;
                            friendshipflask.iteminfo = "Evil person doesn't deserve to use this... Reflect on what you have done.";
                        }
                        updateinfo(friendshipflask.iteminfo,friendshipflask.itemtype+" "+friendshipflask.itemname);
                        break;
                    }
                }
                discardbutton.interactable=true;
                break;
            }
            case "[Treasure]":
            {
                usebutton.interactable=cansell;
                discardbutton.interactable=true;
                break;
            }
            case "[Scrap]":
            {
                usebutton.interactable=cansell;
                discardbutton.interactable=true;
                break;
            }
            case "[Rune]":
            {
                usebutton.interactable=false;
                discardbutton.interactable=false;
                break;
            }
        }
    }

    public void discarditem()
    {
        if (nowselecteditem == null) return;
        foreach (Transform child in slotgrid.transform)
        {
            Slotproperty childItempro = child.gameObject.GetComponent<Slotproperty>();
            if (childItempro.slotitem == nowselecteditem)
            {
                nowselecteditem.itemnum--;
                if (nowselecteditem.itemnum > 0)
                {
                    childItempro.slotnum.text = inttostring(nowselecteditem.itemnum);
                }
                else
                {
                    Destroy(child.gameObject);
                    myitemlist.itemlist.Remove(nowselecteditem);
                    nowselecteditem = null;
                    discardbutton.interactable = false;
                    usebutton.interactable = false;
                    iteminformationshowed.text = "";
                }
                break;
            }
        }
        judgeempty();
    }

    public void useitem()
    {
        switch (nowselecteditem.itemname)
        {
            case "bloodflask":
                {
                    muim.GetComponent<bloodbarcontrol>().increasevolume(1);
                    pickupitem(emptyflask);
                    break;
                }
            case "energyflask":
                {
                    muim.GetComponent<energybarcontrol>().increasevolume(1);
                    pickupitem(emptyflask);
                    break;
                }
            case "friendshipflask":
                {
                    hero.GetComponent<HeroBehavior>().upFriendship(10);
                    int i = ++hero.GetComponent<HeroFakeFriendly>().usedCount;
                    pickupitem(emptyflask);
                    switch (i)
                    {
                        case 1:
                            friendshipflask.iteminfo = "This liquid can make enemies more friendly to you.\nEnjoy it.";
                            break;
                        case 3:
                            friendshipflask.iteminfo = "Enemies seem to be more friendly to me...\nBut why do I feel anxious?\nMaybe I shouln't use it too much...";
                            break;
                        case 5:
                            friendshipflask.iteminfo = "I'm worried that something bad will happen...";
                            break;
                    }
                    if (i >= 5)
                    {
                        hero.GetComponent<HeroFakeFriendly>().fakeFriendly = true;
                    };
                    updateinfo(friendshipflask.iteminfo,friendshipflask.itemtype+" "+friendshipflask.itemname);

                    break;
                }
            case "diamond":
                {
                    int earning = (int)(Random.Range(80, 120));
                    muim.GetComponent<coincontrol>().earn(earning);
                    break;
                }
            case "emptyflask":
                {
                    muim.GetComponent<coincontrol>().earn(5);
                    break;
                }
        }
        judgebutton();
        discarditem();
    }

    public void pickupitem(itemstruct newitem)
    {
        Debug.Log(newitem.itemname);
        if (!myitemlist.itemlist.Contains(newitem))
        {
            newitem.itemnum = 1;
            createnewitem(newitem);
        }
        else
        {
            newitem.itemnum++;
            refreshitem(newitem);
        }
    }

    public void updateinfo(string info,string naame)
    {
        iteminformationshowed.text = info;
        itemtypename.text=name;
    }

    void refreshall()
    {
        refreshitem(bloodflask);
        refreshitem(energyflask);
        refreshitem(friendshipflask);
        refreshitem(emptyflask);
        refreshitem(diamond);
        judgeempty();
    }

    void Awake()
    {
        myitemlist.itemlist.Clear();
        bloodflask.itemnum = 1;
        energyflask.itemnum = 0;
        friendshipflask.itemnum = 0;
        emptyflask.itemnum = 0;
        diamond.itemnum = 1;
        refreshall();
    }

    void Start()
    {
        usebutton.interactable = false;
        discardbutton.interactable = false;
        iteminformationshowed.text = "";
        cansell = false;
        hero = GameObject.Find("hero");
        Debug.Log(hero.name);
        friendshipflask.iteminfo = "A kind of mysterious liquid...\nWant to have a try?";
        closemybag();
    }

    // Update is called once per frame
    void Update()
    {

    }
}

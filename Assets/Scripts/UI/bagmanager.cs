using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class bagmanager : MonoBehaviour
{
    //all items
    public itemstruct bloodflask, energyflask, diamond, friendshipflask, emptyflask;

    public GameObject mybagUI = null; //UI
    public GameObject slotgrid; //itemlistUI
    public GameObject muim;
    public GameObject hero;
    public Text iteminformationshowed;
    public itemstruct nowselecteditem;
    public Button discardbutton;
    public Button usebutton;
    public Image useimage;
    public Image sellimage;
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

        iteminformationshowed.text = "";
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

    private void createnewitem(itemstruct item)
    {
        if (!myitemlist.itemlist.Contains(item)) myitemlist.itemlist.Add(item);
        GameObject newItem = Instantiate(Resources.Load("Prefabs/slot") as GameObject, slotgrid.transform);
        Slotproperty newItempro = newItem.GetComponent<Slotproperty>();
        newItem.transform.SetParent(slotgrid.transform, false);
        newItempro.slotitem = item;
        newItempro.slotimage.sprite = item.itemimage;
        newItempro.slotnum.text = inttostring(item.itemnum);
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
    }

    public void judgecanuse()
    {
        if (nowselecteditem == null) return;
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
                    }
                    break;
                }
            case "diamond":
                {
                    usebutton.interactable = cansell;
                    break;
                }
            case "emptyflask":
                {
                    usebutton.interactable = cansell;
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
                    pickupitem(emptyflask);
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
        judgecanuse();
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

    public void updateinfo(string info)
    {
        iteminformationshowed.text = info;
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
        bloodflask.itemnum = 1;
        energyflask.itemnum = 1;
        friendshipflask.itemnum = 1;
        emptyflask.itemnum = 1;
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
        closemybag();
    }

    // Update is called once per frame
    void Update()
    {

    }
}

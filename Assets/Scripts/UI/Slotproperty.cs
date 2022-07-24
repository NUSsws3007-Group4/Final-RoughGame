using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slotproperty : MonoBehaviour
{
    public itemstruct slotitem;
    public Image slotimage;
    public Text slotnum;

    public void OnClicked()
    {
        bagmanager bg=GameObject.Find("Canvas").GetComponent<bagmanager>();
        if(slotitem.itemname=="diamond" || slotitem.itemname=="emptyflask")
        {
            bg.useimage.gameObject.SetActive(false);
            bg.sellimage.gameObject.SetActive(true);
        }
        else
        {
            bg.useimage.gameObject.SetActive(true);
            bg.sellimage.gameObject.SetActive(false);
        }
        bg.updateinfo(slotitem.iteminfo,slotitem.itemtype+" "+slotitem.itemname);
        bg.nowselecteditem=slotitem;
        bg.discardbutton.interactable=true;
        bg.judgebutton();
    }
}

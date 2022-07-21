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
            bg.usebuttontxt.text="Sell";
        }
        else
        {
            bg.usebuttontxt.text="Use";
        }
        bg.updateinfo(slotitem.iteminfo);
        bg.nowselecteditem=slotitem;
        bg.discardbutton.interactable=true;
        bg.judgecanuse();
    }
}

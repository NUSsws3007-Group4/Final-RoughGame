using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class storeitembehavior : MonoBehaviour
{
    public GameObject muim;
    public itemstruct item;
    private storemanager stg;
    public Text buttonprice;
    public Image soldoutsig;
    public Button buybutton;
    public Image itemimage;
    public Image lockedimage;
    public Text storenumtxt;

    private int money;
    private float switcht; //次数
    private float switchtime; //时长
    private Color colorred;
    private Color colorblack;

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

    void Awake() //不用awake，bagmanager和storemanager里面的设定在awake里
    {

    }

    void Start()
    {
        refreshmyself();
        if(item.itemprice==0) buttonprice.text="free"; else buttonprice.text=inttostring(item.itemprice);
        colorred=new Color(1f,0f,0f,1f);
        colorblack=new Color(0f,0f,0f,1f);
        stg=GameObject.Find("Canvas").GetComponent<storemanager>();
        switcht=0;
    }

    public void onClicked() //更新显示信息
    {
        stg.updateinfo(item.iteminfo);
    }

    public void refreshmyself() //统一刷新
    {
        //judgesoldout();
        //judgelocked();
        refreshstorenum();
        judgelocked();
    }

    public void refreshstorenum() //更新库存数量
    {
        storenumtxt.text=inttostring(item.itemnuminstore);
        judgesoldout();
    }

    public void trybought()
    {
        money=muim.GetComponent<coincontrol>().getcoinnum();
        if(item.itemprice>money)
        {
            buttonprice.color=colorred;
            switcht=3;
            switchtime=0.07f;
        }
        else
        {
            stg.sell(item);
            refreshstorenum();
        }
    }

    public void judgesoldout()
    {
        if(item.locked) return;

        if(item.itemnuminstore>0)
        {
            soldoutsig.gameObject.SetActive(false);
            itemimage.color=new Color(1f,1f,1f,1f);
            buybutton.interactable=true;
        }
        else
        {
            soldoutsig.gameObject.SetActive(true);
            itemimage.color=new Color(0.6f,0.6f,0.6f,1f);
            buybutton.interactable=false;
        }
    }

    public void judgelocked()
    {
        if(item.locked) lockthisitem(); else unlockthisitem();
    }

    private void lockthisitem()
    {
        soldoutsig.gameObject.SetActive(false);
        lockedimage.gameObject.SetActive(true);
        itemimage.color=new Color(1f,1f,1f,1f);
        buybutton.interactable=false;
        gameObject.GetComponent<Button>().interactable=false;
        storenumtxt.text="";
    }

    private void unlockthisitem()
    {
        lockedimage.gameObject.SetActive(false);
        gameObject.GetComponent<Button>().interactable=true;
        buybutton.interactable=true;
        refreshstorenum();
    }

    void Update()
    {
        if(switcht>0)
        {
            switchtime-=Time.unscaledDeltaTime;
            if(switchtime<0f)
            {
                switchtime=0.07f;
                if(buttonprice.color==colorred)
                {
                    buttonprice.color=colorblack;
                    switcht--;
                }
                else
                {
                    buttonprice.color=colorred;
                }
            }
        }
    }
}
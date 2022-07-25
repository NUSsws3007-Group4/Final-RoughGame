using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class tinymapbehavior : MonoBehaviour
{
    public Canvas ftcanvas;
    private RectTransform ftrt;
    private float mheight;
    private float mwidth;
    private Vector2 msizedelta;
    private Vector3 mpos;
    private const float heightbywidth=32f/68f;
    public RawImage Darkcover;

    void Start()
    {
        //Darkcover.color=new Color(0f,0f,0f,0f);
        ftrt=ftcanvas.GetComponent<RectTransform>();
        /*
        msizedelta=ftrt.sizeDelta;
        msizedelta.y/=3f; 
        msizedelta.x=msizedelta.y/heightbywidth;
        if(msizedelta.x>ftrt.sizeDelta.x)
        {
            msizedelta=ftrt.sizeDelta;
            msizedelta.x/=3f;
            msizedelta.y=msizedelta.x*heightbywidth;     
        }
        */
        msizedelta.x=msizedelta.y=0f;
        gameObject.GetComponent<RectTransform>().sizeDelta=msizedelta;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            msizedelta=ftrt.sizeDelta;
            msizedelta.x=msizedelta.y/heightbywidth;
            mpos.x=(ftrt.sizeDelta.x-msizedelta.x)/2f;
            mpos.y=0f;
            if(msizedelta.x>ftrt.sizeDelta.x)
            {
                msizedelta=ftrt.sizeDelta;
                msizedelta.y=msizedelta.x*heightbywidth;
                mpos.x=0f;
                mpos.y=-(ftrt.sizeDelta.y-msizedelta.y)/2f;
            }
            gameObject.GetComponent<RectTransform>().sizeDelta=msizedelta;
            gameObject.GetComponent<RectTransform>().anchoredPosition=mpos;  
            Darkcover.gameObject.SetActive(true);   
            //Darkcover.color=new Color(0f,0f,0f,0.6f); 
        }

        if(Input.GetKeyUp(KeyCode.Tab))
        {
            /*
            msizedelta=ftrt.sizeDelta;  
            msizedelta.y/=3f; 
            msizedelta.x=msizedelta.y/heightbywidth;
            if(msizedelta.x>ftrt.sizeDelta.x)
            {
                msizedelta=ftrt.sizeDelta;
                msizedelta.x/=3f;
                msizedelta.y=msizedelta.x*heightbywidth;     
            }
            */
            msizedelta.x=msizedelta.y=0f;
            mpos.x=0f;
            mpos.y=0f;
            gameObject.GetComponent<RectTransform>().sizeDelta=msizedelta;
            gameObject.GetComponent<RectTransform>().anchoredPosition=mpos;
            Darkcover.gameObject.SetActive(false);
            //Darkcover.color=new Color(0f,0f,0f,0f);
        }
    }
}

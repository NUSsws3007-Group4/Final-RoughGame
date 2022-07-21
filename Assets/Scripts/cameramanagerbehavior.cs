using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cameramanagerbehavior : MonoBehaviour
{
    public Camera maincamera;
    public Camera auxiliarycamera;
    public Camera Tinymapcamera;
    public RawImage tinymap;
    public RawImage auxiliaryviewUI;
    public RenderTexture auxiliaryview;

    void Awake()
    {
        auxiliaryviewUI.GetComponent<RectTransform>().sizeDelta=new Vector2(0f,0f);
    }

    public void setauxiliarycamera(float x,float y,float width,float height,int pixelrate) //rate=pixel per unit/middle
    {
        auxiliarycamera.orthographicSize=height/2f;
        auxiliarycamera.transform.localPosition=new Vector3(x,y,-10f);
        auxiliaryview=new RenderTexture((int)(width*pixelrate),(int)(height*pixelrate),16);
    }

    public void setauxiliaryviewUI(float x,float y,float width,float height) //resolution not units/top left
    {
        auxiliaryviewUI.GetComponent<RectTransform>().sizeDelta=new Vector2(width,height);
        auxiliaryviewUI.GetComponent<RectTransform>().anchoredPosition=new Vector3(x,y,0f);
    }

    public Vector2 getmaincamerasize()
    {
        return new Vector2(maincamera.orthographicSize*maincamera.aspect,maincamera.orthographicSize);
    }

    public Vector2 getscreensize()
    {
        return new Vector2(Screen.width,Screen.height);
    }

    public void setmaincamerafollow()
    {
        maincamera.GetComponent<camerafollow>().iffollow=true;
    }

    public void setmaincamerafixed(float x,float y)
    {
        maincamera.GetComponent<camerafollow>().iffollow=false;
        maincamera.transform.localPosition=new Vector3(x,y,-10);
    }

    public void setmaincamerasize(float halfheight)
    {
        maincamera.orthographicSize=halfheight;
    }

    void Start()
    {
        //setauxiliarycamera(0,0,60f,60f,1);
        //setauxiliaryviewUI(0f,0f,200f,200f);
        setmaincamerafollow();
    }

    void Update()
    {
        
    }
}

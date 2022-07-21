using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camerafollow : MonoBehaviour
{
    public GameObject target;
    public Vector3 targetpos; //not target's position
    public Bounds worldbound;
    public Camera mcamera;
    private float camerasizex,camerasizey; //half of the realsize

    public float smoothTime=0.3f;
    private float xVelocity,yVelocity=0f;

    private Vector3 offset;
    private Vector3 currentpos;
    public bool iffollow=true;

    void Start()
    {
        mcamera=Camera.main.GetComponent<Camera>();
        camerasizex=mcamera.orthographicSize*mcamera.aspect;
        camerasizey=mcamera.orthographicSize;
        worldbound.center=Vector3.zero;
        worldbound.size=new Vector3(68f,32f,2f);
        offset=transform.position-target.transform.position;
    }

    void Update()
    {
        if(iffollow)
        {
            currentpos=transform.position;
            targetpos=target.transform.position;
        
            if(targetpos.x>worldbound.max.x-camerasizex)
            {
                targetpos.x=worldbound.max.x-camerasizex;
            }
            else if(targetpos.x<worldbound.min.x+camerasizex)
            {
                targetpos.x=worldbound.min.x+camerasizex;
            }
            if(targetpos.y>worldbound.max.y-camerasizey)
            {
                targetpos.y=worldbound.max.y-camerasizey;
            }
            else if(targetpos.y<worldbound.min.y+camerasizey)
            {
                targetpos.y=worldbound.min.y+camerasizey;
            }

            currentpos.x=Mathf.SmoothDamp(transform.position.x,targetpos.x,ref xVelocity,smoothTime);
            currentpos.y=Mathf.SmoothDamp(transform.position.y,targetpos.y,ref yVelocity,smoothTime/2f);
            //currentpos.x=targetpos.x;
            //currentpos.y=targetpos.y;

            transform.position=currentpos;
        }
    }
}

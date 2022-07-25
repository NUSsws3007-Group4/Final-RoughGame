using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boxfollow : MonoBehaviour
{
    public GameObject target;
    private Vector3 offset; 
    void Start()
    {
        offset=transform.localPosition-target.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if(target!=null)
        {
            transform.localPosition=target.transform.localPosition+offset;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}

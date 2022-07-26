using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceThornBehavior : MonoBehaviour
{
    private float iceThornExistenceTimer = 0f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        iceThornExistenceTimer += Time.smoothDeltaTime;
        if(iceThornExistenceTimer >= 1f)
        {
            iceThornExistenceTimer = 0f;
            Destroy(transform.gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningBehavior : MonoBehaviour
{
    private float warningTimer = 0f;
    
    void Start()
    {
        
    }

    void Update()
    {
        warningTimer += Time.smoothDeltaTime;
        if(warningTimer >= 1f)
        {
            warningTimer = 0f;
            Destroy(transform.gameObject);
        }
    }
}

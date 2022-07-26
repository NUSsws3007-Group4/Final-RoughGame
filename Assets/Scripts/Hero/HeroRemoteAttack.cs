using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroRemoteAttack : MonoBehaviour
{
    public bool remoteAttackEnable = false;

    void Start()
    {
        
    }

    
    void Update()
    {
        if(remoteAttackEnable)
        {
            if(Input.GetKeyDown(KeyCode.K))
            {
                GameObject remoteAttack = Instantiate(Resources.Load("Prefabs/HeroBullet") as GameObject);
                remoteAttack.transform.position = transform.position;
                remoteAttack.transform.right = transform.right;
            }
        }
    }
}

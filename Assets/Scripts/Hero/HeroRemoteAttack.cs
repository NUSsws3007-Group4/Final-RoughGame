using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroRemoteAttack : MonoBehaviour
{
    public bool remoteAttackEnable = false;

    public energybarcontrol mEnegyBarManager = null;

    void Start()
    {
        
    }

    
    void Update()
    {
        if(remoteAttackEnable)
        {
            if(Input.GetKeyDown(KeyCode.K) && mEnegyBarManager.getvolume() >= 1)
            {
                gameObject.GetComponent<AudioManager>().PlayHero("RemoteAttack");
                mEnegyBarManager.decreasevolume(1);
                GameObject remoteAttack = Instantiate(Resources.Load("Prefabs/HeroBullet") as GameObject);
            }
        }
    }
}

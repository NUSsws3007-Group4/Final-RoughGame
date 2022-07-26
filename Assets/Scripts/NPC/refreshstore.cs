using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class refreshstore : MonoBehaviour
{
    // Start is called before the first frame update
    private storemanager stm;

    void Start()
    {
        stm=GameObject.Find("Canvas").GetComponent<storemanager>();
        stm.bloodflask.itemnuminstore+=1;
        stm.energyflask.itemnuminstore+=1;
        //stm.friendshipflask.itemnuminstore+=1;
        stm.ragepotion.itemnuminstore+=1;
        stm.soulpotion.itemnuminstore+=1;
        stm.defencepotion.itemnuminstore+=1;
        stm.setitemstateinstore(stm.ragepotion,false);
        stm.setitemstateinstore(stm.soulpotion,false);
        stm.setitemstateinstore(stm.defencepotion,false);
        stm.refreshstore();
        stm.seticestyle();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

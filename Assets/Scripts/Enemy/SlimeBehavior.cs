using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeBehavior : EnemyBehavior
{

    override protected void eject()
    {
        Debug.Log("Ejectopm");
        GameObject remoteAttack = Instantiate(Resources.Load("Prefabs/Ball") as GameObject);
        targetpos = targethero.transform.localPosition;
        remoteAttack.transform.localPosition = transform.localPosition;
        remoteAttack.transform.up = (targetpos - transform.localPosition).normalized;
    }
}

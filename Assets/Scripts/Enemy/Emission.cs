using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emission : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject targethero;
    private Vector3 targetpos;
    const float pi = 3.1416f;

    public void emitnormalbullet()
    {
        GameObject remoteAttack = Instantiate(Resources.Load("Prefabs/BulletScreen") as GameObject);
        targetpos = targethero.transform.localPosition;
        targetpos.z=0;
        remoteAttack.transform.localPosition = transform.localPosition;
        remoteAttack.transform.up = (targetpos - transform.localPosition).normalized;
    }

    public void emitcruisebullet()
    {
        GameObject remoteAttack = Instantiate(Resources.Load("Prefabs/CruiseBullet") as GameObject);
        targetpos = targethero.transform.localPosition;
        targetpos.z=0;
        remoteAttack.transform.localPosition = transform.localPosition;
        remoteAttack.transform.up = (targetpos - transform.localPosition).normalized;
    }

    public void emitexplosivebullet()
    {
        GameObject remoteAttack = Instantiate(Resources.Load("Prefabs/ExplosiveBullet") as GameObject);
        targetpos = targethero.transform.localPosition;
        targetpos.z=0;
        remoteAttack.transform.localPosition = transform.localPosition;
        remoteAttack.transform.up = (targetpos - transform.localPosition).normalized;
    }

    public void emitbulletscreen()
    {
        GameObject remoteAttack;
        Vector3 direction, tvec;
        targetpos = targethero.transform.localPosition;
        targetpos.z=0;
        for (float i = -30f; i <= 30f; i += 15f)
        {
            remoteAttack = Instantiate(Resources.Load("Prefabs/BulletScreen") as GameObject);
            remoteAttack.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            remoteAttack.transform.localPosition = transform.localPosition;

            tvec = targetpos - transform.localPosition;
            direction.x = tvec.x * Mathf.Cos(i / 180f * pi) + tvec.y * Mathf.Sin(i / 180f * pi);
            direction.y = -tvec.x * Mathf.Sin(i / 180f * pi) + tvec.y * Mathf.Cos(i / 180f * pi);
            direction.z = 0;
            remoteAttack.transform.up = direction.normalized;
        }
    }

    public void emitbulletscreencapsule()
    {
        GameObject remoteAttack = Instantiate(Resources.Load("Prefabs/BulletScreenCapsule") as GameObject);
        targetpos = targethero.transform.localPosition;
        remoteAttack.transform.localPosition = transform.localPosition;
        remoteAttack.transform.up = (targetpos - transform.localPosition).normalized;
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}

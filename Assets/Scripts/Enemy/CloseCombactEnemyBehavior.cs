using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseCombactEnemyBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    private float timer;
    private float attackgap;
    public GameObject muim = null;

    void Start()
    {
        attackgap = 1f;
        timer = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
            if (timer < 0) timer = 0;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            if (timer == 0)
            {
                timer = attackgap;
                collision.gameObject.GetComponent<HeroBehavior>().hurt();
                muim.GetComponent<bloodbarcontrol>().decreasevolume(1f);
            }
        }
    }

}

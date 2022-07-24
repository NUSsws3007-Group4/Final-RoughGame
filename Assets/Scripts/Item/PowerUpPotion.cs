using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpPotion : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            other.gameObject.GetComponent<HeroAttackHurt>().powerUpPotion = true;
            Destroy(transform.gameObject);
        }
    }
}

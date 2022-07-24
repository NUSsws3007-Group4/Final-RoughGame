using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeAttack : MonoBehaviour
{
    private float branchSpeed = 7.0f;

    public int branchDmg = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += branchSpeed * transform.right * Time.smoothDeltaTime;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Destroy(transform.gameObject);
    }
}

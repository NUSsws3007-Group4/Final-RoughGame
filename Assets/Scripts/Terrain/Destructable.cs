using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructable : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            foreach (Transform i in transform)
            {
                i.gameObject.SetActive(false);
            }
            Debug.Log("Trap Destoryed");
            gameObject.SetActive(false);
            //Destroy(transform.gameObject);
        }
    }
}

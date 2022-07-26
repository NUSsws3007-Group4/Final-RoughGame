using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroBullet : MonoBehaviour
{
    private float heroBulletSpeed = 8f;
    GameObject mHero;
    private int faceDir = 1;

    // Start is called before the first frame update
    void Start()
    {
        mHero = GameObject.Find("hero");
        transform.position = new Vector3(mHero.transform.position.x, mHero.transform.position.y + 0.5f, 0);
        faceDir = mHero.gameObject.GetComponent<HeroBehavior>().getFaceDir();
        if(faceDir == 1)
        {
            transform.right = new Vector3(1, 0, 0);
        }
        else if(faceDir == -1)
        {
            transform.right = new Vector3(-1, 0, 0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += heroBulletSpeed * transform.right * Time.smoothDeltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Enemy")|| 
           other.gameObject.layer == LayerMask.NameToLayer("Boss")||
           other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            //Debug.Log("Kill a Enemy in distance");
            Destroy(transform.gameObject);
        }
    }
}

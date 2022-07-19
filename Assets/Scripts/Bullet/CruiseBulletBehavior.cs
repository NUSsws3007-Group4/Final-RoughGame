using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CruiseBulletBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    private GameManager mb;
    private GameObject targethero;
    private Vector3 targetpos;
    private Vector3 targetvec;
    private Vector3 rotatefvec;
    private float speed;
    private float rotaterate;

    void Start()
    {
        targethero = GameObject.Find("hero");
        mb = GameObject.Find("GameManager").GetComponent<GameManager>();
        speed = 10f;
        rotaterate = 100f;
    }

    // Update is called once per frame
    void Update()
    {
        targetpos = targethero.transform.localPosition;
        targetpos.z=0;
        targetvec = targetpos - gameObject.transform.localPosition;
        //tagertvec.z=0;

        rotatefvec = Vector3.Cross(transform.up, targetvec);
        if (rotatefvec == Vector3.zero) rotatefvec = Vector3.forward;
        rotatefvec = rotatefvec.normalized;

        gameObject.transform.Rotate(rotatefvec * rotaterate * Time.smoothDeltaTime);

        transform.localPosition += transform.up * Time.smoothDeltaTime * speed;

        if (!mb.boundscontainspoint(mb.camerabounds, transform.localPosition))
        {
            Destroy(transform.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer!=10)
        {
            Destroy(transform.gameObject);
        }
    }
}

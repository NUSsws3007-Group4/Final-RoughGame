using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceCrystalBehavior : MonoBehaviour
{
    // Start is called before the first frame update
    private GameManager mGameManager;
    private GameObject targetPlayer;
    private Vector3 targetPosition;
    private Vector3 targetVector;
    private float speed;

    void Start()
    {
        targetPlayer = GameObject.Find("hero");
        mGameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        speed = 10f;
    }

    // Update is called once per frame
    void Update()
    {
        targetPosition = targetPlayer.transform.localPosition;
        targetPosition.z=0;
        targetVector = targetPosition - gameObject.transform.localPosition;
        transform.right = targetVector;

        transform.localPosition += transform.right * Time.smoothDeltaTime * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Boss"))
        {
            Destroy(transform.gameObject);
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroBehavior : MonoBehaviour
{
    private bool followMouse = true;
    private float respawnTimer;
    private Vector3 newpos;
    public GameObject uimanager;
    public float attackingtimer;
    // Start is called before the first frame update
    void Start()
    {
        attackingtimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (attackingtimer > 0)
        {
            attackingtimer -= Time.deltaTime;
            if (attackingtimer < 0)
            {
                attackingtimer = 0;
                gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
        if (followMouse == true)
        {
            respawnTimer = 0;
            newpos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            newpos.z = -1f;
            gameObject.transform.localPosition = newpos;
        }
        else
        {
            respawnTimer += Time.deltaTime;
            if (respawnTimer >= 2f)
            {
                respawnTimer = 0;
                followMouse = true;
            }
        }
    }

    public void setattacked()
    {
        if (attackingtimer == 0)
        {
            attackingtimer = 0.2f;
            gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            uimanager.GetComponent<bloodbarcontrol>().decreasevolume(1);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 13 && followMouse)
        {
            setattacked();
        }
        if (uimanager.GetComponent<bloodbarcontrol>().IsDead() || (collision.gameObject.layer == 6 && !followMouse))
        {
            Respawn();
        }
    }


    private void Respawn()
    {
        followMouse = false;
        CameraSupport s = Camera.main.GetComponent<CameraSupport>();
        if (s != null)
        {
            Vector3 p = transform.localPosition;
            p.x = s.GetWorldBound().min.x * 0.9f + Random.value * s.GetWorldBound().size.x * 0.8f;
            p.y = s.GetWorldBound().min.y * 0.9f + Random.value * s.GetWorldBound().size.y * 0.8f;
            Debug.Log("Respawning at : " + p.x + "," + p.y);
            transform.localPosition = p;
            uimanager.GetComponent<bloodbarcontrol>().Respawn();
        }
    }
    public bool IsRespawned()
    {
        return respawnTimer >= 0.001f;
    }
    public bool IsHidden()
    {
        return !followMouse;
    }
}

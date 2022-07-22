using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.LogWarning(SceneManager.GetActiveScene().name+"onload");
            GameObject hero;
        if (hero = GameObject.Find("hero"))
        {
            hero.GetComponent<HeroBehavior>().setRespawnPoint(transform.position);
            hero.transform.position = transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

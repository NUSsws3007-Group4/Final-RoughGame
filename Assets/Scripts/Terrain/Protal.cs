using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Protal : MonoBehaviour
{
    public string nextLevel;
    private bool mInProtal = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (mInProtal && Input.GetKeyDown(KeyCode.E))
        {
            Debug.LogWarning("Loading: " + nextLevel);
            DontDestroyOnLoad(GameObject.Find("KeepInTransfer"));
            SceneManager.LoadScene(nextLevel);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            mInProtal = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            mInProtal = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            mInProtal = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Protal : MonoBehaviour
{
    public string nextLevel;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            transform.GetChild(0).GetComponent<Renderer>().enabled = true;
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.LogWarning("Loading: " + nextLevel);
                DontDestroyOnLoad(GameObject.Find("KeepInTransfer"));
                SceneManager.LoadScene(nextLevel);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.LogWarning("Loading: " + nextLevel);
                DontDestroyOnLoad(GameObject.Find("KeepInTransfer"));
                SceneManager.LoadScene(nextLevel);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        transform.GetChild(0).GetComponent<Renderer>().enabled = false;
    }
}

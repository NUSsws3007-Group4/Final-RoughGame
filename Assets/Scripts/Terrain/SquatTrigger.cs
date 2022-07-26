using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class SquatTrigger : MonoBehaviour
{
    private DialogueRunner dialogueRunner;
    void Start()
    {
        dialogueRunner = GameObject.Find("Dialogue System").GetComponent<DialogueRunner>();
    }
    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            dialogueRunner.Stop();
            dialogueRunner.StartDialogue("Squat");
            Destroy(transform.gameObject);
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;

public class RemoteAttackAbility : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            DialogueRunner dialogueRunner = GameObject.Find("Dialogue System").GetComponent<DialogueRunner>();
            dialogueRunner.Stop();
            dialogueRunner.StartDialogue("RemoteAttack");
            other.gameObject.GetComponent<HeroRemoteAttack>().remoteAttackEnable = true;
            Destroy(transform.gameObject);
        }
    }
}

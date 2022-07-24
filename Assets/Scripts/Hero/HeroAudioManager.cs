using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroAudioManager : MonoBehaviour
{
    AudioSource speaker;
    // Start is called before the first frame update
    void Start()
    {
        speaker = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void play(string _path)
    {
        if(!speaker.isPlaying)
        {
            speaker.clip = Resources.Load<AudioClip>("Audio/Hero/"+ _path);
            speaker.Play();
        }
    }
    public void audioJump()
    {
        play("juming");

    }
    public void audioHurt()
    {
        play("attacked");
    }
    public void audioAttack()
    {
        play("swordAttacking");
    }
    public void audioSkill()
    {
        play("skillAttacking");
    }
}

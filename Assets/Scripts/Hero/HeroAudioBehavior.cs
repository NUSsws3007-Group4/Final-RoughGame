/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroAudioBehavior : MonoBehaviour
{
    AudioSource voicePlayer;
    float voiceVolumeValue = 0.5f;
    float timer = 2.0f;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            if (Input.GetKeyDown(KeyCode.J))
                AudioClip audio = (AudioClip)Resources.Load("Audio/Hero/jumping", typeof(AudioClip));
        }
    }
}
*/
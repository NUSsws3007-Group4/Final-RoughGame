using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
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
    /// <summary>
    /// 播放Resources/Audio下声音
    /// </summary>
    /// <param name="_path">
    /// 在Audio下路径以及文件名
    /// </param>
    public void PlayAudio(string _path)
    {
        if(!speaker.isPlaying||true)//要不要这个？
        {
            speaker.clip = Resources.Load<AudioClip>("Audio/"+ _path);
            speaker.Play();
        }
    }
    public void audioJump()
    {
        PlayAudio("Hero/juming");

    }
    public void audioHurt()
    {
        PlayAudio("Hero/attacked");
    }
    public void audioAttack()
    {
        PlayAudio("Hero/swordAttacking");
    }
    public void audioSkill()
    {
        PlayAudio("Hero/skillAttacking");
    }
}

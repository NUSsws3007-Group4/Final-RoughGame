using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource speakerHero;
    public AudioSource speakerEnemy;
    public AudioSource speakerEffect;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
    /// <summary>
    /// 播放敌人音效
    /// </summary>
    /// <param name="_file">
    /// 在Audio/Enemy下路径以及文件名
    /// </param>
    public void PlayEnemy(string _file)
    {
        speakerEnemy.clip = Resources.Load<AudioClip>("Audio/Enemy/" + _file);
        speakerEnemy.Play();
    }

    /// <summary>
    /// 播放物品音效
    /// </summary>
    /// <param name="_file">
    /// 在Audio/Effect下路径以及文件名
    /// </param>
    public void PlayEffect(string _file)
    {
        speakerEffect.clip = Resources.Load<AudioClip>("Audio/Effect/" + _file);
        speakerEffect.Play();
    }

    /// <summary>
    /// 播放玩家音效
    /// </summary>
    /// <param name="_file">
    /// 在Audio/Hero下路径以及文件名
    /// </param>
    public void PlayHero(string _file)
    {
        speakerHero.clip = Resources.Load<AudioClip>("Audio/Hero/" + _file);
        speakerHero.Play();
    }
    public void audioJump()
    {
        PlayHero("juming");

    }
    public void audioHurt()
    {
        PlayHero("attacked");
    }
    public void audioAttack()
    {
        int i = Random.Range(1, 6);
        PlayHero("sword_"+i);
    }
    public void audioSkill()
    {
        PlayHero("skillAttacking");
    }
}

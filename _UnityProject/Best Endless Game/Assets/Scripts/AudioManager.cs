using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AudioClipVolume
{
    public AudioClip audioClip;
    public float volume=1f;
}

    public class AudioManager : MonoBehaviour
{
    public AudioClip backgroundMusic;
    public AudioClipVolume[] gameOver;
    public AudioClipVolume[] newBag;
    public AudioClipVolume[] itemGrabed;
    public AudioClipVolume[] itemThrown;
    public AudioClipVolume[] achievement;

    private AudioSource audioSource;
    private AudioManager manager;

    void Awake()
    {
        //DontDestroyOnLoad(gameObject);
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        if(Save.Sound)
        {
            audioSource.volume = 1;
        }
        else
        {
            audioSource.volume = 0;
        }
    }

    public void PlayBackgroundMusic()
    {
        //audioSource.PlayOneShot(backgroundMusic);
    }

    public void PlayGameOverSounds()
    {
        PlayClips(gameOver);
    }

    public void PlayItemGrabbed()
    {
        PlayClips(itemGrabed);
    }

    public void PlayItemThrown()
    {
        PlayClips(itemThrown);
    }

    public void PlayNewBag()
    {
        PlayClips(newBag);
    }

    public void PlayTrophy()
    {
        PlayClips(achievement);
    }

    private void PlayClips(AudioClipVolume[] clips)
    {
        foreach(var clip in clips)
        {
            audioSource.PlayOneShot(clip.audioClip,clip.volume);
        }
    }
}

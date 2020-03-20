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

    private AudioSource audioSource;
    private AudioManager manager;

    void Awake()
    {
        //DontDestroyOnLoad(gameObject);
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayBackgroundMusic()
    {
        audioSource.Stop();
        audioSource.PlayOneShot(backgroundMusic);
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

    private void PlayClips(AudioClipVolume[] clips)
    {
        foreach(var clip in clips)
        {
            audioSource.PlayOneShot(clip.audioClip,clip.volume);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

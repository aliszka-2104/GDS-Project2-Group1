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
    public AudioClip levelMusic;
    public AudioClip rush;
    public AudioClipVolume[] gameOver;
    public AudioClipVolume[] newBag;
    public AudioClipVolume[] itemGrabed;
    public AudioClipVolume[] itemThrown;
    public AudioClipVolume[] achievement;

    public AudioSource soundSource;
    public AudioSource musicSource;
    private AudioManager manager;

    private static AudioManager instance;

    void Awake()
    {
        if (AudioManager.instance == null)
        {
            AudioManager.instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
    }

    public void AdjustSound()
    {
        if (Save.Sound)
        {
            soundSource.mute = false;
        }
        else
        {
            soundSource.mute = true;
        }

        if (Save.Music)
        {
            musicSource.mute = false;
        }
        else
        {
            musicSource.mute = true;
        }
    }

    public void PlayBackgroundMusic()
    {
        musicSource.Stop();
        musicSource.clip = levelMusic;
        musicSource.Play();

        soundSource.Stop();
    }

    public void PlayMenuMusic()
    {
        musicSource.Stop();
        musicSource.clip = backgroundMusic;
        musicSource.Play();

        soundSource.Play();
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
            soundSource.PlayOneShot(clip.audioClip,clip.volume);
        }
    }
    private void Update()
    {
        AdjustSound();
    }
}

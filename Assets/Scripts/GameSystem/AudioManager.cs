using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public Sound[] _musicSound, _sfxSound;
    public AudioSource _musicSource, _sfxSource;

    private void Awake()
    {
        Instance = this;   // No singleton protection
    }

    private void Start()
    {
        PlayMusic("Theme");
    }

    public void PlayMusic(string name)
    {
        Sound s = Array.Find(_musicSound, x => x._name == name);

        if (s == null)
        {
            Debug.Log("Music Not Found");
            return;
        }

        _musicSource.clip = s._clip;
        _musicSource.loop = true;
        _musicSource.Play();
    }

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(_sfxSound, x => x._name == name);

        if (s == null)
        {
            Debug.Log("SFX Not Found");
            return;
        }

        _sfxSource.PlayOneShot(s._clip);
    }

    public void PlayClick()
    {
        PlaySFX("Click");
    }

    public void MenuLoad()
    {
        PlayMusic("Theme");
    }

    public void MusicVolume(float volume)
    {
        _musicSource.volume = volume;
    }

    public void SFXVolume(float volume)
    {
        _sfxSource.volume = volume;
    }
}

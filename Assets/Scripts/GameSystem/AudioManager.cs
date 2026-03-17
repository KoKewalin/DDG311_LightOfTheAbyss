using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Sounds")]
    public Sound[] _musicSound, _sfxSound;

    [Header("Audio Sources")]
    public AudioSource _musicSource, _sfxSource;

    [Header("Volume")]
    [Range(0f, 1f)] public float defaultMusicVolume = 1f;
    [Range(0f, 1f)] public float defaultSFXVolume = 1f;

    private const string MUSIC_KEY = "MusicVolume";
    private const string SFX_KEY = "SFXVolume";

    private string currentMusicName = "";

    private void Awake()
    {
        // Singleton protection
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadVolume();
    }

    private void Start()
    {
        // Only play if nothing is already playing
        if (!_musicSource.isPlaying)
        {
            PlayMusic("Theme");
        }
    }

    public void PlayMusic(string name)
    {
        // Prevent restarting same music again and again
        if (currentMusicName == name && _musicSource.isPlaying)
            return;

        Sound s = Array.Find(_musicSound, x => x._name == name);

        if (s == null)
        {
            Debug.LogWarning("Music Not Found: " + name);
            return;
        }

        currentMusicName = name;
        _musicSource.clip = s._clip;
        _musicSource.loop = true;
        _musicSource.Play();
    }

    public void StopMusic()
    {
        _musicSource.Stop();
        currentMusicName = "";
    }

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(_sfxSound, x => x._name == name);

        if (s == null)
        {
            Debug.LogWarning("SFX Not Found: " + name);
            return;
        }

        _sfxSource.PlayOneShot(s._clip);
    }

    public void PlayClick()
    {
        PlaySFX("Click");
    }

    public void MusicVolume(float volume)
    {
        _musicSource.volume = volume;
        PlayerPrefs.SetFloat(MUSIC_KEY, volume);
        PlayerPrefs.Save();
    }

    public void SFXVolume(float volume)
    {
        _sfxSource.volume = volume;
        PlayerPrefs.SetFloat(SFX_KEY, volume);
        PlayerPrefs.Save();
    }

    public float GetMusicVolume()
    {
        return PlayerPrefs.GetFloat(MUSIC_KEY, defaultMusicVolume);
    }

    public float GetSFXVolume()
    {
        return PlayerPrefs.GetFloat(SFX_KEY, defaultSFXVolume);
    }

    private void LoadVolume()
    {
        _musicSource.volume = PlayerPrefs.GetFloat(MUSIC_KEY, defaultMusicVolume);
        _sfxSource.volume = PlayerPrefs.GetFloat(SFX_KEY, defaultSFXVolume);
    }
}

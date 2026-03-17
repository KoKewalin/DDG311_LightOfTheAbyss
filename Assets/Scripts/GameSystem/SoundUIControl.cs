using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundUIControl : MonoBehaviour
{
    public Slider _musicSlider;
    public Slider _sfxSlider;

    private void Start()
    {
        if (AudioManager.Instance == null)
        {
            Debug.LogWarning("AudioManager not found in scene.");
            return;
        }

        _musicSlider.value = AudioManager.Instance.GetMusicVolume();
        _sfxSlider.value = AudioManager.Instance.GetSFXVolume();

        _musicSlider.onValueChanged.RemoveAllListeners();
        _sfxSlider.onValueChanged.RemoveAllListeners();

        _musicSlider.onValueChanged.AddListener(SetMusicVolume);
        _sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    public void SetMusicVolume(float value)
    {
        AudioManager.Instance.MusicVolume(value);
    }

    public void SetSFXVolume(float value)
    {
        AudioManager.Instance.SFXVolume(value);
    }
}

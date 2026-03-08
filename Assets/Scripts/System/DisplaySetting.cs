using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplaySetting : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Dropdown screenModeDropdown;
    [SerializeField] private TMP_Dropdown resolutionDropdown;

    private Resolution[] resolutions;
    private List<Resolution> filteredResolutions = new List<Resolution>();

    private void Start()
    {
        SetupScreenModeDropdown();
        SetupResolutionDropdown();
        LoadDisplaySettings();
    }

    private void SetupScreenModeDropdown()
    {
        if (screenModeDropdown == null) return;

        screenModeDropdown.ClearOptions();

        List<string> options = new List<string>
        {
            "Windowed",
            "Fullscreen"
        };

        screenModeDropdown.AddOptions(options);
        screenModeDropdown.onValueChanged.AddListener(SetScreenMode);
    }

    private void SetupResolutionDropdown()
    {
        if (resolutionDropdown == null) return;

        resolutionDropdown.ClearOptions();

        resolutions = Screen.resolutions;
        filteredResolutions.Clear();

        List<string> options = new List<string>();
        HashSet<string> added = new HashSet<string>();

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;

            // ¡Ñ¹ resolution «éÓ
            if (!added.Contains(option))
            {
                added.Add(option);
                filteredResolutions.Add(resolutions[i]);
                options.Add(option);
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.onValueChanged.AddListener(SetResolution);
    }

    private void LoadDisplaySettings()
    {
        int savedScreenMode = PlayerPrefs.GetInt("ScreenMode", 1);
        int savedResolutionIndex = PlayerPrefs.GetInt("ResolutionIndex", GetCurrentResolutionIndex());

        if (screenModeDropdown != null)
            screenModeDropdown.value = savedScreenMode;

        if (resolutionDropdown != null)
            resolutionDropdown.value = savedResolutionIndex;

        if (screenModeDropdown != null)
            screenModeDropdown.RefreshShownValue();

        if (resolutionDropdown != null)
            resolutionDropdown.RefreshShownValue();

        ApplyScreenMode(savedScreenMode);
        ApplyResolution(savedResolutionIndex);
    }

    public void SetScreenMode(int index)
    {
        PlayerPrefs.SetInt("ScreenMode", index);
        ApplyScreenMode(index);
        PlayerPrefs.Save();
    }

    public void SetResolution(int index)
    {
        PlayerPrefs.SetInt("ResolutionIndex", index);
        ApplyResolution(index);
        PlayerPrefs.Save();
    }

    private void ApplyScreenMode(int index)
    {
        bool isFullscreen = index == 1;
        Screen.fullScreen = isFullscreen;
    }

    private void ApplyResolution(int index)
    {
        if (index < 0 || index >= filteredResolutions.Count) return;

        Resolution res = filteredResolutions[index];

        bool isFullscreen = Screen.fullScreen;
        Screen.SetResolution(res.width, res.height, isFullscreen);
    }

    private int GetCurrentResolutionIndex()
    {
        for (int i = 0; i < filteredResolutions.Count; i++)
        {
            if (filteredResolutions[i].width == Screen.currentResolution.width &&
                filteredResolutions[i].height == Screen.currentResolution.height)
            {
                return i;
            }
        }

        return Mathf.Max(filteredResolutions.Count - 1, 0);
    }
}

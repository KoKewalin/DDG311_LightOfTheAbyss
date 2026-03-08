using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private GameObject debugFeaturePanel;

    [Header("Keys")]
    [SerializeField] private KeyCode pauseKey = KeyCode.Escape;
    [SerializeField] private KeyCode debugKey = KeyCode.F4;

    private bool isPaused = false;

    private void Start()
    {
        Time.timeScale = 1f;
        isPaused = false;

        if (pausePanel != null)
            pausePanel.SetActive(false);

        if (debugFeaturePanel != null)
            debugFeaturePanel.SetActive(false);
    }

    private void Update()
    {
        // เปิด / ปิด pause menu
        if (Input.GetKeyDown(pauseKey))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }

        // เปิด / ปิด debug feature panel ได้เฉพาะตอน pause
        if (isPaused && Input.GetKeyDown(debugKey))
        {
            ToggleDebugFeaturePanel();
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;

        if (pausePanel != null)
            pausePanel.SetActive(true);

        if (debugFeaturePanel != null)
            debugFeaturePanel.SetActive(false);
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;

        if (pausePanel != null)
            pausePanel.SetActive(false);

        if (debugFeaturePanel != null)
            debugFeaturePanel.SetActive(false);
    }

    public void ToggleDebugFeaturePanel()
    {
        if (debugFeaturePanel == null) return;

        debugFeaturePanel.SetActive(!debugFeaturePanel.activeSelf);
    }

    public bool IsPaused()
    {
        return isPaused;
    }
}

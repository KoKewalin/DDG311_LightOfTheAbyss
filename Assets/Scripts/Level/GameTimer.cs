using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameTimer : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text finishTimeText;

    private float currentTime = 0f;
    private bool isRunning = false;

    private void Update()
    {
        if (!isRunning) return;

        currentTime += Time.deltaTime;
        UpdateTimerUI();
    }

    void UpdateTimerUI()
    {
        if (timerText != null)
            timerText.text = FormatTime(currentTime);
    }

    public void StartTimer()
    {
        currentTime = 0f;
        isRunning = true;
    }

    public void StopTimer()
    {
        isRunning = false;

        if (finishTimeText != null)
            finishTimeText.text = "Time: " + FormatTime(currentTime);
    }

    string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60);
        float seconds = time % 60;

        return string.Format("{0:00}:{1:00.00}", minutes, seconds);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    [SerializeField] private GameObject finishPanel;
    [SerializeField] private GameTimer gameTimer;
    [SerializeField] private GameObject startPanel;

    private bool finished = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (finished) return;

        if (other.CompareTag("Player"))
        {

            finished = true;

            if (gameTimer != null)
                gameTimer.StopTimer();

            if (finishPanel != null)
                finishPanel.SetActive(true);

            Time.timeScale = 0f;

            Debug.Log("Level Finished!");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTrigger : MonoBehaviour
{
    [SerializeField] private GameTimer gameTimer;

    private bool started = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (started) return;

        if (other.CompareTag("Player"))
        {
            started = true;

            if (gameTimer != null)
                gameTimer.StartTimer();

            Debug.Log("Timer Started!");
        }
    }
}

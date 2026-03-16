using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverMmnu : MonoBehaviour
{
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private PlayerRespawn playerRespawn;

    public void RespawnPlayer()
    {
        Time.timeScale = 1f;

        if (gameOverUI != null)
            gameOverUI.SetActive(false);

        if (playerRespawn != null)
            playerRespawn.RespawnPlayer();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [Header("Checkpoint")]
    [SerializeField] private Transform respawnPoint;

    [Header("Lava")]
    [SerializeField] private LavaRaising lava;
    [SerializeField] private Transform lavaStartPoint;

    private bool activated = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (activated) return;

        if (other.CompareTag("Player"))
        {
            activated = true;

            PlayerRespawn respawn = other.GetComponent<PlayerRespawn>();
            if (respawn != null)
            {
                Transform pointToUse = respawnPoint != null ? respawnPoint : transform;
                respawn.SetCheckpoint(pointToUse, lavaStartPoint);
            }

            if (lava != null && lavaStartPoint != null)
            {
                lava.MoveToPosition(lavaStartPoint);
                lava.StartRising();
            }

            Debug.Log("Checkpoint hit - respawn + lava updated");
        }
    }
}

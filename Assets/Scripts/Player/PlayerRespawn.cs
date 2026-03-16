using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [Header("Current Checkpoint")]
    [SerializeField] private Transform currentCheckpoint;
    [SerializeField] private Transform currentLavaPoint;

    [Header("References")]
    [SerializeField] private LavaRaising lava;

    private Rigidbody2D rb;
    private PlayerHealth playerHealth;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerHealth = GetComponent<PlayerHealth>();
    }

    public void SetCheckpoint(Transform checkpoint, Transform lavaPoint)
    {
        currentCheckpoint = checkpoint;
        currentLavaPoint = lavaPoint;

        Debug.Log("Checkpoint set: " + checkpoint.position);
    }

    public void RespawnPlayer()
    {
        if (currentCheckpoint == null)
        {
            Debug.LogWarning("No checkpoint set!");
            return;
        }

        transform.position = currentCheckpoint.position;

        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        if (playerHealth != null)
        {
            playerHealth.ResetHealth();
        }

        if (lava != null && currentLavaPoint != null)
        {
            lava.MoveToPosition(currentLavaPoint);
            lava.StartRising(); // ถ้าอยากให้ respawn แล้ว lava เดินต่อจาก checkpoint นี้
        }

        Debug.Log("Player respawned");
    }
}

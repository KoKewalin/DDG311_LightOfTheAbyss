using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerController player;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Transform winGoal;
    [SerializeField] private GameObject lavaObject;

    [Header("Checkpoint Debug")]
    [SerializeField] private Transform[] checkpoints;

    private bool flyMode = false;
    private bool infiniteJump = false;

    Rigidbody2D rb;

    private void Start()
    {
        rb = player.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (flyMode)
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            rb.velocity = new Vector2(h * 8f, v * 8f);
        }
    }

    public void ToggleFlyMode()
    {
        flyMode = !flyMode;

        if (flyMode)
            rb.gravityScale = 0f;
        else
            rb.gravityScale = 3f;

        Debug.Log("Fly Mode: " + flyMode);
    }

    public void ToggleLava()
    {
        if (lavaObject != null)
        {
            lavaObject.SetActive(!lavaObject.activeSelf);
            Debug.Log("Lava toggled");
        }
    }

    public void InstantHeal()
    {
        if (playerHealth != null)
            playerHealth.Heal(999);
    }

    public void InstantDamage()
    {
        if (playerHealth != null)
            playerHealth.TakeDamage(1);
    }

    public void TeleportToGoal()
    {
        if (winGoal != null)
        {
            player.transform.position = winGoal.position;

            if (rb != null)
            {
                rb.velocity = Vector2.zero;
                rb.angularVelocity = 0f;
            }
        }
    }

    public void ToggleInfiniteJump()
    {
        infiniteJump = !infiniteJump;
        Debug.Log("Infinite Jump: " + infiniteJump);
    }

    public bool IsInfiniteJump()
    {
        return infiniteJump;
    }

    public void TeleportToCheckpoint0()
    {
        TeleportToCheckpointByIndex(0);
    }

    public void TeleportToCheckpoint1()
    {
        TeleportToCheckpointByIndex(1);
    }

    public void TeleportToCheckpoint2()
    {
        TeleportToCheckpointByIndex(2);
    }

    public void TeleportToCheckpoint3()
    {
        TeleportToCheckpointByIndex(3);
    }

    private void TeleportToCheckpointByIndex(int index)
    {
        if (checkpoints == null || checkpoints.Length == 0)
        {
            Debug.LogWarning("No checkpoints assigned in DebugController");
            return;
        }

        if (index < 0 || index >= checkpoints.Length)
        {
            Debug.LogWarning("Checkpoint index out of range: " + index);
            return;
        }

        Transform target = checkpoints[index];

        if (target == null)
        {
            Debug.LogWarning("Checkpoint at index " + index + " is null");
            return;
        }

        player.transform.position = target.position;

        if (rb != null)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        Debug.Log("Teleported to checkpoint index: " + index);
    }
}

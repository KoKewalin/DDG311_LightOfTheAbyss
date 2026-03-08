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

    private bool flyMode = false;
    private bool infiniteJump = false;

    Rigidbody2D rb;

    private void Start()
    {
        rb = player.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Fly mode movement
        if (flyMode)
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            rb.velocity = new Vector2(h * 8f, v * 8f);
        }
    }

    // Player ลอย
    public void ToggleFlyMode()
    {
        flyMode = !flyMode;

        if (flyMode)
            rb.gravityScale = 0f;
        else
            rb.gravityScale = 3f;

        Debug.Log("Fly Mode: " + flyMode);
    }

    // ปิดลาวา
    public void ToggleLava()
    {
        if (lavaObject != null)
        {
            lavaObject.SetActive(!lavaObject.activeSelf);
            Debug.Log("Lava toggled");
        }
    }

    // Instant Heal
    public void InstantHeal()
    {
        if (playerHealth != null)
        {
            playerHealth.Heal(999);
        }
    }

    // Instant Damage
    public void InstantDamage()
    {
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(1);
        }
    }

    // Teleport to Win Goal
    public void TeleportToGoal()
    {
        if (winGoal != null)
        {
            player.transform.position = winGoal.position;
        }
    }

    // Infinite Jump
    public void ToggleInfiniteJump()
    {
        infiniteJump = !infiniteJump;

        if (infiniteJump)
            Debug.Log("Infinite Jump ON");
        else
            Debug.Log("Infinite Jump OFF");
    }

    public bool IsInfiniteJump()
    {
        return infiniteJump;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int maxHP = 2;
    public int CurrentHP { get; private set; }

    [Header("Heart UI")]
    [SerializeField] private Image[] hearts;
    [SerializeField] private Sprite fullHeartSprite;
    [SerializeField] private Sprite emptyHeartSprite;

    [Header("Damage Launch")]
    [SerializeField] private float damageLaunchForce = 10f;
    [SerializeField] private GameObject GameOverUI;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        CurrentHP = maxHP;
        UpdateHeartUI();
    }

    public void TakeDamage(int amount = 1)
    {
        if (CurrentHP <= 0) return;

        CurrentHP -= amount;
        CurrentHP = Mathf.Max(CurrentHP, 0);

        UpdateHeartUI();

        // Screen shake
        CameraFollow camFollow = Camera.main.GetComponent<CameraFollow>();
        if (camFollow != null)
        {
            camFollow.Shake();
        }

        // Launch player upward when damaged
        if (rb != null)
        {
            rb.velocity = new Vector2(rb.velocity.x, damageLaunchForce);
        }

        Debug.Log("Player HP: " + CurrentHP);

        if (CurrentHP <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount = 1)
    {
        if (CurrentHP <= 0) return;

        CurrentHP += amount;
        CurrentHP = Mathf.Min(CurrentHP, maxHP);

        UpdateHeartUI();
    }

    private void UpdateHeartUI()
    {
        if (hearts == null || hearts.Length == 0) return;

        for (int i = 0; i < hearts.Length; i++)
        {
            if (hearts[i] == null) continue;

            if (i < CurrentHP)
                hearts[i].sprite = fullHeartSprite;
            else
                hearts[i].sprite = emptyHeartSprite;
        }
    }

    private void Die()
    {
        Debug.Log("Player died!");
        Time.timeScale = 0f;
        GameOverUI.SetActive(true);
        Destroy(gameObject);
    }
}

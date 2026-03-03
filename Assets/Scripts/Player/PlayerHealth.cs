using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int maxHP = 2;
    public int CurrentHP { get; private set; }

    private void Awake()
    {
        CurrentHP = maxHP;
    }

    public void TakeDamage(int amount = 1)
    {
        if (CurrentHP <= 0) return;

        CurrentHP -= amount;
        CurrentHP = Mathf.Max(CurrentHP, 0);

        // Screen shake
        var camFollow = Camera.main.GetComponent<CameraFollow>();
        if (camFollow != null)
            camFollow.Shake();

        Debug.Log("Player HP: " + CurrentHP);

        if (CurrentHP == 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Player died!");
        // TODO: disable movement / show UI / restart etc.
    }

    public void Heal(int amount = 1)
    {
        if (CurrentHP <= 0) return;
        CurrentHP = Mathf.Min(CurrentHP + amount, maxHP);
    }
}

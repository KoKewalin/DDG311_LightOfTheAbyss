using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaRaising : MonoBehaviour
{
    [Header("Base Settings")]
    [SerializeField] private float startSpeed = 1f;
    [SerializeField] private float acceleration = 0.1f;

    private float currentSpeed;
    void Start()
    {
        currentSpeed = startSpeed;
    }

    void Update()
    {
        // Increase speed over time
        currentSpeed += acceleration * Time.deltaTime;

        // Move lava upward
        transform.position += Vector3.up * currentSpeed * Time.deltaTime;
    }
}

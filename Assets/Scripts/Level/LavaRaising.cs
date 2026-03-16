using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaRaising : MonoBehaviour
{
    [Header("Base Settings")]
    [SerializeField] private float startSpeed = 1f;
    [SerializeField] private float acceleration = 0.1f;

    private float currentSpeed;
    private bool isRising = false;

    void Start()
    {
        currentSpeed = startSpeed;
    }

    void Update()
    {
        if (!isRising) return;

        // Increase speed over time
        currentSpeed += acceleration * Time.deltaTime;

        // Move lava upward
        transform.position += Vector3.up * currentSpeed * Time.deltaTime;
    }

    public void MoveToPosition(Transform targetPoint)
    {
        if (targetPoint == null) return;

        transform.position = targetPoint.position;
        currentSpeed = startSpeed;
        isRising = false;
    }

    public void StartRising()
    {
        currentSpeed = startSpeed;
        isRising = true;
    }

    public void StopRising()
    {
        isRising = false;
    }

    public bool IsRising()
    {
        return isRising;
    }
}

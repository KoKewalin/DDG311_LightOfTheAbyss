using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("General Setting")]
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset = new Vector3(0f, 0f, -10f);

    [Header("Shake")]
    [SerializeField] private float shakeDuration = 0.12f;
    [SerializeField] private float shakeStrength = 0.25f;

    private float shakeTimer;
    private Vector2 shakeOffset;

    private void LateUpdate()
    {
        if (target == null) return;

        // Update shake
        if (shakeTimer > 0f)
        {
            shakeTimer -= Time.deltaTime;
            shakeOffset = Random.insideUnitCircle * shakeStrength;
        }
        else
        {
            shakeOffset = Vector2.zero;
        }

        Vector3 basePos = target.position + offset;
        transform.position = basePos + new Vector3(shakeOffset.x, shakeOffset.y, 0f);
    }

    // Call this when player takes damage
    public void Shake()
    {
        shakeTimer = shakeDuration;
    }
}

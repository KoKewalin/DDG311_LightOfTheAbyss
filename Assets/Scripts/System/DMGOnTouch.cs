using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DMGOnTouch : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        var hp = other.GetComponent<PlayerHealth>();
        if (hp != null)
            hp.TakeDamage(1);
    }
}

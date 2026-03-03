using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJumpAbility : IJumpAbility
{
    private int jumpCount = 0;
    private int maxJumps = 2;

    public bool CanJump(bool isGrounded)
    {
        // If grounded, you can always jump (count will be reset on land anyway)
        if (isGrounded) return true;

        // In air, allow until you reach max jumps
        return jumpCount < maxJumps;
    }

    public void PerformJump(Rigidbody2D rb, float jumpForce, bool isGrounded)
    {
        // Do NOT reset here every time — reset happens on OnLand()
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        jumpCount++;
    }

    public void OnLand()
    {
        jumpCount = 0;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJumpAbility : IJumpAbility
{
    private int jumpCount = 0;
    private int maxJumps = 2;

    public bool CanJump(bool isGrounded)
    {
        if (isGrounded)
            return true;

        return jumpCount < maxJumps - 1;
    }

    public void PerformJump(Rigidbody2D rb, float jumpForce, bool isGrounded)
    {
        if (isGrounded)
            jumpCount = 0;

        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        jumpCount++;
    }

    public void OnLand()
    {
        jumpCount = 0;
    }
}

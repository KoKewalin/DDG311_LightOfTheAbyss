using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJumpAbility : IJumpAbility
{
    private int jumpCount = 0;
    private int maxJumps = 2;
    public void DoubleJump (Rigidbody2D rb, float jumpForce, bool isGrounded)
    {
        if (isGrounded)
            jumpCount = 0;

        if (jumpCount < maxJumps)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount++;
        }
    }
    public void OnLand()
    {
        jumpCount = 0;
    }
}

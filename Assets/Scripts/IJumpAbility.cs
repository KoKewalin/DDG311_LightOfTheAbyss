using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IJumpAbility
{
    bool CanJump(bool isGrounded);
    void PerformJump(Rigidbody2D rb, float jumpForce, bool isGrounded);
    void OnLand();
}

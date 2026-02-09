using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IJumpAbility
{
    void DoubleJump(Rigidbody2D rb, float jumpForce, bool isGrounded);
    void OnLand();
}

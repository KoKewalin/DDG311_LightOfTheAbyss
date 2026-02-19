using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _moveSpeed = 8f;
    [SerializeField] private float _jumpForce = 10f;

    [Header("Dash")]
    [SerializeField] private KeyCode _dashKey = KeyCode.LeftShift;
    [SerializeField] private float _dashForce = 20f;
    [SerializeField] private float _dashDuration = 0.2f;

    [Header("Grab")]
    [SerializeField] private LayerMask _grabLayer;
    [SerializeField] private KeyCode _grabKey = KeyCode.E;
    [SerializeField] private float _grabCheckDistance = 0.5f;


    [Header("Ground Check")]
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private float _groundCheckRadius = .2f;
    [SerializeField] private LayerMask _groundLayer;

    [Header("Combo")]
    [SerializeField] private float _comboBufferTime = 1f;

    private Rigidbody2D rb;
    private IJumpAbility doubleJump;
    private ComboSystem comboSystem;
    private Vector2 lastMovementInput;

    private bool wasGrounded;
    private bool isGrounded;
    private bool isDashing;
    private bool hitPressed;
    private bool isGrabbing;

    private float moveInput;
    private float dashTimer;
    private float originalGravity;

    private void Start()
    {
        comboSystem = new ComboSystem();
        comboSystem.SetBufferTime(_comboBufferTime);
        rb = GetComponent<Rigidbody2D>();
        doubleJump = new DoubleJumpAbility();
        originalGravity = rb.gravityScale;

    }

    private void Update()
    {
        wasGrounded = isGrounded;

        isGrounded = Physics2D.OverlapCircle(
            _groundCheck.position,
            _groundCheckRadius,
            _groundLayer
        );

        if (!wasGrounded && isGrounded)
        {
            doubleJump.OnLand();
        }

        CaptureMoveDir();
        Move();
        HandleDash();
        HandleHit();
        HandleGrab();
        CheckCombos();
        Debug.Log("Buffer: " + comboSystem.GetBufferDebug());
    }

    private void CaptureMoveDir()
    {
        lastMovementInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        comboSystem.UpdateTimer();
    }
    private void Move()
    {
        if (isDashing || isGrabbing) return;

        moveInput = Input.GetAxisRaw("Horizontal");
        //isGrounded = Physics2D.OverlapCircle(_groundCheck.position, _groundCheckRadius, _groundLayer);
        rb.velocity = new Vector2(moveInput * _moveSpeed, rb.velocity.y);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("SPACE PRESSED");

            if (doubleJump.CanJump(isGrounded))
            {
                doubleJump.PerformJump(rb, _jumpForce, isGrounded);
            }

            comboSystem.AddInput(ComboInput.Jump);
        }
    }
    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, _jumpForce);

    }
    private void HandleDash()
    {
        if (Input.GetKeyDown(_dashKey) && !isDashing)
        {
            StartDash();                    // Immediate execution
            comboSystem.AddInput(ComboInput.Dash);  // Still record for combos
        }

        if (isDashing)
        {
            dashTimer -= Time.deltaTime;

            if (dashTimer <= 0)
            {
                isDashing = false;
                rb.gravityScale = originalGravity;
            }
        }
    }
    private void HandleHit()
    {
        if (Input.GetMouseButtonDown(0))
        {
            comboSystem.AddInput(ComboInput.Hit);
            Debug.Log("Hit!");
        }

        //if (Input.GetKeyDown(KeyCode.Mouse0)) // or KeyCode.F
        //{
        //    hitPressed = true;
        //    Debug.Log("Hit triggered");
        //}
        //else
        //{
        //    hitPressed = false;
        //}
    }
    private void StartDash()
    {
        isDashing = true;
        dashTimer = _dashDuration;

        rb.gravityScale = 0f;

        float dir = Mathf.Sign(moveInput);

        if (dir == 0)
            dir = transform.localScale.x >= 0 ? 1 : -1;

        Vector2 dashDir;

        if (isGrounded)
            dashDir = new Vector2(dir, 0);
        else
            dashDir = new Vector2(dir, 1);

        dashDir.Normalize();

        rb.velocity = dashDir * _dashForce;
    }

    private void HandleGrab()
    {
        if (!Input.GetKey(_grabKey))
        {
            StopGrab();
            return;
        }

        if (Input.GetKeyDown(_grabKey))
        {
            comboSystem.AddInput(ComboInput.Grab);
        }

        Vector2 dir = transform.localScale.x > 0 ? Vector2.right : Vector2.left;

        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            dir,
            _grabCheckDistance,
            _grabLayer
        );

        Debug.DrawRay(transform.position, dir * _grabCheckDistance, Color.red);

        if (hit.collider != null)
        {
            StartGrab();
        }
        else
        {
            StopGrab();
        }
    }
    private void StartGrab()
    {
        if (isGrabbing) return;

        isGrabbing = true;
        rb.velocity = Vector2.zero;
        rb.gravityScale = 0f;
    }

    private void StopGrab()
    {
        if (!isGrabbing) return;

        isGrabbing = false;
        rb.gravityScale = originalGravity;
    }

    private void CheckCombos()
    {

        // Dash → Hit
        if (comboSystem.Match(ComboInput.Dash, ComboInput.Hit))
        {
            rb.velocity = new Vector2(0, _dashForce);
            return;
        }

        // Grab alone
        //if (comboSystem.Match(ComboInput.Grab))
        //{
        //    HandleGrab();
        //    return;
        //}
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Vector2 dir = transform.localScale.x > 0 ? Vector2.right : Vector2.left;

        Gizmos.DrawLine(
            transform.position,
            (Vector2)transform.position + dir * _grabCheckDistance
        );
    }
}

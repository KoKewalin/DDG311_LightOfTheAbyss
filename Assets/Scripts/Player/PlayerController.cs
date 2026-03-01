using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _moveSpeed = 8f;
    [SerializeField] private float _jumpForce = 10f;
    [SerializeField] private float _coyoteTime = 0.12f;


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
    [SerializeField] private float _launchForce = 18f;
    [SerializeField] private float _grabLockDuration = 0.15f;


    private Rigidbody2D rb;
    private IJumpAbility doubleJump;
    private ComboSystem comboSystem;
    private Vector2 lastMovementInput;
    private Vector2 comboMovementDirection;
    private Vector2 grabAimDirection;
    private Vector2 _wallNormal;
    private ContactFilter2D _groundFilter;
    private readonly Collider2D[] _groundHits = new Collider2D[8];

    private bool wasGrounded;
    private bool isGrounded;
    private bool isDashing;
    private bool hitPressed;
    private bool isGrabbing;
    private bool _landResetDone;

    private float _coyoteTimer;
    private float _facing = 1f;
    private float _grabLockTimer;
    private float moveInput;
    private float dashTimer;
    private float originalGravity;
    private float previousMoveInput;
    private void Start()
    {
        comboSystem = new ComboSystem();
        comboSystem.SetBufferTime(_comboBufferTime);
        rb = GetComponent<Rigidbody2D>();
        doubleJump = new DoubleJumpAbility();
        originalGravity = rb.gravityScale;
        _groundFilter = new ContactFilter2D();
        _groundFilter.useLayerMask = true;
        _groundFilter.layerMask = _groundLayer;
        _groundFilter.useTriggers = false;

    }

    private void Update()
    {
        wasGrounded = isGrounded;

        int hitCount = Physics2D.OverlapCircle(_groundCheck.position, _groundCheckRadius, _groundFilter, _groundHits);
        isGrounded = hitCount > 0 && rb.velocity.y <= 0.05f;
        if (isGrounded)
            _coyoteTimer = _coyoteTime;
        else
            _coyoteTimer -= Time.deltaTime;

        if (isGrounded && hitCount > 0 && _groundHits[0] != null)
        {
            Debug.Log("Ground hit: " + _groundHits[0].name +
                      " | layer: " + LayerMask.LayerToName(_groundHits[0].gameObject.layer));
        }

        if (isGrounded)
{
        if (!_landResetDone)
        {
            doubleJump.OnLand();
            _landResetDone = true;
        }
        }
        else
        {
            _landResetDone = false;
        }

        if (_grabLockTimer > 0f)
            _grabLockTimer -= Time.deltaTime;

        CaptureMoveDir();
        Move();
        HandleDash();
        HandleHit();
        HandleGrab();
        CheckCombos();
        Debug.Log("Buffer: " + comboSystem.GetBufferDebug());
        Debug.Log("Grounded: " + isGrounded);
    }

    private void CaptureMoveDir()
    {
        lastMovementInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        comboSystem.UpdateTimer();
    }
    private void Move()
    {
        if (isDashing || isGrabbing) return;

        float horizontal = Input.GetAxisRaw("Horizontal");
        if (horizontal > 0.01f) SetFacing(1f);
        else if (horizontal < -0.01f) SetFacing(-1f);
        float vertical = Input.GetAxisRaw("Vertical");

        Vector2 currentInput = new Vector2(horizontal, vertical);

        // Register Movement for combo (any direction)
        if (comboSystem.Contains(ComboInput.Jump) &&
    !comboSystem.Contains(ComboInput.Dash) &&
    currentInput != Vector2.zero)
{
    comboMovementDirection = currentInput.normalized;

    if (!comboSystem.Contains(ComboInput.Movement))
    {
        comboSystem.AddInput(ComboInput.Movement);
    }
}

        // Only horizontal affects actual movement
        rb.velocity = new Vector2(horizontal * _moveSpeed, rb.velocity.y);

        // Jump input
        if (Input.GetKeyDown(KeyCode.Space))
        {
            bool canUseCoyote = _coyoteTimer > 0f;
            bool groundedForJump = isGrounded || canUseCoyote;

            if (doubleJump.CanJump(groundedForJump))
            {
                doubleJump.PerformJump(rb, _jumpForce, isGrounded);

                // Only record Jump if we ACTUALLY jumped
                comboSystem.AddInput(ComboInput.Jump);
            }
        }
    }
    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, _jumpForce);

    }
    private void HandleDash()
    {
        if (Input.GetKeyDown(_dashKey))
        {
            // If grabbing, HandleGrab will handle dash-launch
            if (!isGrabbing)
                comboSystem.AddInput(ComboInput.Dash);
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
            // ✅ If we are currently dashing, convert hit into vertical dash
            if (isDashing)
            {
                StartDash(Vector2.up, overrideDash: true);
                comboSystem.Clear(); // optional: prevents other combos from firing weird
                return;
            }

            comboSystem.AddInput(ComboInput.Hit);
            Debug.Log("Hit!");
        }
    }
    private void StartDash(Vector2 dir, bool overrideDash = false)
    {
        if (isDashing && !overrideDash) return;

        if (dir == Vector2.zero)
            dir = transform.localScale.x > 0 ? Vector2.right : Vector2.left;

        isDashing = true;
        dashTimer = _dashDuration;

        rb.gravityScale = 0f;
        rb.velocity = dir.normalized * _dashForce;
    }

    private void NormalDash()
    {
        if (isDashing) return;

        float dir = Mathf.Sign(Input.GetAxisRaw("Horizontal"));

        if (dir == 0)
            dir = transform.localScale.x > 0 ? 1 : -1;

        Vector2 dashDir = new Vector2(dir, 0);

        isDashing = true;
        dashTimer = _dashDuration;

        rb.gravityScale = 0f;
        rb.velocity = dashDir * _dashForce;
    }

    private void HandleGrab()
    {
        // ✅ Prevent instantly re-grabbing after launching
        if (_grabLockTimer > 0f)
        {
            StopGrab();
            return;
        }

        grabAimDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        if (!Input.GetKey(_grabKey))
        {
            StopGrab();
            return;
        }

        Vector2 dir = transform.localScale.x > 0 ? Vector2.right : Vector2.left;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, _grabCheckDistance, _grabLayer);
        Debug.DrawRay(transform.position, dir * _grabCheckDistance, Color.red);

        if (hit.collider == null)
        {
            StopGrab();
            return;
        }

        // We are touching wall, so we can grab
        StartGrab();

        // Shift while grabbing -> launch
        if (Input.GetKeyDown(_dashKey) && isGrabbing)
        {
            Vector2 launchDir = grabAimDirection;

            if (launchDir == Vector2.zero)
                launchDir = transform.localScale.x > 0 ? Vector2.right : Vector2.left;

            _grabLockTimer = _grabLockDuration;
            StopGrab();
            Launch(launchDir);
            comboSystem.Clear();
            return;
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
    private void SetFacing(float dir)
    {
        if (_facing == dir) return;
        _facing = dir;

        Vector3 s = transform.localScale;
        s.x = Mathf.Abs(s.x) * _facing;
        transform.localScale = s;
    }

    private void CheckCombos()
    {

        // Jump → Hit (Movement can be in between). Launch in last movement direction.
        if (comboSystem.MatchIgnoring(ComboInput.Movement, ComboInput.Jump, ComboInput.Hit))
        {
            if (lastMovementInput != Vector2.zero)
                Launch(lastMovementInput);
            else
                Launch(Vector2.up); // or do nothing

            comboSystem.Clear();
            return;
        }

        // 2️ Jump → Movement → Dash (AIR ONLY DASH)
        if (comboSystem.Match(
            ComboInput.Jump,
            ComboInput.Movement,
            ComboInput.Dash))
        {
            if (!isGrounded)   // 🔥 Only allow in air
            {
                StartDash(comboMovementDirection);
            }

            comboSystem.Clear();
            return;
        }

        //// 3️ Grab → Dash (Player chooses direction)
        //if (comboSystem.Match(
        //    ComboInput.Grab,
        //    ComboInput.Dash))
        //{
        //    Launch(lastMovementInput);
        //    comboSystem.Clear();
        //    return;
        //}

        // 4️ Dash → Hit (Vertical straight dash up)
        if (comboSystem.Match(
            ComboInput.Dash,
            ComboInput.Hit))
        {
            StartDash(Vector2.up);
            comboSystem.Clear();
            return;
        }

        //// 5️ Jump → Jump (Double Jump)
        //if (comboSystem.Match(
        //    ComboInput.Jump,
        //    ComboInput.Jump))
        //{
        //    if (!isGrounded)
        //    {
        //        rb.velocity = new Vector2(rb.velocity.x, _jumpForce);
        //    }

        //    comboSystem.Clear();
        //    return;
        //}
        if (comboSystem.Match(ComboInput.Dash))
        {
            NormalDash();
            comboSystem.Clear();
            return;
        }
    }

    private void Launch(Vector2 dir)
    {
        rb.gravityScale = originalGravity;

        if (dir == Vector2.zero)
            dir = Vector2.up;

        rb.velocity = dir.normalized * _launchForce;
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

    private void OnDrawGizmosSelected()
    {
        // Ground check gizmo
        if (_groundCheck == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_groundCheck.position, _groundCheckRadius);
    }

}

using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement2D : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 8f;
    public float jumpForce = 12f;

    [Header("Ground Check")]
    public Transform groundCheck;            
    public float groundCheckRadius = 0.15f;
    public LayerMask groundLayer;

    [Header("References")]
    public Rigidbody2D rb;
    public SpriteRenderer sr;

    // New Input System
    private InputAction _moveAction;
    private InputAction _jumpAction;

    private Animator animator;

    private void Reset()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;

        // Creates an action that reads WASD + Arrow keys as a Vector2
        _moveAction = new InputAction(type: InputActionType.Value, binding: "<Gamepad>/leftStick");
        _moveAction.AddCompositeBinding("1DAxis")
            .With("Negative", "<Keyboard>/a")
            .With("Negative", "<Keyboard>/leftArrow")
            .With("Positive", "<Keyboard>/d")
            .With("Positive", "<Keyboard>/rightArrow");

        _jumpAction = new InputAction(type: InputActionType.Button);
        _jumpAction.AddBinding("<Keyboard>/space");

        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _moveAction.Enable();
        _jumpAction.Enable();
        _jumpAction.performed += OnJump;
    }
    private void OnDisable()
    {
        _moveAction.Disable();
        _jumpAction.Disable();
        _jumpAction.performed -= OnJump;
    }

    private void FixedUpdate()
    {
        float x = _moveAction.ReadValue<float>();
        rb.linearVelocity = new Vector2(x * moveSpeed, rb.linearVelocity.y);

        if (rb.linearVelocity.x < 0) sr.flipX = true;
        if (rb.linearVelocity.x > 0) sr.flipX = false;

        animator.SetFloat("LinearVelocity", Mathf.Abs(x));
        animator.SetFloat("VerticalVelocity", rb.linearVelocity.y);
        if (!IsGrounded())
        {
            animator.SetBool("isGrounded", false);

        }
        else
        {
            animator.SetBool("isGrounded", true);
        } 
            
    }

    private void OnJump(InputAction.CallbackContext ctx)
    {
        if (!IsGrounded()) return;

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        animator.SetBool("isGrounded", false);


    }

    private bool IsGrounded()
    {
        if (groundCheck == null) return false;
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer) != null;
    }
}
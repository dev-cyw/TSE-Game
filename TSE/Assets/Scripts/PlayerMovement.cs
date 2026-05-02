using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

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

    [Header("Crouch")]
    public float crouchSpeed = 4f;
    public float crouchScale = 0.5f;
    private Vector3 normalScale;
    private bool isCrouching = false;

    [Header("References")]
    public Rigidbody2D rb;
    public SpriteRenderer sr;

    // New Input System
    private InputAction _moveAction;
    private InputAction _jumpAction;
    private InputAction _crouchAction;

    private Animator animator;

    private void Reset()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;

        normalScale = transform.localScale;

        _moveAction = new InputAction(type: InputActionType.Value);
        _moveAction.AddCompositeBinding("1DAxis")
            .With("Negative", "<Keyboard>/a")
            .With("Negative", "<Keyboard>/leftArrow")
            .With("Positive", "<Keyboard>/d")
            .With("Positive", "<Keyboard>/rightArrow");

        //_jumpAction = new InputAction(type: InputActionType.Button);
        //_jumpAction.AddBinding("<Keyboard>/space");

        //_crouchAction = new InputAction(type: InputActionType.Button);
        //_crouchAction.AddBinding("<Keyboard>/c");

        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _moveAction.Enable();
        _jumpAction.Enable();
        _jumpAction.performed += OnJump;

        _crouchAction.Enable();
        _crouchAction.performed += OnCrouchPressed;
        _crouchAction.canceled += OnCrouchReleased;
    }
    private void OnDisable()
    {
        _moveAction.Disable();
        _jumpAction.Disable();
        _jumpAction.performed -= OnJump;

        _crouchAction.Disable();
        _crouchAction.performed -= OnCrouchPressed;
        _crouchAction.canceled -= OnCrouchReleased;
    }

    private void FixedUpdate()
    {
        //float x = _moveAction.ReadValue<float>();

        //float currentSpeed = isCrouching ? crouchSpeed : moveSpeed;
        //rb.linearVelocity = new Vector2(x * moveSpeed, rb.linearVelocity.y);

        if (rb.linearVelocity.x < 0) sr.flipX = true;
        if (rb.linearVelocity.x > 0) sr.flipX = false;
        
        animator.SetFloat("LinearVelocity", Mathf.Abs(rb.linearVelocity.x));
        animator.SetFloat("VerticalVelocity", rb.linearVelocity.y);
        animator.SetBool("isCrouching", isCrouching);
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

        Debug.Log($"Normal Scale before jump: {normalScale}");
        Debug.Log($"Current Scale before jump: {transform.localScale}");

        isCrouching = false;
        UpdateCrouchScale();

        Debug.Log($"Current Scale after jump: {transform.localScale}");

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        animator.SetBool("isGrounded", false);
    }

    private void OnCrouchPressed(InputAction.CallbackContext ctx)
    {
        Debug.Log("Crouch Pressed");
        if (!IsGrounded()) return;  

        isCrouching = true;
        UpdateCrouchScale();
    }

    private void OnCrouchReleased(InputAction.CallbackContext ctx)
    {
        Debug.Log("Crouch Released");
        isCrouching = false;
        UpdateCrouchScale();
    }

    private void UpdateCrouchScale()
    {
        if (isCrouching)
        {
            transform.localScale = new Vector3(normalScale.x, normalScale.y * crouchScale, normalScale.z);
        }
        else
        {
            transform.localScale = normalScale;
        }
    }


    private bool IsGrounded()
    {
        if (groundCheck == null) return false;

        // Cast a ray downward from ground check position
        RaycastHit2D hit = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckRadius, groundLayer);
        return hit.collider != null;
    }

    public void MoveRight(string arg)
    {
        if (float.TryParse(arg, out float distance))
            Enqueue(MoveRightCoroutine(distance));
        else
            Debug.LogError($"Invalid argument for move_right: {arg}");
    }

    private IEnumerator MoveRightCoroutine(float distance)
    {
        float targetX = rb.position.x + distance;
        rb.linearVelocity = new Vector2(moveSpeed, rb.linearVelocity.y);
        while (rb.position.x < targetX)
            yield return new WaitForFixedUpdate();
        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        rb.MovePosition(new Vector2(targetX, rb.position.y));
    }

    public void MoveLeft(string arg)
    {
        if (float.TryParse(arg, out float distance))
            Enqueue(MoveLeftCoroutine(distance));
        else
            Debug.LogError($"Invalid argument for move_left: {arg}");
    }

    private IEnumerator MoveLeftCoroutine(float distance)
    {
        float targetX = rb.position.x - distance;
        rb.linearVelocity = new Vector2(-moveSpeed, rb.linearVelocity.y);
        while (rb.position.x > targetX)
            yield return new WaitForFixedUpdate();
        rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        rb.MovePosition(new Vector2(targetX, rb.position.y));
    }

    public void Jump(string _)
    {
        Enqueue(JumpCoroutine());
    }

    private IEnumerator JumpCoroutine()
    {
        isCrouching = false;
        UpdateCrouchScale();

        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        animator.SetBool("isGrounded", false);

        yield return new WaitUntil(() => IsGrounded());
        animator.SetBool("isGrounded", true);
    }

    public void Crouch(string arg)
    {
        Debug.Log($"Crouch called with: '{arg}'");
        bool.TryParse(arg, out bool isCrouch);
        Enqueue(isCrouch ? CrouchCoroutine() : UnCrouchCoroutine());
    }

    private IEnumerator CrouchCoroutine()
    {
        isCrouching = true;
        UpdateCrouchScale();
        yield break;
    }

    private IEnumerator UnCrouchCoroutine()
    {
        isCrouching = false;
        UpdateCrouchScale();
        yield break;
    }
    
    private Queue<IEnumerator> _commandQueue = new();
    private bool _running = false;

    public void Enqueue(IEnumerator command)
    {
        _commandQueue.Enqueue(command);
        if (!_running) StartCoroutine(RunQueue());
    }

    private IEnumerator RunQueue()
    {
        _running = true;
        while (_commandQueue.Count > 0)
            yield return StartCoroutine(_commandQueue.Dequeue());
        _running = false;
    }
}
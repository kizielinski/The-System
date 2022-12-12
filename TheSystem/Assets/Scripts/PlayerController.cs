using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private LayerMask platformLayerMask;

    // Reference to the Input System
    private PlayerControls playerControls;

    // Reference to the box collider
    BoxCollider2D collisionBox;

    // Input values
    private Vector2 move; // Current move value
    private Vector2 prevMove; // Previous move value

    // Movement Values
    public float jumpSpeed;
    public float moveSpeed;
    public float gravity;
    public float maxSpeed;
    public float maxFallSpeed;

    // Player stats
    private Vector2 vel;
    private Vector2 pos;

    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();

        // Disabling the callback to not have leaks
        playerControls.BasicMovement.Move.performed -= Move;
        playerControls.BasicMovement.Jump.performed -= Jump;
    }

    void Start()
    {
        collisionBox = GetComponent<BoxCollider2D>();

        // Setting up variables
        move = Vector2.zero;
        prevMove = Vector2.zero;

        vel = Vector2.zero;
        pos = transform.position;

        // Setting up callbacks for input
        playerControls.BasicMovement.Move.performed += Move;
        playerControls.BasicMovement.Move.started += Move;
        playerControls.BasicMovement.Jump.performed += Jump;
    }

    private void Update()
    {

    }

    private void FixedUpdate()
    {
        pos += vel;
        transform.position = pos;
        ApplyGravity();
    }

    private void Move(InputAction.CallbackContext context)
    {
        move.x = context.ReadValue<Vector2>().x;

        if(move.x > 0.01f || move.x < -.01f)
        {
            vel.x += move.x * moveSpeed;
            if(vel.x > maxSpeed)
            {
                vel.x = maxSpeed;
            }    
            else if(vel.x < maxSpeed * -1)
            {
                vel.x = maxSpeed * -1;
            }
        }

        Debug.Log("Move");
    }

    private void Jump(InputAction.CallbackContext context)
    {
        if (IsGrounded())
        {
            vel.y = jumpSpeed;
        }
        Debug.Log("Jump");
    }

    private bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(collisionBox.bounds.center, Vector2.down, collisionBox.bounds.extents.y + .01f, platformLayerMask);
        Color rayColor;
        if(hit.collider != null)
        {
            rayColor = Color.green;
        }
        else
        {
            rayColor = Color.red;
        }
        Debug.DrawRay(collisionBox.bounds.center, Vector2.down * (collisionBox.bounds.extents.y + .01f), rayColor);
        return hit.collider != null;
    }

    private void ApplyGravity()
    {
        if(IsGrounded())
        {
            vel.y = 0;
        }
        else
        {
            vel.y -= gravity;
            if(vel.y < maxFallSpeed)
            {
                vel.y = maxFallSpeed;
            }
        }
    }
}

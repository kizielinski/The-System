using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
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
    public float drag;
    public float maxSpeed;
    public float maxFallSpeed;

    // Player stats
    [SerializeField]
    private Vector2 vel;
    private Vector2 pos;

    //Set values for player movement (these will change per character)
    private void Awake()
    {
        gravity = -9.81f;
        maxFallSpeed = -10f;
        jumpSpeed = 5.0f;
        moveSpeed = 3.0f;
        maxSpeed = 8.0f;
        drag = 0.7f;
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
        //playerControls.BasicMovement.Move.performed -= Move;
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
        //playerControls.BasicMovement.Move.performed += Move;
        //playerControls.BasicMovement.Move.started += Move;
        playerControls.BasicMovement.Jump.performed += Jump;
    }

    private void FixedUpdate()
    {
        ApplyGravityAndDrag();
        HorizontalMovement();
        pos.y += vel.y * Time.deltaTime;
        pos.x += vel.x;
        transform.position = pos;
    }

    /* Deprecated, couldn't find a way to have smooth movement work
     * Also InputAction Callback is for single actions 
     * (Attacking, interaction, spell, charging an attack or spell for different effects etc)
     * */

    //private void Move(InputAction.CallbackContext context)
    //{
    //    move.x = context.ReadValue<Vector2>().x;
    //    if (move.x > 0.01f || move.x < -.01f)
    //    {
    //        vel.x += move.x * moveSpeed;
    //        if (vel.x > maxSpeed)
    //        {
    //            vel.x = maxSpeed;
    //        }
    //        else if (vel.x < maxSpeed * -1)
    //        {
    //            vel.x = maxSpeed * -1;
    //        }
    //    }

    //    Debug.Log("Move");
    //    Debug.LogWarning(vel);
    //}

    /// <summary>
    /// This function does basic horizontal movement separate from any characters or abilities
    /// It only looks for and applies movement based on input keys.
    /// </summary>
    /// 
    /// <note>
    /// A state will have to be added in order to have the player affected by alternate movement, or
    /// and adjustment at the end of the function. 
    /// </note>
    private void HorizontalMovement()
    {
        //Right
        if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.L))
        {
            if (vel.x == 0)
            {
                vel.x += moveSpeed * 1.1f * Time.deltaTime;
            }
            else
            {
                vel.x = 0;
                vel.x += moveSpeed * Time.deltaTime;
            }

            return;
        }

        //Left
        if(Input.GetKey(KeyCode.J) || Input.GetKey(KeyCode.A))
        {
            if (vel.x == 0)
            {
                vel.x -= moveSpeed * 1.1f * Time.deltaTime;
            }
            else
            {
                vel.x = 0;
                vel.x -= moveSpeed * Time.deltaTime;
            }

            return;
        }

        //This can be smoothed out if we want instead of an instant stop
        vel.x = 0;
    }

    //Would recommend reworking this into a KeyPress based function as well
    private void Jump(InputAction.CallbackContext context)
    {
        if (IsGrounded())
        {
            vel.y += jumpSpeed;
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

    /// <summary>
    /// Function that applies gravity and player drag so the player doesn't feel super stiff
    /// Feel free to adjust numbers.
    /// </summary>
    
    private void ApplyGravityAndDrag()
    {
        //if(IsGrounded())
        //{
        //    vel.y = 0;
        //}
        vel.y += gravity * Time.deltaTime;
        if(vel.y < maxFallSpeed)
        {
            vel.y = maxFallSpeed;
        }

        if (vel.x < -0.001f)
        {
            vel.x += drag * Time.deltaTime;
        }
        else if (vel.x > 0.0001f)
        {
            vel.x += -drag * Time.deltaTime;
        }
        else
        {
            vel.x = 0;
        }

    }

    //How ground collision should be handled

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "ground")
        {
            vel.y = 0;
            gravity = 0;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag == "ground")
        {
            gravity = -9.81f;
        }
    }
}

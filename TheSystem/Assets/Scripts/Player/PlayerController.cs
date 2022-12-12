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

    // Movement Values
    private float jumpSpeed;
    private float moveSpeed;
    private float gravity;
    private float defaultGravity;
    private float drag;
    private float maxSpeed;
    private float maxFallSpeed;

    // Player stats
    [SerializeField]
    private Vector2 vel;
    private Vector2 pos;
    private int health;
    private int maxHealth;

    //Set values for player movement (these will change per character)
    private void Awake()
    {
        gravity = -15;
        defaultGravity = -15;
        maxFallSpeed = -10f;
        jumpSpeed = 5.0f;
        moveSpeed = 3.0f;
        maxSpeed = 8.0f;
        drag = 0.7f;
        playerControls = new PlayerControls();

        health = 3;
        maxHealth = 3;
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    void Start()
    {
        collisionBox = GetComponent<BoxCollider2D>();

        vel = Vector2.zero;
        pos = transform.position;
    }

    private void FixedUpdate()
    {
        ApplyGravityAndDrag();
        DetectInput();
        pos.y += vel.y * Time.deltaTime;
        pos.x += vel.x;
        transform.position = pos;
    }

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


    private void Jump()
    {
        if (IsGrounded() && Input.GetKey(KeyCode.Space))
        {
            vel.y += jumpSpeed;
        }
    }

    private void DetectInput()
    {
        HorizontalMovement();
        Jump();
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
        vel.y += gravity * Time.deltaTime;
        if (vel.y < maxFallSpeed)
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
            gravity = defaultGravity;
        }
    }

    /// <summary>
    /// Removes a health point from the player
    /// if the player "dies" then it forces a jump
    /// </summary>
    private void TakeDamage()
    {
        health -= 1;
        if(health <= 0)
        {
            Jump();
            health = maxHealth;
        }
    }
}

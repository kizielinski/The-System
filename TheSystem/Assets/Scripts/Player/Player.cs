    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class Player : MonoBehaviour
{
    // Variables that can be set in the inspector related to physics
    public float jumpHeight = 2;
    public float timeToJumpApex = .3f;
    public float accelerationTimeAirborne = .07f;
    public float accelerationTimeGrounded = .08f;
    public float moveSpeed = 8;

    // Calculated values based on the variables above
    float gravity;
    float jumpVelocity;
    Vector3 velocity;
    float velocityXSmoothing;

    //velocity getter
    public Vector3 Velocity
    {
        get { return velocity; }
    }

    // Reference to the controller script
    PlayerController controller;

    //reference to the ledge grab scrip
    LedgeGrabber grabber;

    /// <summary>
    /// Gets a reference to the Controller script and calculates the gravity and jumpVelocity values
    /// </summary>
    void Start()
    {
        grabber = GetComponent<LedgeGrabber>();
        controller = GetComponent<PlayerController>();

        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity * timeToJumpApex);
    }

    /// <summary>
    /// Handles player input
    /// </summary>
    void Update()
    {
        // Resets the vertical velocity if the player touches a ceiling or floor or is grabbing a ledge
        if(controller.collisions.above || controller.collisions.below || grabber.CanGrabLedge)
        {
            velocity.y = 0;
        }

        // Gets the current player axis input
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        // Checks if the player performed a jump
        if((Input.GetKeyDown(KeyCode.Space) && controller.collisions.below))
        {
            velocity.y = jumpVelocity;
        }
        else if(Input.GetKeyDown(KeyCode.Space)&&grabber.CanGrabLedge)
        {
            if (controller.collisions.left)
            {
                transform.position += new Vector3(0.1f, 0f);
            }
            else
            {
                transform.position += new Vector3(-0.1f, 0f);
            }
            velocity.y = jumpVelocity;
        }

        // Smooths out horizontal movement
        float targetVelocityX = input.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);

        // Updates the player's velocity
        if(!grabber.CanGrabLedge)
        {
            velocity.y += gravity * Time.deltaTime;
        }
        controller.Move(velocity * Time.deltaTime);
    }
}

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

    //reference to the wall slide script
    WallSlide slider;

    /// <summary>
    /// Gets a reference to the Controller script and calculates the gravity and jumpVelocity values
    /// </summary>
    void Start()
    {
        //gets the script
        grabber = GetComponent<LedgeGrabber>();
        slider = GetComponent<WallSlide>();
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
        if(controller.collisions.above || controller.collisions.below || grabber.IsGrabbingLedge)
        {
            velocity.y = 0;
        }

        // Gets the current player axis input
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        // Checks if the player can perform a jump
        if(Input.GetKeyDown(KeyCode.Space) && controller.collisions.below 
            || (Input.GetKeyDown(KeyCode.Space) && slider.IsWallSliding && slider.ResetJump)
            || (Input.GetKeyDown(KeyCode.Space) && grabber.IsGrabbingLedge))
        {
            velocity.y = jumpVelocity;
        }

        // Smooths out horizontal movement
        float targetVelocityX = input.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);

        // Updates the player's velocity

        //if the player is wall sliding
        if (slider.IsWallSliding)
        {
            //fall at fixed velocity
            Debug.Log("sliding");
            velocity.y = (gravity * 25) * Time.deltaTime;
        }
        //otherwise if the player is not grabbing a ledge
        else if (!grabber.IsGrabbingLedge)
        {
            //fall normally
            velocity.y += gravity * Time.deltaTime;
        }
        controller.Move(velocity * Time.deltaTime);
    }
}

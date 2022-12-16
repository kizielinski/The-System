using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller))]
public class Player : MonoBehaviour
{
    // Variables that can be set in the inspector related to physics
    public float jumpHeight = 4;
    public float timeToJumpApex = .4f;
    public float accelerationTimeAirborne = .2f;
    public float accelerationTimeGrounded = .1f;
    public float moveSpeed = 6;

    // Calculated values based on the variables above
    float gravity;
    float jumpVelocity;
    Vector3 velocity;
    float velocityXSmoothing;

    // Reference to the controller script
    Controller controller;

    /// <summary>
    /// Gets a reference to the Controller script and calculates the gravity and jumpVelocity values
    /// </summary>
    void Start()
    {
        controller = GetComponent<Controller>();

        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity * timeToJumpApex);
    }

    /// <summary>
    /// Handles player input
    /// </summary>
    void Update()
    {
        // Resets the vertical velocity if the player touches a ceiling or floor
        if(controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }

        // Gets the current player axis input
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        // Checks if the player performed a jump
        if(Input.GetKeyDown(KeyCode.Space) && controller.collisions.below)
        {
            velocity.y = jumpVelocity;
        }

        // Smooths out horizontal movement
        float targetVelocityX = input.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);

        // Updates the player's velocity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}

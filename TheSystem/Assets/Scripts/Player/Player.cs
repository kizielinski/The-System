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
    private float knockBackTime = 0.750f;
    private bool isDamaged = false;

    // Calculated values based on the variables above
    float gravity;
    float jumpVelocity;
    Vector3 velocity;
    float velocityXSmoothing;

    public bool IsDamaged
    {
        get { return isDamaged; }
        set { isDamaged = value; }
    }

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

    /// <summary>
    /// Handles player knockback on player taking damage or hitting an enviroment hazard
    /// </summary>
    /// <param name="attackDirectionVector">
    /// Vector passed in from enemy so orientation can be ascertained.
    /// </param>
    /// <returns></returns>
    private IEnumerator PlayerKnockback(float attackDirectionVector, float damage)
    {
        isDamaged = true;
        float valueMS = moveSpeed;
        moveSpeed = 0;

        float directionKnockback = (attackDirectionVector > 0) ? -1 : 1;

        if (damage <= 0)
        {
            damage = 1;
        }

        Vector3 knockBack = new Vector3(directionKnockback * ((damage * 30)), (damage * 5), 0);

        //Knockback Code
        velocity = knockBack;

        yield return new WaitForSeconds(knockBackTime);

        moveSpeed = valueMS;
        isDamaged = false;
    }
    
    /// <summary>
    /// Makes player take damage and calls knockback function instantly giving player a couple iFrames
    /// </summary>
    /// <param name="damageVector">
    /// Vector passed in from enemy so orientation can be ascertained.
    /// </param>
    public void PlayerTakeDamage(float damageVector, float damage)
    {
        //Knockback stuff
        if(!isDamaged)
        {
            StartCoroutine(PlayerKnockback(damageVector, damage));
        }
        
        //Blah blah blah code etc.
    }
}
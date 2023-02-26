using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class Player : MonoBehaviour
{
    public static Player instance;
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



    //temporary grapple stuff sorry
    public LineRenderer grapple;
    public bool isGrappleOn;
    public float newPointX;
    public float newPointY;
    float distance;
    Vector2 newPos;
    RaycastHit2D hit;
    Ray zipRay;
    //temporary test case, will be removed in final 
    public Camera cameraObj;




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
        if(instance == null)
        {
            instance = this;
        }
        grapple.enabled = false;
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
        if (controller.collisions.above || controller.collisions.below || grabber.IsGrabbingLedge)
        {
            velocity.y = 0;
        }

        // Gets the current player axis input
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        // Checks if the player can perform a jump
        if (Input.GetKeyDown(KeyCode.Space) && controller.collisions.below
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


        /* the old grapple that i worked on that didnt make the cut i'm still mad about that if you're reading
         * this Karl I hate you just kidding you're my boss i love you i understand why it didn't make it 
         * please god don't fire me hahahahah
          
        //temporary grapple code i needed velocity sorry
        if(isGrappleOn)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                newPos = (Vector2)cameraObj.ScreenToWorldPoint(Input.mousePosition);
                grapple.SetPosition(0, newPos);
                grapple.SetPosition(1, transform.position);
                distance = Mathf.Sqrt(Mathf.Pow((transform.position.x - newPos.x), 2) + Mathf.Pow((transform.position.y - newPos.y), 2));

                grapple.enabled = true;
            }
            else if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                grapple.enabled = false;
            }

            if (grapple.enabled)
            {
                grapple.SetPosition(1, transform.position);

                if (Vector2.Distance(transform.position, newPos) > distance)
                {
                    velocity.x -= 20 * (transform.position.x - newPos.x) * Time.deltaTime;
                }

                if (Vector2.Distance(transform.position, newPos) > distance)
                {
                    velocity.y -= 20 * (transform.position.y - newPos.y) * Time.deltaTime;
                }
            }
        }
        */

        //if its been allowed to work
        if (isGrappleOn)
        {
            //if you click
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                
                //calculate where the new position is based on the player and direction faced
                newPos = transform.position + new Vector3(newPointX * Mathf.Sign(velocity.x), newPointY);
                


                //set the line positions to draw
                grapple.SetPosition(0, newPos);
                grapple.SetPosition(1, transform.position);

                //calculate the distance between the player position and the new position
                distance = Mathf.Sqrt(Mathf.Pow((transform.position.x - newPos.x), 2) + Mathf.Pow((transform.position.y - newPos.y), 2));

                //draw the line renderer
                grapple.enabled = true;
            }
            else if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                //if you release mouse1 then stop drawing the line
                grapple.enabled = false;
            }

            //if the line is being drawn
            if (grapple.enabled)
            { 
                //set the position
                grapple.SetPosition(1, transform.position);

                //calculate a zip vector based on position and new position
                Vector3 zipVector = new Vector3(0, 0, 0)-transform.localPosition + (Vector3)newPos;
                
                
                /* dis shit fuckin sucks bruh
                zipRay = new Ray(transform.position, zipVector);
                Debug.DrawRay(transform.position, zipVector);
                if (Physics2D.Raycast(zipRay.origin, zipVector))
                {
                    hit = (Physics2D.Raycast(zipRay.origin, zipVector));
                    Debug.Log(hit.collider);
                    if (hit.collider.tag == "ground")
                    {
                        newPos.x = Mathf.Cos(hit.distance);
                        newPos.y = Mathf.Sin(hit.distance);
                    }
                }
                */

               //if the player is grounded
                if(controller.collisions.below)
                {
                    //bump them up a little bit because the grapple wasnt a fan of the player being stuck to the ground
                    transform.position += new Vector3(0, 0.01f);
                }
                //if the player has reached the new position (close enough to it)
                if(Vector2.Distance(transform.position, newPos)<distance/10)
                {
                    //set their position to the new position and zero out their velocity
                    transform.position = newPos;
                    velocity.x = 0;
                    velocity.y = 0;
                }
                else
                {
                    //otherwise just keep adding velocities based on the calculated direction (zipvector)
                    velocity.x += (100*zipVector.x * Time.deltaTime);
                    velocity.y += (50*zipVector.y * Time.deltaTime);

                    /* other possible way of zipping to point you can mess with in the y direction, 
                     * decide which one is best
                    velocity.y = 0;
                    velocity.y += (1000 * zipVector.y * Time.deltaTime);
                    */
                }
                //currently drawing the ray instead of the linerenderer for the sake of making sure zip rays would be correct
                Debug.DrawRay(transform.position, zipVector);
            }
            
        }

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
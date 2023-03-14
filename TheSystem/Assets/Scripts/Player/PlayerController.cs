using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//test commit
public class PlayerController : RaycastController
{
    /// <summary>
    /// Keeps tracks of the current collision states
    /// </summary>
    public struct CollisionInfo
    {
        [SerializeField] public bool above, below;
        [SerializeField] public bool left, right;

        public bool climbingSlope;
        public bool descendingSlope;
        public float slopeAngle, slopeAngleOld;
        public Vector3 velocityOld;

        public void Reset()
        {
            above = below = false;
            left = right = false;
            climbingSlope = false;
            descendingSlope = false;

            slopeAngleOld = slopeAngle;
            slopeAngle = 0;
        }
    }

    [SerializeField] bool above;
    [SerializeField] bool below;
    [SerializeField] bool left;
    [SerializeField] bool right;
    [SerializeField] bool climbingSlope;
    [SerializeField] bool descendingSlope;
    [SerializeField] public bool spacePressed;
    [SerializeField] public bool grabbingLedge;

    // The maximum angle of a slope that the player can climb
    float maxClimbAngle = 80;
    float maxDescendAngle = 75;

    // Collision info struct definition
    [SerializeField] public CollisionInfo collisions;

    //reference to the ledge grab script
    LedgeGrabber grabber;

    /// <summary>
    /// Get's the player's collision box and calculates the ray spacing for the raycasting
    /// </summary>
    public override void Start()
    {
        base.Start();

        grabber = GetComponent<LedgeGrabber>();
    }

    public void Update()
    {
        above = collisions.above;
        below = collisions.below;
        left = collisions.left;
        right = collisions.right;
        climbingSlope = collisions.climbingSlope;
        descendingSlope = collisions.descendingSlope;
        spacePressed = Input.GetKey("space");
        grabbingLedge = grabber.IsGrabbingLedge;
    }

    /// <summary>
    /// Checks for collisions horizontally and verically
    /// before moving the player to it's expected location
    /// </summary>
    /// <param name="velocity">The player's current velocity after user input</param>
    public void Move(Vector3 velocity, bool standingOnPlatform = false)
    {
        // Updates the four corners of the raycast box
        UpdateRaycastOrigins();

        // Resets the current collision states
        collisions.Reset();

        collisions.velocityOld = velocity;

        if(velocity.y < 0)
        {
            DescendSlope(ref velocity);
        }

        // Check for horizontal collisions if the player is moving horizontally
        if (velocity.x != 0)
        {
            HorizontalCollisions(ref velocity);
        }
        // Check for vertical collisions if the player is moving vertically
        if (velocity.y != 0)
        {
            VerticalCollisions(ref velocity);
        }

        // Moves the player after necessary alterations are done to the velocity
        transform.Translate(velocity);

        if(standingOnPlatform)
        {
            collisions.below = true;
        }
    }

    /// <summary>
    /// Checks for horizontal collisions
    /// Determines whether or not the slope of the object that is collided with is climbable
    ///     and climbs the slope at max horizontal speed
    /// </summary>
    /// <param name="velocity">The player's current velocity after user input</param>
    void HorizontalCollisions(ref Vector3 velocity)
    {
        // Gets the direction of the movement
        float directionX = Mathf.Sign(velocity.x);
        // Gets the length of the ray necessary to check for collisions
        float rayLength = Mathf.Abs(velocity.x) + skinWidth;

        // Loops through rays to cast looking for a collision
        for (int i = 0; i < verticalRayCount; i++)
        {
            // Sets the origin of the first ray to be on the left or right depending on what direction the player is facing
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            // Offsets each ray by the correct spacing amount
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            // Casts a ray and stores the result of the cast
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            // Draws the rays being cast
            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

            // Steps to take after a raycast has hit
            if (hit)
            {
                if (hit.distance == 0)
                {
                    continue;
                }

                // Gets the angle of the hit object
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

                // Checking if the player can climb the slope
                if(i == 0 && slopeAngle <= maxClimbAngle)
                {
                    // Check if the player was previously descending a slope
                    if (collisions.descendingSlope)
                    {
                        collisions.descendingSlope = false;
                        velocity = collisions.velocityOld;
                    }

                    // checking if the slope climb was started last frame
                    float distanceToSlopeStart = 0;
                    if(slopeAngle != collisions.slopeAngleOld)
                    {
                        // Brings the player to be touching the slope
                        distanceToSlopeStart = hit.distance - skinWidth;
                        velocity.x -= distanceToSlopeStart * directionX;
                    }
                    // Performs calculations to climb the slope
                    ClimbSlope(ref velocity, slopeAngle);
                    velocity.x += distanceToSlopeStart * directionX;
                }

                // Things to do when the player can't climb the new slope
                if (!collisions.climbingSlope || slopeAngle > maxClimbAngle)
                {
                    // Stop the player from going through the wall
                    velocity.x = (hit.distance - skinWidth) * directionX;
                    rayLength = hit.distance;

                    // If the player was climbing a slope offset the vertical movement so it doesn't bounce
                    if (collisions.climbingSlope)
                    {
                        velocity.y = Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x);
                    }

                    // Keep track of what side of the player the collision is on
                    collisions.left = directionX == -1;
                    collisions.right = directionX == 1;
                }
            }
        }
    }

    /// <summary>
    /// Checks for Vertical collisions
    /// </summary>
    /// <param name="velocity">The player's current velocity after user input</param>
    void VerticalCollisions(ref Vector3 velocity)
    {
        // Gets the direction of movement
        float directionY = Mathf.Sign(velocity.y);
        // Gets the length of the ray necessary to check for collisions
        float rayLength = Mathf.Abs(velocity.y) + skinWidth;

        // Casts rays and performs logic based on whether or not there was a hit
        for (int i = 0; i < verticalRayCount; i++)
        {
            // Sets the origin of the first ray to be on the top or bottom depending on what direction the player is moving
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);

            // Casts a ray and stores the hit value
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

            // Draws the cast rays
            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

            // Logic to perform on a hit
            if (hit)
            {
                // Stop the player right before they go through the object
                velocity.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance;

                // If the player was climbing a slope offset the horizontal movement so it stays against the slope
                if (collisions.climbingSlope)
                {
                    velocity.x = velocity.y / Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(velocity.x);
                }

                // Keep track of what side of the player the collision was on
                collisions.below = directionY == -1;
                collisions.above = directionY == 1;
            }

            if (collisions.climbingSlope)
            {
                float directionX = Mathf.Sign(velocity.x);
                rayLength = Mathf.Abs(velocity.x) + skinWidth;
                rayOrigin = ((directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight) + Vector2.up * velocity.y;
                hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

                if (hit)
                {
                    float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                    if(slopeAngle != collisions.slopeAngle)
                    {
                        velocity.x = (hit.distance - skinWidth) * directionX;
                        collisions.slopeAngle = slopeAngle;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Performs the calculations to walk up a slope at the same horizontal speed as flat terrain
    /// </summary>
    /// <param name="velocity"></param>
    /// <param name="slopeAngle"></param>
    void ClimbSlope(ref Vector3 velocity, float slopeAngle)
    {
        // Determines the total horizontal distance it is supposed to move up the slope
        float moveDistance = Mathf.Abs(velocity.x);
        // Calculates how far up the slope it has to move in order to be at the correct X position
        float climbVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;

        // Checks if the player is not jumping and climbs the slope
        if (velocity.y <= climbVelocityY)
        {
            velocity.y = climbVelocityY;
            velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
            collisions.below = true;
            collisions.climbingSlope = true;
            collisions.slopeAngle = slopeAngle;
        }
    }

    void DescendSlope(ref Vector3 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomRight : raycastOrigins.bottomLeft;
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, -Vector2.up, Mathf.Infinity, collisionMask);

        if (hit)
        {
            float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
            if(slopeAngle != 0 && slopeAngle <= maxDescendAngle)
            {
                if(Mathf.Sign(hit.normal.x) == directionX)
                {
                    if (hit.distance - skinWidth <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x))
                    {
                        float moveDistance = Mathf.Abs(velocity.x);
                        float descendVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
                        velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
                        velocity.y -= descendVelocityY;

                        collisions.slopeAngle = slopeAngle;
                        collisions.descendingSlope = true;
                        collisions.below = true;
                    }
                }
            }
        }
    }
}
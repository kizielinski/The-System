using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public struct RayCastOrigins
{
    public Vector2 topLeft, topRight, bottomLeft, bottomRight;
}

public struct CollisionDirections
{
    public bool above, below, right, left;
    public void Reset()
    {
        above = below = right = left = false;
    }
}

public class PlayerController : MonoBehaviour
{
    // The Rigidbody of the object
    private Rigidbody rb;

    // Velocity and Position
    private Vector3 pos;
    private Vector2 vel;

    // Player Collision Values
    #region Collisons
    [SerializeField]
    LayerMask collisionLayers;
    new BoxCollider2D collider;
    [SerializeField]
    Bounds bounds => collider.bounds;
    public Bounds Bounds => bounds;
    CollisionDirections collisionDirs;
    public CollisionDirections CollisionsDirs { get => collisionDirs; }
    RayCastOrigins rayOrigins;
    public RayCastOrigins RayOrigins { get => rayOrigins; }
    const float skinWidth = 0.015f;
    [SerializeField]
    int horizontalRayCount = 3;
    [SerializeField]
    int verticalRayCount = 3;
    private float horizontalRaySpacing;
    private float verticalRaySpacing;
    #endregion

    // Bools for current player properties
    #region Properties
    bool canMove;
    public bool CanMove { get => canMove; set => canMove = value; }
    public bool CanFall { set => canFall = value; }
    [SerializeField]
    bool canFall;
    bool isGrounded;
    public bool IsGrounded { get => isGrounded; }

    bool isFacingRight;
    public bool IsFacingRight { get => isFacingRight; set => isFacingRight = value; }

    bool IsJumping { set => isJumping = value; }
    [SerializeField]

    bool isJumping;
    [SerializeField]

    bool isDashing;
    [SerializeField]

    bool dashReady;
    [SerializeField]

    bool canOnlyDashOnGround;
    [SerializeField]

    bool overrideDashTime;
    [SerializeField]

    float dashTime;
    [SerializeField]

    float dashCooldown;
    [SerializeField]

    bool stopHorizontalMomentumOnRelease;
    #endregion

    // Parameters for physics
    #region Physics Values
    [SerializeField]
    Vector2 velocity;
    public Vector2 Velocity { get => velocity; set => velocity = value; }
    [SerializeField]
    float gravity;
    [SerializeField]
    float maxFallSpeed;
    [SerializeField]
    float maxHorizontalSpeed;
    public float MaxHorizontalSpeed { get => maxHorizontalSpeed; set => maxHorizontalSpeed = value; }
    [SerializeField]
    float walkAcceleration;
    [SerializeField]
    float groundDecelerationTime;
    [SerializeField]
    float airDecelerationTime;
    [SerializeField]
    float horizontalInput;
    public float HorizontalInput { get => horizontalInput; set => horizontalInput = value; }
    [SerializeField]
    float jumpHeight;
    [SerializeField]
    float timeToMaxHeight;
    [SerializeField]
    float jumpVelocity;
    [SerializeField]
    float dashVelocity;
    [SerializeField]
    float knockbackScalar;
    [SerializeField]
    float knockbackDuration;
    float smoothVel = 0.0F;
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        canMove = true;

        CalculateGravityAndJump();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Horizontal movement code
        #region Horizontal Movement
        if(canMove)
        {
            if (isDashing)
            {
                velocity.x = dashVelocity * (isFacingRight ? 1 : -1);
            }
            else
            {
                velocity.x += horizontalInput * walkAcceleration * Time.deltaTime;
                //clamp the max speed of the player
                velocity.x = Mathf.Clamp(velocity.x, -maxHorizontalSpeed, maxHorizontalSpeed);
                //flip the player if their direction changes
                if (velocity.x < 0 && isFacingRight ||
                    velocity.x > 0 && !isFacingRight)
                {
                    Flip();
                }
            }
        }

        //slow the player over time after they have let go of the horizontal movement keys
        if (!stopHorizontalMomentumOnRelease &&
        (horizontalInput == 0 || !canMove) &&
        velocity.x != 0.0f &&
        !isDashing)
        {
            velocity.x = Mathf.SmoothDamp(velocity.x, 0.0f, ref smoothVel, velocity.x / maxHorizontalSpeed * (isGrounded ? groundDecelerationTime : airDecelerationTime));
            /* if (Mathf.Approximately(velocity.x, 0.0f))
                velocity.x = 0.0f; */
        }
        #endregion

        #region Vertical Movement
        if (!isGrounded && canFall)
        {
            // Apply gravity to the player
            velocity.y += gravity * Time.deltaTime;

            velocity.y = Mathf.Clamp(velocity.y, -maxFallSpeed, Mathf.Infinity);
        }
        #endregion

        if (collisionDirs.below)
        {
            Land();
        }
        else if (collisionDirs.above)
        {
            velocity.y = 0;
        }
        if (collisionDirs.right || collisionDirs.left)
        {
            velocity.x = 0;
        }

        //UpdateRayCastOrigins();
        collisionDirs.Reset();

        Vector2 newVel = velocity * Time.deltaTime;

        if (velocity.x != 0)
            //HorizontalCollisions(ref newVel);
        if (velocity.y != 0)
            //VerticalCollisions(ref newVel);
        transform.Translate(newVel);
        //isGrounded = GroundCheck();
    }

    [ContextMenu("Calculate Gravity & Jump")]
    void CalculateGravityAndJump()
    {
        gravity = -2 * jumpHeight / Mathf.Pow(timeToMaxHeight, 2);
        jumpVelocity = -gravity * timeToMaxHeight;
    }

    void Flip()
    {
        IsFacingRight = !IsFacingRight;
        Vector3 localscale = transform.localScale;
        localscale.x *= -1;
        transform.localScale = localscale;
    }

    /// <summary>
    /// Updates the player state when they land on the ground
    /// </summary>
    public void Land()
    {
        //reset various parameters when the player is no longer in the air
        IsJumping = false;
        velocity.y = 0.0f;
    }
}

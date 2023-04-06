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
    private bool isFacingRight = true;
    // Calculated values based on the variables above
    float gravity;
    float jumpVelocity;
    [SerializeField] Vector3 velocity;
    float velocityXSmoothing;
    private InventoryManager inventoryManager;
    [SerializeField] private bool inventoryActive = false;

    //Player health
    [SerializeField] private int playerHealthPool = 3;
    [SerializeField] private DisplayPlayerHealth dph;
    [SerializeField] private bool playerInvincible = false;
    [SerializeField] private float invincibilityTime = 2.0f;

    public bool IsDamaged
    {
        get { return isDamaged; }
        set { isDamaged = value; }
    }

    public bool FacingRight
    {
        get { return isFacingRight; }
    }


    //velocity getter/setter
    public Vector3 Velocity
    {
        get { return velocity; }
        set { 
            velocity.x = value.x;
            velocity.y = value.y;
        }
    }

    public Vector3 GetPos
    {
        get { return transform.position; }
    }

    //controller getter
    public PlayerController Controller
    {
        get { return controller; }
    }

    // Reference to the controller script
    PlayerController controller;

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

        if(SpawnHandler.instance != null)
        {
            transform.position = SpawnHandler.instance.SpawnPoint;
        }

        if(inventoryManager == null && inventoryActive)
        {
            inventoryManager = GameObject.FindGameObjectWithTag("inventory").GetComponent<InventoryManager>();
        }

        if(dph == null)
        {
            Debug.LogWarning("Make sure to assign the UI Health to player!");
        }

        //gets the script
        slider = GetComponent<WallSlide>();
        controller = GetComponent<PlayerController>();

        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity * timeToJumpApex);
    }

    /// <summary>
    /// Handles player input
    /// </summary>
    void FixedUpdate()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && inventoryManager)
        {
            if(!inventoryActive)
            {
                inventoryManager.ShowInventory();
                inventoryActive = !inventoryActive;
            }
            else
            {
                inventoryManager.HideInventory();
                inventoryActive = !inventoryActive;
            }
            
        }


        // Gets the current player axis input
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        // Checks if the player can perform a jump
        if (controller.spacePressed && controller.collisions.below
            || (controller.spacePressed && slider.IsWallSliding && slider.ResetJump)
            || (controller.spacePressed && controller.grabbingLedge))
        {
            velocity.y = jumpVelocity;
            Debug.LogWarning("Jumping");
        }

        // Smooths out horizontal movement
        float targetVelocityX = input.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);

        // Updates the player's velocity

        //if the player is wall sliding
        if (slider.IsWallSliding)
        {
            //fall at fixed velocity
            velocity.y = (gravity * 4) * Time.deltaTime;
        }
        //otherwise if the player is not grabbing a ledge
        else if (!controller.grabbingLedge)
        {
            //fall normally
            velocity.y += gravity * Time.deltaTime;
        }

        // Resets the vertical velocity if the player touches a ceiling or floor or is grabbing a ledge
        if ((controller.collisions.above || controller.collisions.below || controller.grabbingLedge) && !controller.spacePressed)
        {
            velocity.y = 0;
            //Debug.LogWarning("Stopped");
        }

        controller.Move(velocity * Time.deltaTime);

        //we live in a society
        if (velocity.x < -0.05)
        {
            isFacingRight = false;
        }
        else if (velocity.x > 0.05)
        {
            isFacingRight = true;
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

        float directionKnockback = (attackDirectionVector > 0) ? 1 : -1;

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
            Debug.LogWarning("Damage player this way: " + damageVector);
            StartCoroutine(PlayerKnockback(damageVector, damage));
        }

        if(!playerInvincible)
        {
            playerHealthPool -= 1;
            if(dph)
            {
                dph.TakeUIDamage();
            }

            if(playerHealthPool <= 0)
            {
                SpawnHandler.instance.RespawnPlayer();
                if(dph)
                {
                    dph.UIRespawn();
                }
            }
            StartCoroutine(IFrames());
        }

        //Blah blah blah code etc.
    }

    protected virtual IEnumerator IFrames()
    {
        float currentTimer = 0;
        playerInvincible = true;
        while (currentTimer < invincibilityTime)
        {
            currentTimer += Time.deltaTime;
            yield return null;
        }
        playerInvincible = false;
    }
}
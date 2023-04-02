//Kyle Zielinski
//2/1/2023
//Base enemy class that sets up initial parameters for enemies in the game

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class Enemy : Entity
{
    //EnemyController controller;
    [SerializeField] protected LayerMask platformLayerMask;
    [SerializeField] protected LayerMask playerLayerMask;

    //Orientation values
    public bool facingRight;
    [SerializeField] public float walkSpeed;
    protected bool canMove;
    protected float cosValue = 0;
    protected float stepValue = 0.5f;


    protected BoxCollider2D collisionBox;
    protected float viewRange;
    protected Vector3 pos;

    //Attacks
    [SerializeField] protected float attackDuration; //How fast the attack comes out
    [SerializeField] protected float attackCooldown; //Attack cooldown
    [SerializeField] protected float attackCooldownTimer; //Tracks Attack Cooldown
    [SerializeField] protected bool attackCooldownFinished; //Tracks Attack Cooldown if finished [DEBUG]
    [SerializeField] protected bool isAttacking = false; //Handles mid attack checks
    [SerializeField] protected bool canAttack = false; //Checks for no cooldown on attack
    protected float attackDamage; 
    protected float attackStatusEffect;
    private bool isInvincible;
    [SerializeField] protected float invincibilityTime = 1.0f;
    [SerializeField] protected bool isStunned = false;
    [SerializeField] protected float stunTime = 4.0f;
    [SerializeField] protected float attackRange = 1.25f;

    //Status
    protected bool isAlive;

    public void Start()
    {
       
    }

    public void Update()
    {
        base.Update();
    }

    /// <summary>
    /// Attacks the Player
    /// </summary>
    protected virtual IEnumerator Attack()
    {
        yield return null;
    }

    protected virtual IEnumerator AttackCooldown()
    {
        while (attackCooldownTimer < attackCooldown)
        {
            attackCooldownTimer += Time.deltaTime;
            attackCooldownFinished = false;
            yield return null;
        }

        attackCooldownTimer = 0;
        attackCooldownFinished = true;
        canAttack = true;
    }

    protected virtual void Search()
    {
    }

    /// <summary>
    /// Moves the enemy until it hits a wall, if it can move
    /// </summary>
    protected void Walk()
    {
        if (canMove)
        {
            if (facingRight)
            {
                pos.x += walkSpeed * Time.deltaTime;
            }
            else
            {
                pos.x -= walkSpeed * Time.deltaTime;
            }
            if (HitWall())
            {
                TurnAround();
            }
        }
    }

    protected void Patrol()
    {
        pos = transform.position;
        float walkValue = walkSpeed * Mathf.Cos(cosValue) * Time.deltaTime;
        pos.x += walkValue;
        cosValue += stepValue * Time.deltaTime;
        facingRight = walkValue > 0;
    }

    /// <summary>
    /// Checks in front of it to see if it hits a wall or not
    /// </summary>
    /// <returns> Returns whether or not there was a platform object hit </returns>
    public bool HitWall()
    {
        RaycastHit2D hit;
        if (facingRight)
        {
            hit = Physics2D.Raycast(collisionBox.bounds.center, Vector2.right, collisionBox.bounds.extents.x + .01f, platformLayerMask);
        }
        else
        {
            hit = Physics2D.Raycast(collisionBox.bounds.center, Vector2.left, collisionBox.bounds.extents.x + .01f, platformLayerMask);
        }
        Color rayColor;
        if (hit.collider != null)
        {
            rayColor = Color.green;
        }
        else
        {
            rayColor = Color.blue;
        }
        if (facingRight)
        {
            Debug.DrawRay(collisionBox.bounds.center, Vector2.right * (collisionBox.bounds.extents.x + .01f), rayColor);
        }
        else
        {
            Debug.DrawRay(collisionBox.bounds.center, Vector2.left * (collisionBox.bounds.extents.x + .01f), rayColor);
        }
        return hit.collider != null;
    }

    /// <summary>
    /// Changes the direction the enemy is facing
    /// </summary>
    void TurnAround()
    {
        if (facingRight)
        {
            facingRight = false;
            return;
        }
        facingRight = true;
    }

    /// <summary>
    /// Casts a ray in the attack range box to detect the player
    /// </summary>
    /// <returns></returns>
    protected bool PlayerInRange()
    {
        RaycastHit2D hit;
        if (facingRight)
        {
            hit = Physics2D.Raycast(transform.position + new Vector3(.5f, 0, 0), Vector2.right, attackRange, playerLayerMask);
        }
        else
        {
            hit = Physics2D.Raycast(transform.position + new Vector3(-.5f, 0, 0), Vector2.left, attackRange, playerLayerMask);
        }
        Color rayColor;
        if (hit.collider != null)
        {
            rayColor = Color.green;
        }
        else
        {
            rayColor = Color.red;
        }
        if (facingRight)
        {
            Debug.DrawRay(transform.position + new Vector3(.5f, 0, 0), Vector2.right * (attackRange + .01f), rayColor);
        }
        else
        {
            Debug.DrawRay(transform.position + new Vector3(-.5f, 0, 0), Vector2.left * (attackRange + .01f), rayColor);
        }
        return hit.collider != null;
    }

    private void TakeDamage(int damage)
    {
        if(!isInvincible)
        {
            healthPool -= damage;
            StartCoroutine(IFrames());
        } 
    }

    protected virtual IEnumerator IFrames()
    {
        float currentTimer = 0;
        isInvincible = true;
        while (currentTimer < invincibilityTime)
        {
            currentTimer += Time.deltaTime;
            yield return null;
        }
        isInvincible = false;
    }

    protected virtual IEnumerator Stunned()
    {
        float currentTimer = 0;
        isStunned = true;
        while (currentTimer < stunTime)
        {
            currentTimer += Time.deltaTime;
            yield return null;
        }
        isStunned = false;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.LogWarning(collision.tag);
        switch(collision.tag)
        {
            case "ground":
                velocity = Vector3.zero;
                acceleration = Vector3.zero;
                isAerial = false;
                break;
            case "weapon_0":
                TakeDamage(1);
                break;
            case "weapon_0_stun":

                Debug.LogWarning("Enemy:" + gameObject.name + " Stunned!");

                TakeDamage(1);
                StartCoroutine(Stunned());
                break;
            case "weapon_1":
                //stub for Solider
                break;
            case "weapon_2":
                //stub for Mutant
                break;
            case "Player":
                   Player.instance.PlayerTakeDamage(transform.position.x, 0);
                break;
            default:
                Debug.LogError("Handled this Collision! Collision with: " + collision.name);
                break;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "ground")
        {
            isAerial = true;
            acceleration.y = gravity;
        }
    }

    public void SetCharacterOrientation(bool facingRight)
    {
        cosValue = facingRight ? 0 : Mathf.PI / 2;
    }
}

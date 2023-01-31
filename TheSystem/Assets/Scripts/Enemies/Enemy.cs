using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    [SerializeField] private LayerMask platformLayerMask;
    [SerializeField] private LayerMask playerLayerMask;

    public bool facingRight;
    [SerializeField] public float walkSpeed;
    protected bool canMove;
    protected BoxCollider2D collisionBox;
    protected BoxCollider2D attackRange;
    protected Vector2 pos;

    //Attacks
    [SerializeField] protected float attackDuration; //How fast the attack comes out
    [SerializeField] protected float attackCooldown; //Attack cooldown
    [SerializeField] protected float attackCooldownTimer; //Tracks Attack Cooldown
    [SerializeField] protected bool attackCooldownFinished; //Tracks Attack Cooldown if finished [DEBUG]
    [SerializeField] protected bool isAttacking = false; //Handles mid attack checks
    [SerializeField] protected bool canAttack = false; //Checks for no cooldown on attack
    protected float attackDamage; 
    protected float attackStatusEffect; 

    /// <summary>
    /// Attacks the Player
    /// </summary>
    protected virtual IEnumerator Attack()
    {
        yield return null;
    }

    protected virtual IEnumerator AttackCooldown()
    {
        yield return null;
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
            hit = Physics2D.Raycast(attackRange.bounds.center + new Vector3(.5f, 0, 0), Vector2.right, attackRange.bounds.extents.x - .5f, playerLayerMask);
        }
        else
        {
            hit = Physics2D.Raycast(attackRange.bounds.center + new Vector3(-.5f, 0, 0), Vector2.left, attackRange.bounds.extents.x - .5f, playerLayerMask);
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
            Debug.DrawRay(attackRange.bounds.center + new Vector3(.5f, 0, 0), Vector2.right * (attackRange.bounds.extents.x - .5f + .01f), rayColor);
        }
        else
        {
            Debug.DrawRay(attackRange.bounds.center + new Vector3(-.5f, 0, 0), Vector2.left * (attackRange.bounds.extents.x - .5f + .01f), rayColor);
        }
        return hit.collider != null;
    }
}

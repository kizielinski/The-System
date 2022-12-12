using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walker : MonoBehaviour
{
    [SerializeField] private LayerMask platformLayerMask;
    [SerializeField] private LayerMask playerLayerMask;

    public bool facingRight;
    public float walkSpeed;

    private bool canMove;

    private BoxCollider2D collisionBox;
    private CapsuleCollider2D attackRange;

    private Vector2 pos;

    void Start()
    {
        canMove = true;

        collisionBox = GetComponent<BoxCollider2D>();
        attackRange = GetComponent<CapsuleCollider2D>();

        pos = transform.position;
    }

    private void FixedUpdate()
    {
        Walk();
        if(PlayerInRange())
        {
            Attack();
        }
        transform.position = pos;
    }

    /// <summary>
    /// Moves the enemy until it hits a wall, if it can move
    /// </summary>
    void Walk()
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
        if(facingRight)
        {
            facingRight = false;
            return;
        }
        facingRight = true;
    }

    /// <summary>
    /// Attacks the Player
    /// </summary>
    void Attack()
    {
        canMove = false;
    }

    /// <summary>
    /// Casts in front of it to see if the player is in attack range
    /// </summary>
    /// <returns></returns>
    bool PlayerInRange()
    {
        RaycastHit2D hit;
        if (facingRight)
        {
            hit = Physics2D.Raycast(attackRange.bounds.center, Vector2.right, attackRange.bounds.extents.x, playerLayerMask);
        }
        else
        {
            hit = Physics2D.Raycast(attackRange.bounds.center, Vector2.left, attackRange.bounds.extents.x, playerLayerMask);
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
            Debug.DrawRay(attackRange.bounds.center, Vector2.right * (attackRange.bounds.extents.x + .01f), rayColor);
        }
        else
        {
            Debug.DrawRay(attackRange.bounds.center, Vector2.left * (attackRange.bounds.extents.x + .01f), rayColor);
        }
        return hit.collider != null;
    }
}

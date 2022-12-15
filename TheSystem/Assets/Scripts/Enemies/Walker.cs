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
    private BoxCollider2D attackRange;

    private Vector2 pos;

    void Start()
    {
        canMove = true;

        BoxCollider2D[] boxes = GetComponents<BoxCollider2D>();

        collisionBox = boxes[0];
        attackRange = boxes[1];

        pos = transform.position;
    }

    private void FixedUpdate()
    {
        Walk();
        if(PlayerInRange())
        {
            Attack();
        }
        else
        {
            canMove = true;
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
    /// Casts a ray in the attack range box to detect the player
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
            Debug.DrawRay(attackRange.bounds.center + new Vector3(.5f, 0, 0), Vector2.right * (attackRange.bounds.extents.x - .5f + .01f), rayColor);
        }
        else
        {
            Debug.DrawRay(attackRange.bounds.center + new Vector3(-.5f, 0, 0), Vector2.left * (attackRange.bounds.extents.x - .5f + .01f), rayColor);
        }
        return hit.collider != null;
    }
}

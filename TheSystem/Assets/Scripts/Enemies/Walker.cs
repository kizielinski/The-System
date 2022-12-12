using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walker : MonoBehaviour
{
    [SerializeField] private LayerMask platformLayerMask;
    [SerializeField] private LayerMask playerLayerMask;

    public bool facingRight;
    public float walkSpeed;

    private BoxCollider2D collisionBox;

    private Vector2 pos;

    void Start()
    {
        collisionBox = GetComponent<BoxCollider2D>();

        pos = transform.position;
    }

    private void FixedUpdate()
    {
        Walk();
        transform.position = pos;
    }

    void Walk()
    {
        if(facingRight)
        {
            pos.x += walkSpeed * Time.deltaTime;
        }
        else
        {
            pos.x -= walkSpeed * Time.deltaTime;
        }
        if(HitWall())
        {
            TurnAround();
        }
    }

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

    void TurnAround()
    {
        if(facingRight)
        {
            facingRight = false;
            return;
        }
        facingRight = true;
    }
}

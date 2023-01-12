using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class RaycastController : MonoBehaviour
{
    // Getting the proper layer that the player is supposed to collide with
    public LayerMask collisionMask;

    // Raycasting values
    public const float skinWidth = .015f; // How far into the object the rays start
    public int horizontalRayCount = 4;
    public int verticalRayCount = 4;

    [HideInInspector]
    public float horizontalRaySpacing;
    [HideInInspector]
    public float verticalRaySpacing;

    // Player collision info
    public BoxCollider2D collider;

    // Raycast struct definition
    public RaycastOrigins raycastOrigins;

    /// <summary>
    /// Get's the player's collision box and calculates the ray spacing for the raycasting
    /// </summary>
    public virtual void Start()
    {
        collider = GetComponent<BoxCollider2D>();
        CalculateRaySpacing();
    }

    /// <summary>
    /// Updates the four corners of the box where the raycasts will begin drawing from
    /// </summary>
    public void UpdateRaycastOrigins()
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * -2);

        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    /// <summary>
    /// Calculates how much space should be in between each ray
    /// </summary>
    public void CalculateRaySpacing()
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * -2);

        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, 6);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, 6);

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.y / (verticalRayCount - 1);
    }

    /// <summary>
    /// Keeps track of all four corners of the raycasting box
    /// </summary>
    public struct RaycastOrigins
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }
}

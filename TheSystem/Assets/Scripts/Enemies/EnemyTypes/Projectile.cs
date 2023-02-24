using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Entity
{
    Vector3 pos;
    // Start is called before the first frame update
    void Start()
    {
        pos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        velocity.y += gravity;
        velocity += VelocityToTarget();
    }

    public Vector3 VelocityToTarget(Vector3 target, Vector3 force)
    {
        force.x *= Utilities.DirectionToTarget(target).x > 0 ? 1 : -1;
        return Vector3.zero;
    }
}

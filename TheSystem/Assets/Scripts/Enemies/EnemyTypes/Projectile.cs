using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Entity
{
    Vector3 pos;
    Vector3 target;
    Vector3 force;
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        pos = transform.position;
        interactable = true;
        Debug.LogWarning("Hello Projectile!");
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
        velocity += acceleration * Time.deltaTime;
        pos += velocity * Time.deltaTime;
        transform.position = pos;
        PrepareNextFrame();
    }

    public void PrepareNextFrame()
    {
        pos = transform.position;
        gravity = SetGravity();
    }

    private float SetGravity()
    {
        float new_gravity = -9.81f;

        if(interactable == false)
        {
            new_gravity = 0.0f;
            velocity = Vector3.zero;
        }

        return new_gravity;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.LogWarning(collision.name);
        if(collision.tag == "ground")
        {
            interactable = false;
        }
    }

    public void SetAcceleration(Vector3 _accel)
    {
        acceleration = _accel;
    }
}

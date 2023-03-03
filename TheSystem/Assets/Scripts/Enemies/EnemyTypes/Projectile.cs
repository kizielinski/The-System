using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Entity
{
    private Vector3 pos;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        pos = transform.position;
        interactable = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (interactable)
        {
            base.Update();
            velocity += acceleration * Time.deltaTime;
            pos += (velocity * Time.deltaTime) + (0.5f * acceleration * Mathf.Pow(Time.deltaTime, 2));
            transform.position = pos;
            PrepareNextFrame();
        }
    }

    public void PrepareNextFrame()
    {
        pos = transform.position;
        gravity = SetGravity();
    }

    private float SetGravity()
    {
        float new_gravity = -9.81f;

        return new_gravity;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "ground" || collision.tag == "Player")
        {
            interactable = false;

            Explode();
        }
    }

    public void SetAcceleration(Vector3 _accel)
    {
        acceleration = _accel;
    }

    private void Explode()
    {

    }
}

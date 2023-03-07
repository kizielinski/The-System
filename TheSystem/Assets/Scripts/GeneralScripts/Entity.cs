//Kyle Zielinski
//2/1/2023
//This class defines what entites are in our game and how they
//interact with the game rules and being a Unity GameObject.


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    //Every entity can be used for the following
    [SerializeField] protected int healthPool = 3;
    protected bool interactable = false;
    protected bool canTakeDamage = false;
    protected bool isMechanicTrigger = false;
    protected bool isHazard = false;
    [SerializeField] protected bool isAerial = false;

    protected float gravity = -9.81f;
    protected float mass = 1;
    protected Vector3 acceleration;
    protected Vector3 velocity;
    protected Vector3 drag;
    protected Vector3 friction;

    protected bool isDamaged = false;


    // Start is called before the first frame update
    protected void Start()
    {
        interactable = true;

        drag = Vector3.one;
        drag.z = 0;

        friction = Vector3.one;
        friction.z = 0;
    }

    // Update is called once per frame
    protected void Update()
    {
        ApplyInheritForces();

        if(healthPool <=0)
        {
            Destroy(gameObject);
        }
    }

    private void ApplyInheritForces()
    {
        float facingRight = velocity.x > 0 ? -1 : 1;

        //Drag and friction
        if((acceleration.x < -0.001f || acceleration.x > 0.001f) && isAerial)
        {
            acceleration += drag * facingRight * Time.deltaTime;
        }
        else if(acceleration.x < -0.001f || acceleration.x > 0.001f)
        {
            acceleration += friction * facingRight * Time.deltaTime;
        }

        if ((velocity.x < -0.001f || velocity.x > 0.001f) && isAerial)
        {
            velocity += drag * facingRight * Time.deltaTime;
        }
        else if (velocity.x < -0.001f || velocity.x > 0.001f)
        {
            velocity += friction * facingRight * Time.deltaTime;
        }
        else
        {
            velocity.x = 0;
        }

        if(acceleration.y > -9.81f)
        {
            acceleration.y += (gravity - 1.0f) * Time.deltaTime;
        }
        else if (acceleration.y < -10.0f)
        {
            acceleration.y += 1.0f * Time.deltaTime;
        }
        else
        {
            acceleration.y = -9.81f;
        }
    }

    //velocity getter
    public Vector3 Velocity
    {
        get { return velocity; }
    }

    public bool IsDamaged
    {
        get { return isDamaged; }
        set { isDamaged = value; }
    }
}

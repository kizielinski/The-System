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
    protected int healthPool = 0;
    protected bool interactable = false;
    protected bool canTakeDamage = false;
    protected bool isMechanicTrigger = false;

    protected float gravity;
    protected float mass = 1;
    protected Vector3 acceleration;
    protected Vector3 velocity;

    protected bool isDamaged = false;


    // Start is called before the first frame update
    void Start()
    {
        interactable = true;

        Debug.Log(interactable);
    }

    // Update is called once per frame
    void Update()
    {
        
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

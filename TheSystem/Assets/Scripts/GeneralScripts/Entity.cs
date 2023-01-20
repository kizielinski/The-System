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
}

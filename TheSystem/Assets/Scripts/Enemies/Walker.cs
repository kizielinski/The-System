using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walker : MonoBehaviour
{
    public bool facingRight;
    public float walkSpeed;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private bool IsWall()
    {
        return facingRight;
    }
}

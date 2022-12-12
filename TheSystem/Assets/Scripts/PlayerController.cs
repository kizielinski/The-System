using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Reference to the Input System
    private PlayerControls playerControls;

    // Movement values
    Vector2 move; // Current move value
    Vector2 prevMove; // Previous move value

    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    void Start()
    {
        
    }

    private void Update()
    {
        move = playerControls.BasicMovement.Move.ReadValue<Vector2>();
        prevMove = move;
    }
}

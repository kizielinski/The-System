using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : Enemy
{
    private BoxCollider2D damageBox;
    private float zRot;
    [SerializeField] private bool isStill;
    private float laserRotationCosValue = 0f;
    [SerializeField] private float laserRotationStepValue;
    private Transform laserContainer;

    // Start is called before the first frame update
    void Start()
    {
        //Default value for Enemy
        canTakeDamage = false;

        canMove = true;

        healthPool = 1;

        isHazard = true;

        BoxCollider2D[] boxes = GetComponentsInChildren<BoxCollider2D>();
        damageBox = boxes[0];

        pos = transform.position;
        attackDamage = 0;

        laserRotationStepValue = 0.4f; //How far laser moves
        walkSpeed = 0.0f; //How fast laser moves
        laserContainer = gameObject.transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isStill)
        {
            RotateLaser();
        }
    }

    private void RotateLaser()
    {
        zRot = walkSpeed * Mathf.Cos(laserRotationCosValue) * Time.deltaTime;
        laserContainer.Rotate(0, 0, zRot);
        laserRotationCosValue += laserRotationStepValue * Time.deltaTime;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : Enemy
{
    private BoxCollider2D damageBox;
    private float zRot;
    private bool isStill;
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

        walkSpeed = 5.0f; //Resuse normal walk speed for laser rotation
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

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.LogWarning(collision.name);
        if (collision.name == "Player")
        {
            int x = 0;
            Player p = collision.GetComponent<Player>();
            p.PlayerTakeDamage(transform.position.x, 0);
        }
    }

    private void RotateLaser()
    {
        zRot = walkSpeed * Mathf.Cos(laserRotationCosValue) * Time.deltaTime;
        laserContainer.Rotate(0, 0, zRot);
        laserRotationCosValue += laserRotationStepValue * Time.deltaTime;
    }
}

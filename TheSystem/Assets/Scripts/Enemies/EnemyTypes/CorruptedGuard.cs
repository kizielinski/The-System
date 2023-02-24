using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorruptedGuard : Enemy
{
    private FieldOfView fov;
    [SerializeField] private GameObject shield;

    // Start is called before the first frame update
    void Start()
    {
        //Default value for Enemy
        canTakeDamage = true;
        canMove = true;
        walkSpeed =5;
        healthPool = 10;
        attackDuration = 0.5f;
        attackCooldown = 1.0f;

        BoxCollider2D[] boxes = GetComponentsInChildren<BoxCollider2D>();

        collisionBox = boxes[0];
        //damageBox = boxes[2];

        pos = transform.position;

        canAttack = false;
        attackCooldownFinished = true;

        isAlive = true;
        
        if(shield == null)
        {
            shield = gameObject.GetComponentInChildren<BoxCollider2D>().transform.gameObject; //Rough line to catch null shield values
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(fov == null)
        {
            fov = GetComponent<FieldOfView>();
        }
        fov.SetAimDirection(facingRight == true ? Vector3.right : Vector3.left);
        fov.SetOrigin(transform.position);
        if(isAlive)
        {
            if (fov.playerHit)
            {
                Vector3 pPos = Player.instance.transform.position;
                if (Vector3.Distance(transform.position, pPos) < 3)
                {
                    //Shield Bash
                }
                else
                {
                    //ThrowProjectile();
                }
            }
            else
            {
                Patrol();
            }

            transform.position = new Vector3(pos.x, pos.y, 0);
        }

        
    }


}

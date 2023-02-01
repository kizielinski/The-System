using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walker : Enemy
{
    private BoxCollider2D damageBox;

    void Start()
    {
        //Default value for Enemy
        canTakeDamage = true;
        canMove = true;
        walkSpeed = 1;
        healthPool = 10;
        attackDuration = 0.5f;
        attackCooldown = 1.0f;

        BoxCollider2D[] boxes = GetComponentsInChildren<BoxCollider2D>();

        collisionBox = boxes[0];
        attackRange = boxes[1];
        damageBox = boxes[2];

        pos = transform.position;

        canAttack = false;
        attackCooldownFinished = true;
    }

    private void FixedUpdate()
    {
        if(canAttack) //If cooldown done, able to attack
        {
            if(PlayerInRange()) //Check range
            {
                isAttacking = true;
                StartCoroutine(Attack());
            }
        }
        else if(attackCooldownFinished) //Start a new cooldown
        {
            StartCoroutine(AttackCooldown());
        }

        //If not attacking walk.
        if (!isAttacking && !PlayerInRange())
        {
            Walk();
        }

        transform.position = pos;
    }

    protected override IEnumerator Attack()
    {
        canAttack = false;
        canMove = false;
        isAttacking = true;

        this.GetComponent<SpriteRenderer>().color = new Color(0, 0, 255);

        //Get transform and rotation for hitbox.
        Transform damageBoxTransform = damageBox.transform;
        Vector3 attackStartPosition = damageBoxTransform.position;


        //Orient attack direction
        float attackdirection = 4.0f;
        if(facingRight)
        { attackdirection *= 1.0f; }
        else
        { attackdirection *= -1.0f; }

        //Setup values for lerp calculation
        Vector3 attackEndPos = new Vector3(attackdirection + attackStartPosition.x, attackStartPosition.y, 0);

        //Setup Lerp values
        Vector3 attackPosition = new Vector3();
        float currentTime = 0;

        while(currentTime < attackDuration)
        {
            attackPosition.x = Mathf.Lerp(attackStartPosition.x, attackEndPos.x, currentTime);
            attackPosition.y = Mathf.Lerp(attackStartPosition.y, attackEndPos.y, currentTime);

            damageBox.transform.position = attackPosition;

            currentTime += Time.deltaTime;
            yield return null;
        }

        //End the attack
        isAttacking = false;
        canMove = true;

        //Set end position
        damageBox.transform.localPosition = new Vector3(0, 0.25f, 0);
        this.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0);
    } 
}

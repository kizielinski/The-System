using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walker : Enemy
{
    BoxCollider2D damageBox;

    void Start()
    {
        //Default value for Enemy
        canTakeDamage = true;
        canMove = true;
        walkSpeed = 1;
        healthPool = 10;
        attackDuration = 1.0f;

        BoxCollider2D[] boxes = GetComponentsInChildren<BoxCollider2D>();

        collisionBox = boxes[0];
        attackRange = boxes[1];
        damageBox = boxes[2];

        pos = transform.position;
    }

    private void FixedUpdate()
    {
        Walk();
        if(PlayerInRange())
        {
            if(!isAttacking)
            {
                StartCoroutine(Attack());
            }
            
        }
        transform.position = pos;
    }

    protected override IEnumerator Attack()
    {
        canMove = false;
        isAttacking = true;

        this.GetComponent<SpriteRenderer>().color = new Color(0, 0, 255);

        //Get transform and rotation for hitbox.
        Transform damageBoxTransform = damageBox.transform;
        Vector3 attackStartPosition = damageBoxTransform.position;
        Quaternion attackRotation = damageBoxTransform.rotation;

        //Orient attack direction
        float attackdirection = 0.0f;
        if(facingRight)
        { attackdirection = 1.0f; }
        else
        { attackdirection = -1.0f; }

        //Setup values for lerp calculation
        Vector3 attackEndPos = new Vector3(attackdirection + attackStartPosition.x, attackStartPosition.y-1.0f, 0);

        //Setup Lerp values
        Vector3 tempRotation = new Vector3();
        Quaternion tempRotationFinal = new Quaternion();
        Vector3 attackPosition = new Vector3();
        float attackEndRotation = attackdirection * -90f;
        float currentTime = 0;

        while(currentTime < attackDuration)
        {
            attackPosition.x = Mathf.Lerp(attackStartPosition.x, attackEndPos.x, currentTime);
            attackPosition.y = Mathf.Lerp(attackStartPosition.y, attackEndPos.y, currentTime);

            damageBox.transform.position = attackPosition;

            
            tempRotation.z = Mathf.Lerp(attackRotation.eulerAngles.z, attackEndRotation, currentTime);
            tempRotationFinal.eulerAngles = tempRotation;
            damageBox.transform.rotation = tempRotationFinal;

            currentTime += Time.deltaTime;
            yield return null;
        }

        //End the attack
            //Set end position
            damageBox.transform.localPosition = new Vector3(0, 1, 0);

            //Set end rotation
            tempRotation.z = attackEndRotation;
            damageBoxTransform.transform.rotation = new Quaternion(0, 0, 0, 0);

        isAttacking = false;
        canMove = true;
    }    
}

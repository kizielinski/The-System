using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapons : MonoBehaviour
{
    [SerializeField] private GameObject weaponParent;
    private Collider2D weaponHitBox;
    private float attackDuration = 1.0f;

    private float attackCooldown;
    private float attackCooldownTimer;
    private bool attackCooldownFinished;

    private static bool facingRight = true;
    [SerializeField] private bool canAttack = true;

    // Start is called before the first frame update
    void Start()
    {
        attackCooldown = 3.0f;
        attackCooldownTimer = 0;

        weaponHitBox = weaponParent.GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {

        if (canAttack)
        {
            if (Input.GetKey(KeyCode.Alpha1))
            {
                Debug.LogWarning("Swing One");
                StartCoroutine(Attack());
            }
        }
        //if (Input.GetKey(KeyCode.Alpha2))
        //{

        //}
        //if (Input.GetKey(KeyCode.Alpha3))
        //{

        //}
    }

    private IEnumerator Attack()
    {

        Vector2 newHitboxSize;

        canAttack = false;
        //canMove = false;
        //isAttacking = true;
        bool secondAttack = false;
        bool thirdAttack = false;
        Transform weaponTransform = weaponParent.transform;
        BoxCollider2D weaponHitBox = weaponTransform.gameObject.GetComponent<BoxCollider2D>();

        weaponParent.GetComponent<SpriteRenderer>().color = new Color(0, 0, 255);

        //Get transform and rotation for hitbox.
        Vector3 attackStartPosition = weaponTransform.position;
        Quaternion attackRotation = transform.localRotation;

        //Orient attack direction
        float attackdirection = 0.0f;
        if (facingRight)
        { attackdirection = 1.0f; }
        else
        { attackdirection = -1.0f; }

        //Setup values for lerp calculation

        //Setup Attack One - Overhead Attack
        Vector3 tempRotation = new Vector3();
        Quaternion tempRotationFinal = new Quaternion();
        float attackEndRotation = attackdirection * -90f;
        float currentTime = 0;
        Utilities.ScaleHitBox(weaponHitBox, new Vector2(1, 5));
        weaponTransform.localPosition += new Vector3(0, 2.5f, 0);

        //Attack One - Overhead Attack

        {

        }

        //while (currentTime < attackDuration)
        //{
        //    if (currentTime > ((2 * attackDuration) / 3) && Input.GetKey(KeyCode.Alpha1))
        //    {
        //        secondAttack = true;
        //    }

        //    tempRotation.z = Mathf.Lerp(attackRotation.eulerAngles.z, attackEndRotation, currentTime);
        //    tempRotationFinal.eulerAngles = tempRotation;
        //    transform.localRotation = tempRotationFinal;

        //    currentTime += Time.deltaTime;
        //    yield return null;
        //}

        Utilities.DefaultHitBox(weaponHitBox);

        //currentTime = 0;
        if(secondAttack)
        {
            //Setup Attack Two
            Vector2 attackPosition = new Vector2();
            Vector3 attackEndPos = new Vector3(1 + attackStartPosition.x, 0, 0);
            Utilities.ScaleHitBox(weaponHitBox, new Vector2(5, 1));

            while (currentTime < attackDuration)
            {
                attackPosition.x = Mathf.Lerp(0, 4, currentTime * 6);
                weaponHitBox.offset = attackPosition;

                currentTime += Time.deltaTime;
                yield return null;
            }

            currentTime = 0;
        }
        else
        {
            StartCoroutine(AttackCooldown());
        }

        if(thirdAttack)
        {
            while (secondAttack && currentTime < attackDuration)
            {
                
                yield return null;
            }
        }
        else
        {
            StartCoroutine(AttackCooldown());
        }

        //End the attack
        {
            //Set end position
            weaponHitBox.transform.localPosition = new Vector3(0, 6, 0);

            //Set end rotation
            transform.localRotation = new Quaternion(0, 0, 0, 0);

            ////isAttacking = false;
            ////canMove = true;
            StartCoroutine(AttackCooldown());
        }
    }

    protected virtual IEnumerator AttackCooldown()
    {
        StopCoroutine(Attack());

        while (attackCooldownTimer < attackCooldown)
        {
            attackCooldownTimer += Time.deltaTime;
            attackCooldownFinished = false;
            yield return null;
        }

        attackCooldownTimer = 0;
        attackCooldownFinished = true;
        canAttack = true;
    }
}

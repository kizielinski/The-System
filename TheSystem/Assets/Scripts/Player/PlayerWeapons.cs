using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UIElements;

public class PlayerWeapons : MonoBehaviour
{
    [SerializeField] private GameObject weaponParent;
    private Collider2D weaponHitBox;

    //Attack speed for each individual Attack
    [Tooltip ("Adjust values accordingly, each attack will happen at the speed set by their respective value")]
    [SerializeField] private float attackDuration_1 = 1.0f;
    [SerializeField] private float attackDuration_2 = 1.0f;
    [SerializeField] private float attackDuration_3 = 1.0f;

    [Tooltip("Attack Speed Multiplier. Starts at 1.0. Do not set below 0")]
    [SerializeField] private float attackSpeedMultiplier = 5.0f; //Default one


    [SerializeField] private float attackCooldown = 3.0f;
    [SerializeField] private float attackCooldownTimer = 0;
    private bool attackCooldownFinished;

    [SerializeField] private bool canAttack = true;
    private bool isAttacking = false;

    // Start is called before the first frame update
    void Start()
    {
        weaponHitBox = weaponParent.GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canAttack)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                //Debug.LogWarning("Swing One");
                canAttack = false;
                StopCoroutine(AttackCooldown());
                StartCoroutine(Attack());
            }
        }
    }

    //Handles Scrapper attacks, needs to be refined visually but works really well.
    private IEnumerator Attack()
    {
        if(attackSpeedMultiplier < 0)
        {
            attackSpeedMultiplier = 0.0001f;
        }

        if(isAttacking)
        {
            StopCoroutine(Attack());
        }
        canAttack = false;
        isAttacking = true;
        bool firstAttack = true;
        bool secondAttack = false;
        bool thirdAttack = false;
        bool attackOver = false;
        Transform weaponTransform = weaponParent.transform;
        BoxCollider2D weaponHitBox = weaponTransform.gameObject.GetComponent<BoxCollider2D>();

        weaponParent.GetComponent<SpriteRenderer>().color = new Color(0, 0, 255);

        //Get transform and rotation for hitbox.
        Vector3 attackStartPosition = weaponTransform.position;
        Quaternion attackRotation = transform.localRotation;

        //Orient attack direction
        float attackdirection = 0.0f;
        if (Player.instance.FacingRight)
        { attackdirection = 1.0f; }
        else
        { attackdirection = -1.0f; }

        //Setup values for lerp calculation

        //Setup Attack One - Overhead Attack
        Vector3 tempRotation = new Vector3();
        Quaternion tempRotationFinal = new Quaternion();
        float attackEndRotation = attackdirection * -90f;
        float currentTime = 0;
        Utilities.ScaleHitBox(weaponHitBox, new Vector2(1, 4));
        weaponTransform.localPosition += new Vector3(0, 2.5f, 0);

        //Attack One - Overhead Attack

        if(firstAttack)
        {
            firstAttack = false;
            while (currentTime < attackDuration_1)
            {
                if (currentTime > ((2 * attackDuration_1) / 3) && Input.GetKey(KeyCode.Mouse0))
                {
                    secondAttack = true;
                }

                tempRotation.z = Mathf.Lerp(attackRotation.eulerAngles.z, attackEndRotation, currentTime * attackSpeedMultiplier);
                tempRotationFinal.eulerAngles = tempRotation;
                transform.localRotation = tempRotationFinal;

                currentTime += Time.deltaTime;
                yield return null;
            }

            Debug.LogWarning("First Attack Complete!");

            Utilities.DefaultHitBox(weaponHitBox);
            weaponTransform.localPosition = Vector3.zero;
            weaponHitBox.offset = Vector2.zero;
            currentTime = 0;
        }

        if (secondAttack)
        {
            secondAttack = false;
            //Setup Attack Two
            Vector3 attackPosition = new Vector3();
            float attackEndPos = attackdirection * 2;
            Utilities.ScaleHitBox(weaponHitBox, new Vector2(1, 5));
            while (currentTime < attackDuration_2)
            {
                if (currentTime > (attackDuration_2 / 3) && Input.GetKey(KeyCode.Mouse0))
                {
                    thirdAttack = true;
                }

                attackPosition.x = Mathf.Lerp(0, attackEndPos, currentTime * attackSpeedMultiplier);
                weaponTransform.position = weaponTransform.position + (attackPosition * Time.deltaTime);

                currentTime += Time.deltaTime;
                yield return null;
            }
            Debug.LogWarning("Second Attack");
        }
        else
        {
            attackOver = true;
        }

        if (thirdAttack && !attackOver)
        {
            //Setup Attack Three - Overhead Attack
            tempRotation = new Vector3();
            tempRotationFinal = new Quaternion();
            attackEndRotation = attackdirection * 90f;
            weaponHitBox.offset = Vector2.zero;
            currentTime = 0;
            Utilities.ScaleHitBox(weaponHitBox, new Vector2(1, 5));
            weaponTransform.localPosition = new Vector3(0, -2.5f, 0);
            weaponHitBox.tag = "weapon_0_stun";

            while (currentTime < attackDuration_3)
            {
                tempRotation.z = Mathf.Lerp(attackRotation.eulerAngles.z, attackEndRotation, currentTime * attackSpeedMultiplier);
                tempRotationFinal.eulerAngles = tempRotation;
                transform.localRotation = tempRotationFinal;

                currentTime += Time.deltaTime;
                yield return null;
            }
            Debug.LogWarning("Third Attack");
        }
        else
        {
            attackOver = true;
        }
        weaponHitBox.tag = "weapon_0";

        //End the attack
        {
            //Set end position and roation
            weaponTransform.localPosition = Vector3.zero;
            weaponHitBox.offset = Vector2.zero;
            transform.localRotation = new Quaternion(0, 0, 0, 0);
            Utilities.DefaultHitBox(weaponHitBox);
            currentTime = 0;

            //instancesOfAttack -= 1;
            StartCoroutine(AttackCooldown());
        }
    }

    /// <summary>
    /// Overrides parent method for AttackCooldown
    /// Handles player attacks and ensures Attack()
    /// is no longer running on another thread. Otherwise
    /// there would be player action collisions which is undesirable.
    /// </summary>
    /// <returns></returns>
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.GraphicsBuffer;

public class CorruptedGuard : Enemy
{
    private FieldOfView fov;
    [SerializeField] private GameObject shield;
    private static Object prefab;
    private bool isThrowing;
    [SerializeField] private bool isDashing;
    [SerializeField] private Vector3 dashPos;
    private int throwAttackCooldown;

    // Start is called before the first frame update
    void Start()
    {
        //Default value for Enemy
        canTakeDamage = true;
        canMove = true;
        walkSpeed = 2;
        healthPool = 10;
        attackDuration = 2.5f;
        attackCooldown = 4.0f;
        throwAttackCooldown = (int)attackCooldown/2;

        BoxCollider2D[] boxes = GetComponentsInChildren<BoxCollider2D>();

        collisionBox = boxes[0];
        //damageBox = boxes[2];

        pos = transform.position;

        canAttack = true;
        attackCooldownFinished = true;

        isAlive = true;
        
        if(shield == null)
        {
            shield = gameObject.GetComponentInChildren<BoxCollider2D>().transform.gameObject; //Rough line to catch null shield values
        }

        prefab = Resources.Load("Prefabs/Projectile");

        isThrowing = false;
        isAerial = true;
        isDashing = false;
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
        if (isAlive || !isStunned)
        {
            if (fov.playerHit && canAttack)
            {
                StartCoroutine(Attack());
            }
            else if(attackCooldownFinished)
            {
                Patrol();
                fov.playerHit = false;
            }

            base.Update();
            if(isAerial)
            {
                velocity += acceleration * Time.deltaTime;
            }
            else
            {
                velocity.x += acceleration.x * Time.deltaTime;
            }
            pos += velocity * Time.deltaTime;
            transform.position = pos;
        }
    }

    private void ThrowProjectile()
    {
        canAttack = false;
        GameObject projBomb = Instantiate(prefab, transform.position, transform.rotation) as GameObject;
        Projectile p = projBomb.GetComponent<Projectile>();
        p.SetAcceleration(new Vector3(((facingRight == true ? 1 : -1) * 8), 9, 0)); //This is gross needs to be fixed
        attackCooldown = throwAttackCooldown;

        StartCoroutine(AttackCooldown());
    }

    private IEnumerator ShieldBash()
    {
        StopCoroutine("Attack");

        canAttack = false;
        fov.playerHit = false;

        Debug.LogError("Dash");
        float currentTime = 0;

        if (!isDashing)
        {
            //Set dash vector
            if (facingRight)
            {
                dashPos = transform.position + (Vector3.right * 30);
            }
            else
            {
                dashPos = transform.position + (Vector3.left * 30);
            }
            isDashing = true;
        }

        while (Vector3.Distance(transform.position, dashPos) > 0.5f)
        {
            velocity.x += (facingRight ? 1.0f : -1.0f) * 32.0f * Time.deltaTime;
            
            currentTime += Time.deltaTime;
            yield return null;
        }

        velocity.x = 0;
        SetCharacterOrientation(!facingRight);
        isDashing = false;
        fov.PlayerHitIsNow = false;
        attackCooldown = throwAttackCooldown * 2;

        StartCoroutine(AttackCooldown());

        ////If dash has reached its destination
        //Debug.LogError("Dash Distance: " + (Vector3.Distance(pos, dashPos)));
        //Debug.LogError("Is Dashing " + isDashing);
        //Debug.LogError("Speed/Dir: " + walkSpeed);
        //Debug.LogError("Cos Value: " + cosValue);
        //Debug.LogError("Walk Direction: " + Mathf.Cos(cosValue));
    }

    protected override IEnumerator Attack()
    {
        Debug.LogError("ATTACK!");
        float distance = Vector3.Distance(transform.position, Player.instance.transform.position);
       
        canAttack = false;

        //If within fov distance throw bomb
        if (distance > 25.0f)
        {
            Debug.LogError("Throw");
            ThrowProjectile();
        }
        else
        {
            StartCoroutine(ShieldBash());
        }
        yield return null;
    }

}

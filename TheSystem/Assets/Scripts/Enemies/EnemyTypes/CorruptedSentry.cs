//Kyle Zielinski
//2/1/2023
//This enemy agros and dashes at the player continuing to do so until the player 
//ends up outside of its view or attack range.

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using static UnityEditor.PlayerSettings;

public class CorruptedSentry : Enemy
{
    public GameObject spawn;
    private BoxCollider2D damageBox;
    //private IEnumerator enemyMoveThread;
    private Vector3 previousPlayerPos;
    private Collider2D playerBox;
    private bool isSearching;
    [SerializeField] private float agroTimer;
    private float escapeTimer;

    // Start is called before the first frame update
    void Start()
    {
        //Default value for Enemy
        canTakeDamage = true;
        canMove = true;
        walkSpeed = 1;
        healthPool = 10;
        attackDuration = 0.8f;
        attackCooldown = 2.0f;

        BoxCollider2D[] boxes = GetComponentsInChildren<BoxCollider2D>();
        damageBox = boxes[0];

        pos = transform.position;

        canAttack = false;
        attackCooldownFinished = true;

        viewRange = 20.0f;
        facingRight = false;
        isSearching = true;

        escapeTimer = 3.0f;
    }

    private void FixedUpdate()
    {
        if (isSearching)
        {
            Search();
        }

        //Insure that the enemy faces the player if the player is known to it.
        if(playerBox != null)
        {
            Vector3 directionVector = playerBox.transform.position - transform.position;
            facingRight = (directionVector.x > 0) ? true : false;
        }
    }

    //Function that has this enemy attack *through* the player
    protected override IEnumerator Attack()
    {
        StopCoroutine("SentryMove");
        isSearching = false;
        canAttack = false;
        Vector3 startPosition = transform.position;
        Vector3 currentPosition = new Vector3();
        Vector3 directionVector = playerBox.transform.position - startPosition;
        Vector3 endPosition = playerBox.transform.position + directionVector;

        Debug.DrawRay(transform.position, (directionVector) * (viewRange), Color.black);

        float currentTime = 0;

        while (currentTime < attackDuration)
        {
            currentPosition.x = Mathf.Lerp(startPosition.x, endPosition.x, currentTime);
            currentPosition.y = Mathf.Lerp(startPosition.y, endPosition.y, currentTime);
            transform.position = currentPosition;
            currentTime += Time.deltaTime;
            yield return null;
        }

        isSearching = true;
        StartCoroutine(AttackCooldown());
    }

    //Enemy looks around for player.
    protected override void Search()
    {
        RaycastHit2D hit;

        hit = SearchRays();

        //If it finds player move towards them
        if (hit.collider != null)
        {
            if (previousPlayerPos.magnitude < 0)
            {
                previousPlayerPos = hit.transform.position;
            }

            if (canMove)
            {
                StartCoroutine(SentryMove(hit));
            }

            previousPlayerPos = hit.transform.position;
        }

        //If it did not find player wait 3 seconds and then go back "home"
        else if (agroTimer >= escapeTimer)
        {
            transform.position = Vector3.MoveTowards(transform.position, spawn.transform.position, 0.05f);
        }
        else
        {
            agroTimer += Time.deltaTime;
        }
    }

    //Function for creating rubbish vision cone for corrupted sentry
    private RaycastHit2D SearchRays()
    {
        RaycastHit2D[] hits = new RaycastHit2D[7];

        int dF;

        if (facingRight)
        {dF = 1;}
        else
        {dF = -1;}

        //Raycast in correct direction
        for(int i = 0; i < hits.Length; i++)
        {
            float x = i;
            Vector3 direction = Vector3.zero;
            if (i > 0 && i < 3)
            {
                direction = new Vector3(1 * dF, -x/12, 0);
            }
            else
            {
                direction = new Vector3(1 * dF, (x - 3)/12, 0);
            }


            hits[i] = Physics2D.Raycast(
            transform.position + new Vector3(dF, 0, 0), //Origin
            direction, viewRange * 0.8f,                       //Direction
            playerLayerMask                             //What can get hit
            );
            Debug.DrawRay(transform.position, direction * (viewRange * 0.6f), Color.black);
        }

        RaycastHit2D hitValue = new RaycastHit2D();

        foreach (RaycastHit2D checkRay in hits)
        {
            if(checkRay.collider != null)
            {
                hitValue = checkRay;
            }
        }

        return hitValue;
    }

    //Function where enemy moves towards player before attacking
    private IEnumerator SentryMove(RaycastHit2D rayHit)
    {
        agroTimer = 0;
        canMove = false;
        Vector3 startPosition = transform.position;
        Vector3 currentPosition = new Vector3();
        Vector3 endPosition = rayHit.transform.position;

        float currentTime = 0;

        while (currentTime < walkSpeed)
        {
            currentPosition.x = Mathf.Lerp(startPosition.x, endPosition.x, currentTime);
            currentPosition.y = Mathf.Lerp(startPosition.y, endPosition.y, currentTime);
            transform.position = currentPosition;
            currentTime += Time.deltaTime;
            yield return null;
        }

        canMove = true;
    }

    //Collision trigger functions for enemy attacking player.
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            Debug.LogWarning("Found you!");
            agroTimer = 0;
            playerBox = collision;
            if (attackCooldownFinished)
            {
                attackCooldownFinished = false;
                StartCoroutine(Attack());
            }
        }
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            Debug.LogWarning("Found you!");
            agroTimer = 0;
            playerBox = collision;
            if(attackCooldownFinished)
            {
                attackCooldownFinished = false;
                StartCoroutine(Attack());
            }
        }
    }
}

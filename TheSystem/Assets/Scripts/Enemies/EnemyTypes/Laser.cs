using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : Enemy
{
    private BoxCollider2D damageBox;
    // Start is called before the first frame update
    void Start()
    {
        //Default value for Enemy
        canTakeDamage = false;

        canMove = true;
        walkSpeed = 1;
        healthPool = 1;

        BoxCollider2D[] boxes = GetComponentsInChildren<BoxCollider2D>();
        damageBox = boxes[0];

        pos = transform.position;
        attackDamage = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.LogWarning(collision.name);
        if(collision.name == "Player")
        {
            int x = 0;
            Player p = collision.GetComponent<Player>();
            p.PlayerTakeDamage(transform.position.x, 0);
        }
    }
}

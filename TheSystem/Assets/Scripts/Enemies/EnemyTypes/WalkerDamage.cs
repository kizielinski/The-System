using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkerDamage : MonoBehaviour
{
    public float damage = 2.0f;

    // Start is called before the first frame update
    void Start()
    {   
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Player.instance.PlayerTakeDamage(
                Utilities.DirectionToTarget(Player.instance.GetPos, transform.position).x, 
                1
            );
        }
    }
}

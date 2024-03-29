using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector3 target;
    private float speed;

    void Start()
    {
        target = Utilities.GetTargetPlayer();
        Debug.LogWarning("Target Located at:" + target);
        speed = 100;
        target = Utilities.DirectionToTarget(target, transform.position);
    }

    public float SetSpeed
    {
        set
        {speed = value;}
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(target * speed * Time.deltaTime, Space.World);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.LogWarning(collision.tag);

        if (collision.tag == "ground" || collision.tag == "Player")
        {
            if(collision.tag == "Player")
            {
                Player.instance.PlayerTakeDamage(Utilities.DirectionToTarget(collision.transform.position, transform.position).x, 1);
            }
            gameObject.SetActive(false);
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.LogWarning(collision.transform.tag);

        if (collision.transform.tag == "ground")
        {
            gameObject.SetActive(false);
        }
    }
}

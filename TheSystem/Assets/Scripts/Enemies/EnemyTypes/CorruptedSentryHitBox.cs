using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorruptedSentryHitBox : MonoBehaviour
{
    //CorruptedSentry parentInstance;
    Collider playerBox;

    public void OnTriggerEnter(Collider collision)
    {
        if (collision.name == "Player")
        {
            playerBox = collision;
        }
    }
}

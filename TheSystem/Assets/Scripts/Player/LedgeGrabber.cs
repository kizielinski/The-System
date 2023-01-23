using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeGrabber : MonoBehaviour
{
    private bool upperCastHit;
    private Vector2 upperCastStart;
    public float upperCastEnd;

    private bool lowerCastHit;
    private Vector2 lowerCastStart;
    public float lowerCastEnd;

    public Player player;
    public PlayerController controller;
    public LayerMask platformLayer;

    private bool drop;

    private bool canGrabLedge;
    public bool CanGrabLedge
    {
        get { return canGrabLedge; }
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //if you press S
        if (Input.GetKeyDown(KeyCode.S))
        {
            //you drop let go of the ledge
            drop = true;
            canGrabLedge = false;
        }
        
        if(!drop || controller.collisions.below)
        {
            upperCastStart = player.gameObject.transform.position + new Vector3(Mathf.Sign(player.Velocity.x) * player.gameObject.transform.localScale.x / 2, 1f);
            upperCastHit = Physics2D.Raycast(upperCastStart, Vector2.right * Mathf.Sign(player.Velocity.x), upperCastEnd, platformLayer);
            //Debug.DrawRay(upperCastStart, Vector2.right * Mathf.Sign(player.Velocity.x) * upperCastEnd, Color.blue);

            lowerCastStart = player.gameObject.transform.position + new Vector3(Mathf.Sign(player.Velocity.x) * player.gameObject.transform.localScale.x / 2, 0f);
            lowerCastHit = Physics2D.Raycast(lowerCastStart, Vector2.right * Mathf.Sign(player.Velocity.x), lowerCastEnd, platformLayer);
            //Debug.DrawRay(lowerCastStart, Vector2.right * Mathf.Sign(player.Velocity.x) * lowerCastEnd, Color.yellow);

            // Debug.Log(CheckForGrab(upperCastHit, lowerCastHit, controller.collisions.below));
            canGrabLedge = CheckForGrab(upperCastHit, lowerCastHit, controller.collisions.below);
            drop = false;
        }
        
        if((Input.GetKeyDown(KeyCode.A) && canGrabLedge) || (Input.GetKeyDown(KeyCode.D) && canGrabLedge))
        {
            ClimbLedge();
        }

        
    }

    public bool CheckForGrab(bool upperCast, bool lowerCast, bool grounded)
    {
        if(!upperCast && lowerCast && !grounded)
        {
            return true;
        }
        return false;
    }

    public void ClimbLedge()
    {
        if(Input.GetKey(KeyCode.A) && controller.collisions.left)
        {
            Debug.Log("test climb left");
            player.transform.position += new Vector3(-1, 1);
        }
        else if(Input.GetKey(KeyCode.D) && controller.collisions.right)
        {
            Debug.Log("test climb right");
            player.transform.position += new Vector3(1, 1);
        }
    }
}

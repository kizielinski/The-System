using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LedgeGrabber : MonoBehaviour
{
    //top end raycast variables
    private bool upperCastHit;
    private Vector2 upperCastStart;
    public float upperCastEnd;

    //bototm end raycast variables
    private bool lowerCastHit;
    private Vector2 lowerCastStart;
    public float lowerCastEnd;

    //player variables
    public Player player;
    public PlayerController controller;

    //platforms
    public LayerMask platformLayer;

    //if the player can grab the ledge
    private bool isGrabbingLedge;

    //getter
    public bool IsGrabbingLedge
    {
        get { return isGrabbingLedge; }
    }

    // Update is called once per frame
    void Update()
    {
        //if you press S
        if (Input.GetKeyDown(KeyCode.S) && isGrabbingLedge)
        {
            //the player is moved down slightly to disable the top end raycast check
            player.transform.position += new Vector3(0, -0.1f);
        }

        //sets start position of the upper raycast
        upperCastStart = player.gameObject.transform.position       //gets the position of the player
            + new Vector3(Mathf.Sign(player.Velocity.x)             //calculates where the player is looking and puts the start on that side of the player
            * player.gameObject.transform.localScale.x / 2, .52f);   //places the raycast start position at the top corner of the side the player is facing

        //draws a raycast from the starting position to the end position and returns a boolean value if that raycast has hit a platform
        upperCastHit = Physics2D.Raycast(upperCastStart, Vector2.right * Mathf.Sign(player.Velocity.x), upperCastEnd, platformLayer);

        //shows the upper raycast in blue
        Debug.DrawRay(upperCastStart, Vector2.right * Mathf.Sign(player.Velocity.x) * upperCastEnd, Color.blue);

        //sets the start position of the lower raycast
        lowerCastStart = player.gameObject.transform.position        //gets the position of the player
            + new Vector3(Mathf.Sign(player.Velocity.x)              //calculates where the player is looking and puts the start on that side of the player
            * player.gameObject.transform.localScale.x / 2, .48f);    //places the raycast start position at the middle of the side the player is facing

        //draws a raycast from the starting position to the end position and returns a boolean value if that raycast has hit a platform
        lowerCastHit = Physics2D.Raycast(lowerCastStart, Vector2.right * Mathf.Sign(player.Velocity.x), lowerCastEnd, platformLayer);

        //shows lower raycast in yellow
        Debug.DrawRay(lowerCastStart, Vector2.right * Mathf.Sign(player.Velocity.x) * lowerCastEnd, Color.yellow);

        //sets isGrabbingLedge to the bool value calculated by the CheckForGrab method
        //to be sent by the getter to the player script
        isGrabbingLedge = CheckForGrab(upperCastHit, lowerCastHit, controller.collisions.below);
        
        //if the player can grab a ledge and presses A or D
        if((Input.GetKeyDown(KeyCode.A) && isGrabbingLedge) || (Input.GetKeyDown(KeyCode.D) && isGrabbingLedge))
        {
            //run the ClimbLedgeMethod
            ClimbLedge();
        }

        
    }

    /// <summary>
    /// Checks for the ability for the player to grab a ledge
    /// </summary>
    /// <param name="upperCast">the upper raycast</param>
    /// <param name="lowerCast">the lower raycast</param>
    /// <param name="grounded">boolean value if the player is on the ground</param>
    /// <returns></returns>
    public bool CheckForGrab(bool upperCast, bool lowerCast, bool grounded)
    {
        //if the uppercast is not hitting a platform and the lower raycast is
        //and the player is not currently standing on the ground
        if(!upperCast && lowerCast && !grounded)
        {
            //the player can grab a ledge
            return true;
        }

        //the player cannot grab a ledge
        return false;
    }

    /// <summary>
    /// Method for climbing the ledge the player is grabbing
    /// </summary>
    public void ClimbLedge()
    {
        //if the player is colliding with a wall on the left and they press A
        if(Input.GetKey(KeyCode.A) && controller.collisions.left)
        {
            //the player climbs to the top of that ledge
            player.transform.position += new Vector3(-0.5f, 1);
        }
        //otherwise if the player is colliding with a wall on the right and presses D
        else if(Input.GetKey(KeyCode.D) && controller.collisions.right)
        {
            //the player climbs to the top of that ledge
            player.transform.position += new Vector3(0.5f, 1);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSlide : MonoBehaviour
{
    //private bools that can be toggled on or off
    [SerializeField]
    bool resetJump;
    [SerializeField]
    bool scriptIsActive;

    //getter for resetjump
    public bool ResetJump
    {
        get { return resetJump; }
    }

    //getter for wall sliding method
    public bool IsWallSliding
    {
        get { return WallSliding(); }
    }

    //references to player, controller, and ledge grabber
    Player player;
    PlayerController controller;
    LedgeGrabber ledge;

    // Start is called before the first frame update
    void Start()
    {
        //gets the components of each script
        player = GetComponent<Player>();
        controller = GetComponent<PlayerController>();
        ledge = GetComponent<LedgeGrabber>();
    }

    /// <summary>
    /// checks if the player is wall sliding
    /// </summary>
    /// <returns></returns>
    private bool WallSliding()
    {
        //if the script has been toggled to active
        if(scriptIsActive)
        {
            //if the player is holding into a wall, is not grabbing a ledge, and has a downward velocity
            if ((Input.GetKey(KeyCode.A) && controller.collisions.left && !controller.collisions.below && !ledge.IsGrabbingLedge && player.Velocity.y < 0)
            || (Input.GetKey(KeyCode.D) && controller.collisions.right && !controller.collisions.below && !ledge.IsGrabbingLedge && player.Velocity.y < 0))
            {
                //they are wall sliding
                return true;
            }
            //not sliding
            return false;
        }
        //not sliding
        return false;
    }
}

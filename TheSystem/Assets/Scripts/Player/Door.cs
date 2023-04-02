using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    //references to the player, the box collider of the door itself, 
    //and the trigger box for the door
    [SerializeField] Player player;
    [SerializeField] BoxCollider2D thisBox;
    [SerializeField] BoxCollider2D triggerBox;

    //values for the final open position of the door and closed position of the door
    [SerializeField] Vector3 closedPos;
    [SerializeField] Vector3 openedPos;

    //tunable value for how far away the player can open/close the door
    [SerializeField] float distanceTriggerable;

    //smooth lerping values
    private float current, target;

    //bools to check if the door is being opened/closed and
    //if the door is fully open or fully closed
    bool openingDoor;
    bool doorIsOpen;

    // Start is called before the first frame update
    void Start()
    {
        //setting start valoues
        current = 0; 
        target = 1;
        doorIsOpen = false;
    }

    // Update is called once per frame
    void Update()
    { 
        //if the player is within the box collider to activate the door and they press E
        if (PointInBoxCollider(player.transform.position, triggerBox) && Input.GetKeyDown(KeyCode.E))
        {
            //and if the player is not too close to the door collider itself then open the door
            if((player.transform.position.x > thisBox.transform.position.x + distanceTriggerable) || (player.transform.position.x < thisBox.transform.position.x - distanceTriggerable))
            openingDoor = true;
        }

        //if the door is being opened/closed
        if (openingDoor)
        {
            //if the door is currently closed when the door is activated
            if(!doorIsOpen)
            {
                //open the door
                current = Mathf.MoveTowards(current, target, 6 * Time.deltaTime);
                thisBox.transform.localPosition = Vector3.Lerp(closedPos, openedPos, current);

                //once the door has reached its open position
                if (thisBox.transform.localPosition == openedPos)
                {
                    //the door is open
                    openingDoor = false;
                    doorIsOpen = true;

                    //reset the value to be used again
                    current = 0;
                }
            }
            //else if the door is currently open when the door is activated
            else if (doorIsOpen)
            {
                //close the door
                current = Mathf.MoveTowards(current, target, 6 * Time.deltaTime);
                thisBox.transform.localPosition = Vector3.Lerp(openedPos, closedPos, current);

                //once the door has reached its closed position
                if (thisBox.transform.localPosition == closedPos)
                {
                    //the door is closed
                    openingDoor = false;
                    doorIsOpen = false;

                    //reset the value to be used again
                    current = 0;
                }
            }

        }
        
    }

    //method borrowed from Abubakar5415545 at https://forum.unity.com/threads/solved-use-oncollisionenter-or-ontriggerenter-without-rigidbody.539870/
    //thank you Abubakar <3
    bool PointInBoxCollider(Vector3 point, BoxCollider2D box)
    {
        //grabs the center of the box collider and the extents of the box collider
        Vector2 center = box.bounds.center;
        Vector2 extents = box.bounds.extents;

        //calculates the far corners of the box collider using the above calculated center and extents
        Vector2 rightTop = center + extents;
        Vector2 leftTop = center + new Vector2(-extents.x, extents.y);
        Vector2 rightBottom = center + new Vector2(extents.x, -extents.y);
        Vector2 leftBottom = center - extents;

        //checks if the player is within the calculated box and returns true or false
        if (point.x < rightTop.x && point.x > leftTop.x && point.y < rightTop.y && point.y > rightBottom.y)
            return true;
        return false;
    }
}

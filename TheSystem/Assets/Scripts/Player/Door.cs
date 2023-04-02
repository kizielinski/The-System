using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] BoxCollider2D thisBox;
    [SerializeField] BoxCollider2D triggerBox;

    [SerializeField] Vector3 closedPos;
    [SerializeField] Vector3 openedPos;

    [SerializeField] float distanceTriggerable;

    private float current, target;
    bool openingDoor;
    bool doorIsOpen;

    // Start is called before the first frame update
    void Start()
    {
        current = 0; 
        target = 1;
        doorIsOpen = false;
    }

    // Update is called once per frame
    void Update()
    { 
        if (PointInBoxCollider(player.transform.position, triggerBox) && Input.GetKeyDown(KeyCode.E))
        {
            if((player.transform.position.x > thisBox.transform.position.x + distanceTriggerable) || (player.transform.position.x < thisBox.transform.position.x - distanceTriggerable))
            openingDoor = true;
        }

        if (openingDoor)
        {
            if(!doorIsOpen)
            {
                current = Mathf.MoveTowards(current, target, 6 * Time.deltaTime);
                thisBox.transform.localPosition = Vector3.Lerp(closedPos, openedPos, current);
                Debug.Log("lerp");

                if (thisBox.transform.localPosition == openedPos)
                {
                    openingDoor = false;
                    doorIsOpen = true;
                    current = 0;
                }
            }
            else if (doorIsOpen)
            {
                current = Mathf.MoveTowards(current, target, 6 * Time.deltaTime);
                thisBox.transform.localPosition = Vector3.Lerp(openedPos, closedPos, current);
                Debug.Log("lerp");

                if (thisBox.transform.localPosition == closedPos)
                {
                    openingDoor = false;
                    doorIsOpen = false;
                    current = 0;
                }
            }

        }
        /*
        else if(!openDoor && doorIsOpen)
        {
            current = Mathf.MoveTowards(current, target, 6 * Time.deltaTime);
            thisBox.transform.position = Vector3.Lerp(openedPos, closedPos, current);
            Debug.Log("lerp");

            if (thisBox.transform.position == openedPos)
            {
                openDoor = false;
                doorIsOpen = false;
                current = 0;
            }
        }*/
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

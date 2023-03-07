using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplerPositionSet : MonoBehaviour
{
    //get reference to grapple script
    public Grappler grapple;

    //check for player entering the hook shot's range
    bool isActive;

    //get the spriteRenderer to change colors when in range
    SpriteRenderer spriteRenderer;

    // Set up the spriteRenderer
    private void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //if the player enters the box collider of the hook
        if(PointInBoxCollider(grapple.transform.position, gameObject.GetComponent<BoxCollider2D>()) && !isActive)
        {
            //activate the double check
            isActive = true;

            //enable the player's grapple script
            grapple.enabled = true;

            //set the new x and y points that the player will zip to
            grapple.PointX = gameObject.transform.position.x;
            grapple.PointY = gameObject.transform.position.y;

            //set the color of the grapple point to blue
            spriteRenderer.color = Color.blue;
        }
        //if the player exits the box collider of the hook
        else if (!PointInBoxCollider(grapple.transform.position, gameObject.GetComponent<BoxCollider2D>()) && isActive)
        {
            //diable the grapple script and secondary check
            grapple.enabled = false;
            isActive = false;

            //set the color of the grapple point to white
            spriteRenderer.color = Color.white;
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

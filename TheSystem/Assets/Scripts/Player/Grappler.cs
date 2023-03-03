using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grappler : MonoBehaviour
{
    //reference to the player
    public Player player;

    //public LineRenderer grapple;

    //the new calculated position
    Vector3 newPos;

    //the new points the player will zip to for the calculated position
    public float newPointX;
    public float newPointY;

    //the speeds of each component of the zip vector
    public float zipSpeedX;
    public float zipSpeedY;

    //the zip vector itself
    Vector3 zipVector;
    

    // Start is called before the first frame update
    void Start()
    {
        //make sure gapple is disabled so there's no accidental grapples
        //grapple.enabled = false;
        enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

        //if you click
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            //deactivate bottom collision so the grapple can take effect
            player.Controller.collisions.below = false;

            //calculate where the new position is based on the player and calculated points from GrapplerPositionSet script
            newPos = new Vector3(newPointX, newPointY);


            //set the line positions to draw
            //grapple.SetPosition(0, newPos);
            //grapple.SetPosition(1, transform.position);

            //draw the line renderer
           // grapple.enabled = true;

            //calculates vector from player to specified grapple point
            zipVector = new Vector3(0, 0, 0) - transform.localPosition + newPos;
        
            //shoot player in the direction of the zip vector by the x and y speeds
            player.Velocity = new Vector3(zipVector.normalized.x * zipSpeedX, zipVector.normalized.y * zipSpeedY);
            
        }

        //shows the zip vector
        if (Input.GetKey(KeyCode.Mouse0))
        {
            Debug.DrawRay(transform.position, zipVector);
        }
    }
}






/* the old grapple that i worked on that didnt make the cut i'm still mad about that if you're reading
 * this Karl I hate you just kidding you're my boss i love you i understand why it didn't make it 
 * please god don't fire me hahahahah

//temporary grapple code i needed velocity sorry
if(isGrappleOn)
{
    if (Input.GetKeyDown(KeyCode.Mouse0))
    {
        newPos = (Vector2)cameraObj.ScreenToWorldPoint(Input.mousePosition);
        grapple.SetPosition(0, newPos);
        grapple.SetPosition(1, transform.position);
        distance = Mathf.Sqrt(Mathf.Pow((transform.position.x - newPos.x), 2) + Mathf.Pow((transform.position.y - newPos.y), 2));

        grapple.enabled = true;
    }
    else if (Input.GetKeyUp(KeyCode.Mouse0))
    {
        grapple.enabled = false;
    }

    if (grapple.enabled)
    {
        grapple.SetPosition(1, transform.position);

        if (Vector2.Distance(transform.position, newPos) > distance)
        {
            velocity.x -= 20 * (transform.position.x - newPos.x) * Time.deltaTime;
        }

        if (Vector2.Distance(transform.position, newPos) > distance)
        {
            velocity.y -= 20 * (transform.position.y - newPos.y) * Time.deltaTime;
        }
    }
}
*/


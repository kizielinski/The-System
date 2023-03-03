using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grappler : MonoBehaviour
{
    public Player player;
    public LineRenderer grapple;

    public bool isGrappleOn;

    public float newPointX;
    public float newPointY;
    public float zipSpeedX;
    public float zipSpeedY;

    Vector2 newPos;
    float distance;

    // Start is called before the first frame update
    void Start()
    {
        grapple.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        //if its been allowed to work
        if (isGrappleOn)
        {
            //if you click
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {

                //calculate where the new position is based on the player and direction faced
                newPos = transform.position + new Vector3(newPointX * Mathf.Sign(player.Velocity.x), newPointY);



                //set the line positions to draw
                grapple.SetPosition(0, newPos);
                grapple.SetPosition(1, transform.position);

                //calculate the distance between the player position and the new position
                distance = Mathf.Sqrt(Mathf.Pow((transform.position.x - newPos.x), 2) + Mathf.Pow((transform.position.y - newPos.y), 2));

                //draw the line renderer
                grapple.enabled = true;
            }
            else if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                //if you release mouse1 then stop drawing the line
                grapple.enabled = false;
            }

            //if the line is being drawn
            if (grapple.enabled)
            {
                //set the position
                grapple.SetPosition(1, transform.position);

                //calculate a zip vector based on position and new position
                Vector3 zipVector = new Vector3(0, 0, 0) - transform.localPosition + (Vector3)newPos;


                

                //if the player is grounded
                if (player.Controller.collisions.below)
                {
                    //bump them up a little bit because the grapple wasnt a fan of the player being stuck to the ground
                    player.transform.position += new Vector3(0, 0.01f);
                }
                //if the player has reached the new position (close enough to it)
                if (Vector2.Distance(transform.position, newPos) < distance / 10)
                {
                    //set their position to the new position and zero out their velocity
                    transform.position = newPos;
                    player.Velocity = new Vector3(0, 0);
                }
                else
                {
                    //otherwise just keep adding velocities based on the calculated direction (zipvector)
                    player.Velocity += new Vector3((zipSpeedX * zipVector.x * Time.deltaTime), (zipSpeedY * zipVector.y * Time.deltaTime));

                    /* other possible way of zipping to point you can mess with in the y direction, 
                     * decide which one is best
                    velocity.y = 0;
                    velocity.y += (1000 * zipVector.y * Time.deltaTime);
                    */
                }
                //currently drawing the ray instead of the linerenderer for the sake of making sure zip rays would be correct
                Debug.DrawRay(transform.position, zipVector);
            }

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


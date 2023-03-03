using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grappler : MonoBehaviour
{
    public Player player;
    public LineRenderer grapple;


    public float newPointX;
    public float newPointY;
    public float zipSpeedX;
    public float zipSpeedY;
    Vector3 zipVector;

    Vector3 newPos;

    // Start is called before the first frame update
    void Start()
    {
        grapple.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
            //if you click
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {

                //calculate where the new position is based on the player and direction faced
                newPos = new Vector3(newPointX, newPointY);



                //set the line positions to draw
                grapple.SetPosition(0, newPos);
                grapple.SetPosition(1, transform.position);

                //draw the line renderer
                grapple.enabled = true;

                zipVector = new Vector3(0, 0, 0) - transform.localPosition + newPos;
            //player.transform.position += new Vector3(0, 0.1f);
            
            player.Velocity = zipVector.normalized*10;
            //player.Velocity += zipVector * new Vector3(zipSpeedX, zipSpeedY);
            //Debug.Log(zipVector.normalized * 60);
                
            }

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


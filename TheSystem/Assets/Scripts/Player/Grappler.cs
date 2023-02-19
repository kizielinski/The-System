using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grappler : MonoBehaviour
{
    public LineRenderer grapple;

    float distanceX;
    float distanceY;
    Vector2 mousPos;
    //temporary test case, will be removed in final 
    public Camera cameraObj;
    //will be used in the final version instead of mouse
    //Vector2 grappleStart;
    bool isScrapper;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            mousPos = (Vector2)cameraObj.ScreenToWorldPoint(Input.mousePosition);
            grapple.SetPosition(0, mousPos);
            grapple.SetPosition(1, transform.position);
            
            distanceX = (transform.position.x - mousPos.x);
            distanceY = Mathf.Pow((transform.position.y - mousPos.y), 2);
            grapple.enabled = true;
        }
        else if(Input.GetKeyUp(KeyCode.Mouse0))
        {
            grapple.enabled = false;
        }

        if(grapple.enabled)
        {
            grapple.SetPosition(1, transform.position);

            if(transform.position.x > distanceX)
            {
                transform.position = new Vector2(distanceX, transform.position.y);
            }
            if(transform.position.x < -distanceX)
            {
                transform.position = new Vector2(-distanceX, transform.position.y);
            }
           // if (transform.position.y > distanceY)
           // {
           //     transform.position = mousPos + new Vector2(distanceX, distanceY);
            //}
        }
    }
}
